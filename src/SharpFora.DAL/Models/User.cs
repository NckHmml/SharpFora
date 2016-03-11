using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.DAL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20), MinLength(2), Required]
        public string Name { get; set; }

        [MaxLength(200), MinLength(5), Required]
        public string Email { get; set; }

        [MaxLength(20), Required]
        public byte[] Password { get; set; }

        [MaxLength(20), Required]
        public byte[] Salt { get; set; }

        [MaxLength(10)]
        public byte[] TokenSecret { get; set; }

        public int LastToken { get; set; }

        [MaxLength(40)]
        public string SecurityStamp { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }

        internal ICollection<UserRole> UserRoles { get; }

        [NotMapped]
        public IEnumerable<Role> Roles
        {
            get
            {
                return UserRoles.Select(x => x.Role);
            }
        }
    }
}
