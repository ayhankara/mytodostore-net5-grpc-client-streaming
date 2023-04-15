using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyTodoStore.SupplierProduct.API.Services
{
    public interface ISupplierProductService
    {
        Task<int> ImportProductsAsync(IFormFile formFile);
    }
}