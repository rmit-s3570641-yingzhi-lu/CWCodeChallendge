using CW.Infrastructure.Interfaces;
using CW.Infrastructure.Models;
using CWCodeChallendge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CWCodeChallendge.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _repo;

        public ProductController(IProductRepository repo, ILogger<ProductController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var products = await _repo.ListAsync();
            var productsToReturn = new List<ProductDto>();
            products.ForEach(product => productsToReturn.Add(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price,
                IsActive = product.IsActive
            }));
            return View(productsToReturn);
        }

        public ActionResult Create()
        {
            // Return an empty form to submit
            return View(new ProductDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] ProductDto product)
        {
            try
            {
                var regex = new Regex(@"^\d+\.\d{2}?$"); // ^\d+(\.|\,)\d{2}?$ use this incase your dec separator can be comma or decimal.
                if (!regex.IsMatch(product.Price))
                {
                    ModelState.AddModelError(nameof(product.Price), "Price must have two demical places");
                }

                if (!Enum.TryParse<ProductTypes>(product.Type, out var pType))
                {
                    ModelState.AddModelError(nameof(product.Type), "Product Type must be in range");
                }

                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                await _repo.AddAsync(new Product
                {
                    Name = product.Name,
                    Type = pType,
                    Price = product.Price,
                    IsActive = product.IsActive
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(product);
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            var product = await _repo.GetByIdAsync(id);
            return View(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price,
                IsActive = product.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm]ProductDto product)
        {
            try
            {
                var regex = new Regex(@"^\d+\.\d{2}?$"); // ^\d+(\.|\,)\d{2}?$ use this incase your dec separator can be comma or decimal.
                if (!regex.IsMatch(product.Price))
                {
                    ModelState.AddModelError(nameof(product.Price), "Price must have two demical places");
                }

                if (!Enum.TryParse<ProductTypes>(product.Type, out var pType))
                {
                    ModelState.AddModelError(nameof(product.Type), "Product Type must be in range");
                }

                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                await _repo.UpdateAsync(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Type = pType,
                    Price = product.Price,
                    IsActive = product.IsActive
                });

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(product);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _repo.DeleteByIdAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }
    }
}
