using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanNuocGiaiKhat.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();

            TrangThaiDhs = new HashSet<TrangThaiDh>();
        }

        public int MaDh { get; set; }
        public int MaKh { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThaiHuyDon { get; set; }
        public bool ThanhToan { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string Note { get; set; }
        public int TongTien { get; set; }

        public virtual KhachHang MaKhNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual ICollection<TrangThaiDh> TrangThaiDhs { get; set; }
    }
}
