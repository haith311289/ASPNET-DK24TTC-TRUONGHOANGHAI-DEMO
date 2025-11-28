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
    public class AdminRoleController : Controller
    {
        private readonly CuaHangBanNuocGiaiKhatContext _context;

        public AdminRoleController(CuaHangBanNuocGiaiKhatContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoleAccounts.ToListAsync());
        }

    }
}
