using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using MyTodoStore.SupplierProduct.API.Models;

namespace MyTodoStore.SupplierProduct.API.Services
{
    public class SupplierProductService : ISupplierProductService
    {
        private readonly ProductGRPCService.ProductGRPCServiceClient _productGRPCServiceClient;

        public SupplierProductService(ProductGRPCService.ProductGRPCServiceClient productGRPCServiceClient)
        {
            _productGRPCServiceClient = productGRPCServiceClient;
        }

        public async Task<int> ImportProductsAsync(IFormFile formFile)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using var importProductStream = _productGRPCServiceClient.ImportProductsStream();

            using (var reader = new StreamReader(formFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, config))
            {
                IAsyncEnumerable<SupplierProductModel> products = csv.GetRecordsAsync<SupplierProductModel>();

                await foreach (SupplierProductModel product in products)
                {
                    ImportProductRequest importProductRequest = new()
                    {
                        SupplierId = product.SupplierID,
                        Sku = product.SKU,
                        Name = product.Name,
                        Description = product.Description,
                        Brand = product.Brand
                    };

                    await importProductStream.RequestStream.WriteAsync(importProductRequest);
                }
            }
            await importProductStream.RequestStream.CompleteAsync();

            ImportProductResponse response = await importProductStream;

            return response.Count;
        }
    }
}
