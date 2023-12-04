using ExternalService.Common;
using ExternalService.Data;
using ExternalService.Data.Entities;
using ExternalService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExternalService.Controllers
{
    [EnableCors("PolicyCore")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromQuery] string? filter)
        {
            try
            {
                IQueryable<Category> query = _context.tblCategory.Include(c => c.Products).AsQueryable();

                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(c => c.StrName.Contains(filter));
                }

                var categoriesWithProducts = await query.ToListAsync();

                var response = categoriesWithProducts.Select(category => new
                {
                    Id = category.Id,
                    Name = category.StrName,
                    Active = category.BiActive,
                    Products = category.Products.Select(product => new
                    {
                        Id = product.Id,
                        Name = product.StrName,
                        Price = product.DePrice,
                        Image = product.StrImageUrl,
                        Active = product.BiActive
                    }).ToList()
                }).ToList();

                return Ok(new
                {
                    categories = response
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryModel model)
        {
            try
            {
                var validationErrors = DataValidator.ValidateModel(model);

                if (validationErrors.Any())
                {
                    return BadRequest(new { message = string.Join(", ", validationErrors) });
                }

                // Validar si ya existe una categoría con este nombre
                if (await _context.tblCategory.AnyAsync(c => c.StrName == model.Name))
                {
                    return BadRequest(new { Message = "Ya existe una categoría con el mismo nombre" });
                }

                // Crear la nueva categoría
                var newCategory = new Category
                {
                    Id = Guid.NewGuid(),
                    StrName = model.Name.ToUpper(),
                    BiActive = model.Active,
                    Products = new List<Product>()
                };

                // Agregar productos a la nueva categoría si se proporcionan en el modelo
                if (model.Products != null && model.Products.Any())
                {
                    foreach (var productModel in model.Products)
                    {
                        var newProduct = new Product
                        {
                            Id = Guid.NewGuid(),
                            CategoryFK = newCategory.Id,
                            StrName = productModel.Name,
                            DePrice = productModel.Price,
                            StrImageUrl = productModel.ImageUrl,
                            BiActive = productModel.Active
                        };

                        newCategory.Products.Add(newProduct);
                    }
                }

                // Agregar la nueva categoría al contexto y guardar cambios
                await _context.tblCategory.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }
    }
}
