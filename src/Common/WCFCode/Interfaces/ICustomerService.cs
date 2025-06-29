namespace Interfaces
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        List<Customer> GetAllCustomers();
        
        [OperationContract]
        Customer GetCustomerById(int customerId);
        
        [OperationContract]
        Customer GetCustomerByEmail(string email);
        
        [OperationContract]
        bool AddCustomer(Customer customer);
        
        [OperationContract]
        bool UpdateCustomer(Customer customer);
        
        [OperationContract]
        bool DeleteCustomer(int customerId);
    }
}