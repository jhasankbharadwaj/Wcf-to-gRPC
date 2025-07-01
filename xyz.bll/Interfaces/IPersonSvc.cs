using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.bll;
using xyz.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace xyz.bll.Interfaces
{
    public interface IPersonSvc
    {
        
        Task<Person> GetPersonByIdAsync(int id);

        Task<List<Person>> GetAllPersonsAsync();

        Task<int> AddPersonAsync(Person person);

        Task<bool> UpdatePersonAsync(Person person);

        Task<bool> DeletePersonAsync(int id);
    }
}
