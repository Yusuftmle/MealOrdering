﻿using MealOrdering.Shared.DTO;

namespace MealOrdering.Server.Services.Infratructure
{
    public interface ISupplierService
    {
        public Task<List<SupplierDTO>> GetSuppliers();

        public Task<SupplierDTO> CreateSupplier(SupplierDTO Order);

        public Task<SupplierDTO> UpdateSupplier(SupplierDTO Order);

        public Task DeleteSupplier(Guid SupplierId);

        public Task<SupplierDTO> GetSupplierById(Guid Id);
    }
}

