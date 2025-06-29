using System.Collections.Generic;
using System.ServiceModel;
using GamesApplication.Models;

namespace Interfaces
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        List<Product> GetAllProducts();
        
        [OperationContract]
        Product GetProductById(int productId);
        
        [OperationContract]
        List<Product> GetProductsByCategory(string category);
        
        [OperationContract]
        bool AddProduct(Product product);
        
        [OperationContract]
        bool UpdateProduct(Product product);
        
        [OperationContract]
        bool DeleteProduct(int productId);
        
        [OperationContract]
        bool UpdateStock(int productId, int quantity);
    }
}
