using Grpc.Core;

namespace GrpcServer.Services
{
    public class ProductService : Product.ProductBase
    {
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public override Task<ProductModel> GetProductInfo(ProductLookupModel request, ServerCallContext context)
        {
            ProductModel output = new();

            _logger.LogInformation($"Requested: {context.Method}");
            if (request.ProductId == 1)
            {
                output.CreationDate = DateTime.Now.ToShortDateString();
                output.Desc = "ProductId 1";
                output.Name = request.ProductId.ToString() + "Product ";
            }
            else if (request.ProductId == 2)
            {
                output.CreationDate = DateTime.Now.AddDays(-1).ToShortDateString();
                output.Desc = request.ProductId.ToString();
                output.Name = request.ProductId.ToString() + "Product";
            }
            else
            {
                throw new ArgumentOutOfRangeException($"ProductId invalid: {request.ProductId.ToString()}");
            }

            return Task.FromResult(output);
        }

        public override async Task CreateNewProduct(NewProductModel request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            List<ProductModel> products = new List<ProductModel>(10);

            for (int i = 0; i < 10; i++)
            {
                products.Add(new ProductModel
                {
                    Name = "Product " + i.ToString(),
                    CreationDate = DateTime.Now.AddHours(i).ToShortDateString(),
                    Desc = "Desc " + i.ToString(),
                    Quantity = i,
                    IsActive = true
                });
            }

            foreach (var prod in products)
            {
                await Task.Delay(2000);
                await responseStream.WriteAsync(prod);
            }
        }
    }
}
