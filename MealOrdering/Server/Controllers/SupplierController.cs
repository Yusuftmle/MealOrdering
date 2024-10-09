using MealOrdering.Server.Services.Infratructure;
using MealOrdering.Shared.DTO;
using MealOrdering.Shared.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealOrdering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierService supplierService;

        public SupplierController(ISupplierService SupplierService)
        {
            supplierService = SupplierService;
        }



        [HttpGet("SupplierById/{Id}")]
        public async Task<ServiceResponse<SupplierDTO>> GetSupplierById(Guid Id)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await supplierService.GetSupplierById(Id)
            };
        }


        [HttpGet("Suppliers")]
        public async Task<ServiceResponse<List<SupplierDTO>>> GetSuppliers()
        {
            return new ServiceResponse<List<SupplierDTO>>()
            {
                Value = await supplierService.GetSuppliers()
            };
        }


        [HttpPost("CreateSupplier")]
        public async Task<ServiceResponse<SupplierDTO>> CreateSupplier(SupplierDTO Supplier)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await supplierService.CreateSupplier(Supplier)
            };
        }


        [HttpPost("UpdateSupplier")]
        public async Task<ServiceResponse<SupplierDTO>> UpdateSupplier(SupplierDTO Supplier)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await supplierService.UpdateSupplier(Supplier)
            };
        }


        [HttpPost("DeleteSupplier")]
        public async Task<BaseResponse> DeleteSupplier([FromBody] Guid SupplierId)
        {
            await supplierService.DeleteSupplier(SupplierId);
            return new BaseResponse();
        }
    }

}
