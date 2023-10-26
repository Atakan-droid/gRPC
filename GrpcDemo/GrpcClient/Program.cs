using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await GreetProtoAsync();

            //await GetProductAsync();

            var channel = GrpcChannel.ForAddress("https://localhost:7248");
            var client = new Product.ProductClient(channel);

            var clientReq = new NewProductModel();

            var product = client.CreateNewProduct(clientReq);

            using var call= client.CreateNewProduct(clientReq);

            while (await call.ResponseStream.MoveNext())
            {
                var current= call.ResponseStream.Current;
                Console.WriteLine(current);
            }

            Console.ReadLine();
        }

        private static async Task GetProductAsync()
        {
            var input = int.Parse(Console.ReadLine());
            var channel = GrpcChannel.ForAddress("https://localhost:7248");
            var client = new Product.ProductClient(channel);

            var model = new ProductLookupModel()
            {
                ProductId = input
            };
            var reply = await client.GetProductInfoAsync(model);
            Console.WriteLine(reply.ToString());
        }

        private static async Task GreetProtoAsync()
        {
            Console.WriteLine("Input waiting..");
            string getName = Console.ReadLine().ToString();

            var input = new HelloRequest()
            {
                Name = getName
            };

            var channel = GrpcChannel.ForAddress("https://localhost:7248");
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(input);
            Console.WriteLine(reply.ToString());
        }
    }
}