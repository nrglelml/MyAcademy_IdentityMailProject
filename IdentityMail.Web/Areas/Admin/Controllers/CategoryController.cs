using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class CategoryController(AppDbContext _context) : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
        .OrderBy(c => c.Name)
        .Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            ColorHex = c.ColorHex
        })
        .ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(new Category
                {
                    Name = category.Name,
                    ColorHex = category.ColorHex
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Categories
      .OrderBy(c => c.Name)
      .Select(c => new CategoryDto
      {
          Id = c.Id,
          Name = c.Name,
          ColorHex = c.ColorHex
      }).FirstOrDefaultAsync(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    ColorHex = category.ColorHex
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
