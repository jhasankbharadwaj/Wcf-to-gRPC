using models;
using interfaces;
namespace Services
{
    public class CustomerBusinessLogic
    {
        private readonly CustomerRepository customerRepository;

        public CustomerBusinessLogic()
        {
            customerRepository = new CustomerRepository();
        }

        public List<Customer> GetAllCustomers()
        {
            return customerRepository.GetAllCustomers();
        }

        public Customer GetCustomerById(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID must be greater than 0");

            return customerRepository.GetCustomerById(customerId);
        }

        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty");

            return customerRepository.GetCustomerByEmail(email);
        }

        public bool AddCustomer(Customer customer)
        {
            ValidateCustomer(customer);

            // Check if email already exists
            var existingCustomer = customerRepository.GetCustomerByEmail(customer.Email);
            if (existingCustomer != null)
                throw new ArgumentException("Customer with this email already exists");

            return customerRepository.AddCustomer(customer);
        }

        public bool UpdateCustomer(Customer customer)
        {
            ValidateCustomer(customer);
            if (customer.CustomerId <= 0)
                throw new ArgumentException("Customer ID must be greater than 0");

            return customerRepository.UpdateCustomer(customer);
        }

        public bool DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID must be greater than 0");

            return customerRepository.DeleteCustomer(customerId);
        }

        private void ValidateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("Customer cannot be null");
            if (string.IsNullOrEmpty(customer.FirstName))
                throw new ArgumentException("First name is required");
            if (string.IsNullOrEmpty(customer.LastName))
                throw new ArgumentException("Last name is required");
            if (string.IsNullOrEmpty(customer.Email))
                throw new ArgumentException("Email is required");
            if (!IsValidEmail(customer.Email))
                throw new ArgumentException("Invalid email format");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
