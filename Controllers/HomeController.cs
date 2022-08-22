using Microsoft.AspNetCore.Mvc;
using FoodCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;




namespace FoodCatalog.Controllers
{
    public class HomeController : Controller
    {

            private ApplicationContext db;
            public HomeController(ApplicationContext context)
            {
                db = context;
            }

            public ActionResult ViewProducts(int? categoryid, string prodname)
            {
                List<Categorys> category = db.Category.ToList();
                IQueryable<Products> product = db.Products.Include(u => u.Categorys);
                if (categoryid != null && categoryid != 0)
                {
                    product = product.Where(u => u.Categorys.Id == categoryid);
                }
                if (!String.IsNullOrEmpty(prodname))
                {
                    product = product.Where(u => u.ProductName.Contains(prodname));

                }
                SearchM mod = new SearchM
                {
                    Categor = new SelectList(category, "Id", "CategoryName"),
                    Product = product.ToList()
                };

                return View(mod);
            }

            public async Task<IActionResult> ViewCategoriesList()
            {
                return View(await db.Category.ToListAsync());
            }


        [Authorize(Roles = "admin, user, advanced")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            return RedirectToAction("ViewProducts");
        }


        public IActionResult CreateCategory()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateCategory(Categorys category)
            {
                db.Category.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewCategoriesList");
            }

            public async Task<IActionResult> EditCategory(int? id)
            {
                if (id != null)
                {
                    Categorys category = await db.Category.FirstOrDefaultAsync(p => p.Id == id);
                    if (category != null)
                        return View(category);
                }
                return NotFound();
            }
            [HttpPost]
            public async Task<IActionResult> EditCategory(Categorys category)
            {
                db.Category.Update(category);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewCategoriesList");
            }




            [HttpGet]
            [ActionName("DeleteCategory")]
            public async Task<IActionResult> ConfirmDelete(int? id)
            {
                if (id != null)
                {
                    Categorys category = await db.Category.FirstOrDefaultAsync(p => p.Id == id);
                    if (category != null)
                        return View(category);
                }
                return NotFound();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteCategory(int? id)
            {
                if (id != null)
                {
                    Categorys category = await db.Category.FirstOrDefaultAsync(p => p.Id == id);
                    if (category != null)
                    {
                        db.Category.Remove(category);
                        await db.SaveChangesAsync();
                        return RedirectToAction("ViewCategoriesList");
                    }
                }
                return NotFound();
            }


            [HttpGet]
            [ActionName("DeleteProduct")]
            public async Task<IActionResult> ConfirmDeleteP(int? id)
            {
                if (id != null)
                {
                    Products product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                    if (product != null)
                        return View(product);
                }
                return NotFound();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteProduct(int? id)
            {
                if (id != null)
                {
                    Products product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        await db.SaveChangesAsync();
                        return RedirectToAction("ViewProducts");
                    }
                }
                return NotFound();
            }

            public async Task<IActionResult> EditProduct(int? id)
            {
                List<Categorys> category = db.Category.ToList();

                if (id != null)
                {
                    Products product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                    if (product != null)
                    {
                    SelectForEditting mod1 = new SelectForEditting
                    {
                            Categor = new SelectList(category, "Id", "CategoryName"),
                            Product = product
                        };
                        return View(mod1);
                    }
                }
                return NotFound();
            }
            [HttpPost]
            public async Task<IActionResult> EditProduct(SelectForEditting mod)
            {
                db.Products.Update(mod.Product);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewProducts");
            }



            public IActionResult AddProduct()
            {
                List<Categorys> category = db.Category.ToList();
            SelectForEditting mod1 = new SelectForEditting
            {
                    Categor = new SelectList(category, "Id", "CategoryName"),
                    Product = new Products()
                };
                return View(mod1);
            }

            [HttpPost]
            public async Task<IActionResult> AddProduct(SelectForEditting mod)
            {
                db.Products.Add(mod.Product);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewProducts");
            }
       }

    }
