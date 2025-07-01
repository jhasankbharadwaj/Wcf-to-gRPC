using System.Collections.Generic;
using xyz.Shared;
using xyz.Shared.Models;
using xyz.bll.Interfaces;
using xyz.bll;
using xyz.Data;

namespace xyz.Server
{
    public class PersonService : IPersonService
    {
        private readonly IPersonSvc _personSvc;

        public PersonService()
        {
            // Normally you'd use Dependency Injection; keeping it simple here
            _personSvc = new PersonSvc(new SqlDataServices());
        }

        public Person GetPersonById(int id)
        {
            // Async -> Sync bridge
            return _personSvc.GetPersonByIdAsync(id).GetAwaiter().GetResult();
        }

        public List<Person> GetAllPersons()
        {
            return _personSvc.GetAllPersonsAsync().GetAwaiter().GetResult();
        }

        public void AddPerson(Person person)
        {
            _personSvc.AddPersonAsync(person).GetAwaiter().GetResult();
        }

        public void UpdatePerson(Person person)
        {
            _personSvc.UpdatePersonAsync(person).GetAwaiter().GetResult();
        }

        public bool DeletePerson(int id)
        {
            return _personSvc.DeletePersonAsync(id).GetAwaiter().GetResult();
        }
    }
}
