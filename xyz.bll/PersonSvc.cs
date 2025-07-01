using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.bll.Interfaces;

namespace xyz.bll
{
    public class PersonSvc : IPersonSvc
    {
        private readonly SqlDataServices _sqlDataServices;

        public PersonSvc(SqlDataServices sqlDataServices)
        {
            _sqlDataServices = sqlDataServices;
        }

        public Task<Person> GetPersonByIdAsync(int id)
            => _sqlDataServices.GetPersonByIdAsync(id);

        public Task<List<Person>> GetAllPersonsAsync()
            => _sqlDataServices.GetAllPersonsAsync();

        public Task<int> AddPersonAsync(Person person)
            => _sqlDataServices.AddPersonAsync(person);

        public Task<bool> UpdatePersonAsync(Person person)
            => _sqlDataServices.UpdatePersonAsync(person);

        public Task<bool> DeletePersonAsync(int id)
            => _sqlDataServices.DeletePersonAsync(id);
    }

}
