using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace xyz.Shared
{
    [ServiceContract]
    public interface IPersonService
    {
        [OperationContract]
        Person GetPersonById(int id);

        [OperationContract]
        List<Person> GetAllPersons();

        [OperationContract]
        void AddPerson(Person person);

        [OperationContract]
        void UpdatePerson(Person person);

        [OperationContract]
        bool DeletePerson(int id);
    }

  
}
