using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanNuocGiaiKhat.Models
{
    public partial class RoleAccount
    {
        public RoleAccount()
        {
            Accounts = new HashSet<Account>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Mota { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
