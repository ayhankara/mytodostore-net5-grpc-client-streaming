using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTodoStore.SupplierProduct.API.Services;

namespace MyTodoStore.SupplierProduct.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ISupplierProductService _supplierProductService;

        public ProductController(ISupplierProductService supplierProductService)
        {
            _supplierProductService = supplierProductService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadProducts(IFormFile formFile)
        {
            int importedProductCount = await _supplierProductService.ImportProductsAsync(formFile);

            return Ok($"{importedProductCount} products have been imported.");
        }
    }
}
