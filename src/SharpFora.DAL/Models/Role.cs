using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpFora.DAL.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20), MinLength(2), Required]
        public string Name { get; set; }

        internal ICollection<UserRole> UserRoles { get; }

        [NotMapped]
        public IEnumerable<User> Users
        {
            get
            {
                return UserRoles.Select(x => x.User);
            }
        }
    }
}
