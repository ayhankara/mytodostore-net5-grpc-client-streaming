using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace MyTodoStore.Product.GRPC
{
    public class ProductService : ProductGRPCService.ProductGRPCServiceBase
    {
        public override async Task<ImportProductResponse> ImportProductsStream(IAsyncStreamReader<ImportProductRequest> requestStream, ServerCallContext context)
        {
            var importResponse = new ImportProductResponse();

            await foreach (var importProductItem in requestStream.ReadAllAsync())
            {
                // product import operations...

                importResponse.Count += 1;
                Console.WriteLine($"1 product has been imported. SKU: {importProductItem.Sku} Brand: {importProductItem.Brand}");
            }

            Console.WriteLine("Import products stream has been ended.");

            return importResponse;
        }
    }
}
