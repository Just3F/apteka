using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Apteka.Models;
using Apteka.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Apteka.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;
        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult AddToOrder(int productId)
        {
            var newProduct = new UserProduct()
            {
                ProductId = productId,
                UserId = GetUserId()
            };
            var existProduct = dbContext.UserProducts.FirstOrDefault(x => x.UserId == newProduct.UserId && x.ProductId == newProduct.ProductId);

            if (existProduct != null)
            {
                existProduct.Count++;
            }
            else
            {
                newProduct.Count = 1;
                dbContext.UserProducts.Add(newProduct);
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteFromOrder(int productId)
        {
            var existProduct = dbContext.UserProducts.FirstOrDefault(x => x.UserId == GetUserId() && x.ProductId == productId);

            if (existProduct != null)
            {
                if (existProduct.Count > 0)
                    existProduct.Count--;

                if (existProduct.Count < 1)
                    dbContext.UserProducts.Remove(existProduct);

            }

            dbContext.SaveChanges();

            return RedirectToAction("Cart", "Home");
        }

        private int GetUserId()
        {
            int userId = 0;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userId = Convert.ToInt32(userIdClaim.Value);
                }
            }

            return userId;
        }

        public IActionResult Cart()
        {
            var products = dbContext.UserProducts.Where(x => x.UserId == GetUserId()).Include(x => x.Product).Include(x => x.User).ToList();
            return View(products);
        }

        public IActionResult Index()
        {
            var products = dbContext.Products.ToList();
            return View(products);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
