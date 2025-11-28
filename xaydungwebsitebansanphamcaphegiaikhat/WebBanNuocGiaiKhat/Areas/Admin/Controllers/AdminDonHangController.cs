using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanNuocGiaiKhat.Models;

namespace WebBanNuocGiaiKhat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonHangController : Controller
    {
        private readonly CuaHangBanNuocGiaiKhatContext _context;

        public AdminDonHangController(CuaHangBanNuocGiaiKhatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDonHang
        public async Task<IActionResult> Index()
        {
            var cuaHangBanNuocGiaiKhatContext = _context.DonHangs.Include(d => d.MaKhNavigation).OrderByDescending(x => x.NgayTao);
            return View(await cuaHangBanNuocGiaiKhatContext.ToListAsync());
        }

        // GET: Admin/AdminDonHang/Details/5
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
                .Include(d => d.TrangThaiDhs)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int MaDh, string TrangThai, string Mota)
        {
            if (ModelState.IsValid)
            {
                // Tìm các trạng thái cũ của đơn hàng này
                var oldStatuses = _context.TrangThaiDhs.Where(x => x.MaDh == MaDh);
                // Xóa hết trạng thái cũ
                if (oldStatuses.Any())
                {
                    _context.TrangThaiDhs.RemoveRange(oldStatuses);
                }

                // Thêm trạng thái mới
                var trangThaiDh = new TrangThaiDh
                {
                    MaDh = MaDh,
                    TrangThai = TrangThai,
                    Mota = Mota
                };
                _context.Add(trangThaiDh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = MaDh });
            }
            return RedirectToAction(nameof(Details), new { id = MaDh });
        }
    }
}
