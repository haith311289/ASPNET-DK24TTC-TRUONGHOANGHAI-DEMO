using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanNuocGiaiKhat.Models;
using Microsoft.AspNetCore.Http;

namespace WebBanNuocGiaiKhat.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly CuaHangBanNuocGiaiKhatContext _context;

        public DonHangsController(CuaHangBanNuocGiaiKhatContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var maKhStr = HttpContext.Session.GetString("MaKh");
            if (string.IsNullOrEmpty(maKhStr))
            {
                return RedirectToAction("Login", "Home");
            }

            if (int.TryParse(maKhStr, out int maKh))
            {
                var donHangs = await _context.DonHangs
                    .AsNoTracking()
                    .Where(d => d.MaKh == maKh)
                    .OrderByDescending(d => d.NgayTao)
                    .ToListAsync();
                return View(donHangs);
            }
            
            return RedirectToAction("Login", "Home");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }


        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDh,MaKh,NgayTao,TrangThaiHuyDon,ThanhToan,NgayThanhToan,Note")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDh,MaKh,NgayTao,TrangThaiHuyDon,ThanhToan,NgayThanhToan,Note")] DonHang donHang)
        {
            if (id != donHang.MaDh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
        }


        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDh == id);
        }
    }
}
