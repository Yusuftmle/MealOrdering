using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Shared.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }

        public string FulName => $"{FirstName} {LastName}";
    }
}
