using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanNuocGiaiKhat.Models;

namespace WebBanNuocGiaiKhat.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly CuaHangBanNuocGiaiKhatContext _context;

        public SanPhamController(CuaHangBanNuocGiaiKhatContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());
        }

        // Search products
        public async Task<IActionResult> Search(string keyword)
        {
            ViewBag.Keyword = keyword;
            
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return RedirectToAction("Index");
            }

            var products = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .Where(s => s.TenSp.ToLower().Contains(keyword.ToLower()) && s.TrangThai == true)
                .ToListAsync();

            ViewBag.SearchCount = products.Count;
            return View(products);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            
            if (sanPham == null)
            {
                return NotFound();
            }

            // Load best seller products for recommendations
            ViewBag.BestSellers = await _context.SanPhams
                .Where(x => x.BestSeller == true && x.TrangThai == true)
                .Take(8)
                .ToListAsync();

            return View(sanPham);
        }

       public async Task<IActionResult> SuaTT()
        {
            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());
        }

        public async Task<IActionResult> SuaChua()
        {
            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());
        }

        public async Task<IActionResult> BoPhomat()
        {
            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());
        }


        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
