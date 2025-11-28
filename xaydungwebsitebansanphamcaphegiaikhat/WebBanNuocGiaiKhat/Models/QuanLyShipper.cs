using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanNuocGiaiKhat.Models
{
    public partial class QuanLyShipper
    {
        public int MaShipper { get; set; }
        public int MaDh { get; set; }
        public string TenShipper { get; set; }
        public DateTime NgayLayHang { get; set; }
        public int Phone { get; set; }
        public string TenCongty { get; set; }

        public virtual DonHang MaDhNavigation { get; set; }
    }
}
