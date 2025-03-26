using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    class Admin
    {
        private int AdminId { get; set; }
        private int UserId { get; set; }

        public Admin(int adminId, int userId)
        {
            AdminId = adminId;
            UserId = userId;
        }
    }
}
