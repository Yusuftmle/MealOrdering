using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Shared.DTO
{
    public class SupplierDTO
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public bool IsActive { get; set; }

    }
}
