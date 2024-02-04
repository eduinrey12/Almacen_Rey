using Grpc.Core;

namespace Almacen_Rey.Services
{
    public class AlmacenService: Almacen.AlmacenBase
    {
        private static Dictionary<string, Producto> _products = new Dictionary<string, Producto>();
        public override Task<ProductID> AddProducttById(Producto request, ServerCallContext context)
        {
            _products[request.Id] = request;
            return Task.FromResult(new ProductID { Id = request.Id });
        }

        public override Task<Producto> UpdateProducttById(Producto request, ServerCallContext context)
        {
            if (_products.ContainsKey(request.Id))
            {
                _products[request.Id] = request;
                return Task.FromResult(request);
            }
            else
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Id} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Producto());
            }
        }

        public override Task<Producto> GetProducttById(ProductID request, ServerCallContext context)
        {
            if (_products.TryGetValue(request.Id, out Producto product) && product != null)
            {
                return Task.FromResult(product);
            }
            else
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Id} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Producto());
            }
        }

        public override Task<Producto> GetProducttByName(ProductName request, ServerCallContext context)
        {
            Producto matchingProduct = null;
            foreach (var product in _products.Values)
            {
                if (product.Nombre == request.Nombre)
                {
                    matchingProduct = product;
                    break;
                }
            }
            if (matchingProduct != null)
            {
                return Task.FromResult(matchingProduct);
            }
            else
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Nombre} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Producto());
            }
        }
    }
}
