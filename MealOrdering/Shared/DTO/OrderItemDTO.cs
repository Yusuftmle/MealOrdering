﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Shared.DTO
{
    public class OrderItemDTO
    {
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid CreatedUserId { get; set; }

        public Guid OrderId { get; set; }

        public String Description { get; set; }

        public string OrderName { get; set; }
        public string CreatedUserFullName { get; set; }
    }
}
