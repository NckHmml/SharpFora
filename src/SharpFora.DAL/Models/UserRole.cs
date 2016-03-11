using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.DAL.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; }
    }
}
