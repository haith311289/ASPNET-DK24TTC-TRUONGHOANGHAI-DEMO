using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanNuocGiaiKhat.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebBanNuocGiaiKhat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomersController : Controller
    {
        private readonly CuaHangBanNuocGiaiKhatContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminCustomersController(CuaHangBanNuocGiaiKhatContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.KhachHangs.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/AdminCustomers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKh,TenKh,GioiTinh,Diachi,Ngaysinh,Phone,Email,Password,CreateDate")] KhachHang khachHang, IFormFile fAvatar)
        {
            if (id != khachHang.MaKh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(x => x.MaKh == id);

                    if (fAvatar != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(fAvatar.FileName);
                        string extension = Path.GetExtension(fAvatar.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/images/customers/", fileName);
                        
                        // Ensure directory exists
                        var directory = Path.GetDirectoryName(path);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await fAvatar.CopyToAsync(fileStream);
                        }
                        khachHang.AvatarKh = fileName;
                    }
                    else
                    {
                        khachHang.AvatarKh = existingCustomer.AvatarKh;
                    }

                    // Preserve password if not changed
                    if (string.IsNullOrEmpty(khachHang.Password))
                    {
                        khachHang.Password = existingCustomer.Password;
                    }

                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh))
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
            return View(khachHang);
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }

    }
}
