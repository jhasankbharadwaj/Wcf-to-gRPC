// OldLeaf.OldKey.BLL/Person/PersonDirProxyImpl.cs
using System;
using System.Collections.Generic;
using Com.OldLeaf.Shared.Person;
using OldLeaf.Services;

namespace OldLeaf.OldKey.BLL.Person
{
    public class PersonDirProxyImpl : IPersonDirProxy
    {
        private readonly SqlDataServices _dataServices;

        public PersonDirProxyImpl()
        {
            _dataServices = new SqlDataServices();
        }

        public PersonDirProxyImpl(string connectionString)
        {
            _dataServices = new SqlDataServices(connectionString);
        }

        public Com.OldLeaf.Shared.Person.Person Get(int id)
        {
            return _dataServices.Get<Com.OldLeaf.Shared.Person.Person>(id);
        }

        public List<Com.OldLeaf.Shared.Person.Person> Search(string criteria)
        {
            return _dataServices.Search<Com.OldLeaf.Shared.Person.Person>(criteria);
        }

        public void Save(Com.OldLeaf.Shared.Person.Person person)
        {
            _dataServices.Save(person);
        }

        public void Remove(int id, PersonType typeOfPerson)
        {
            string additionalCriteria = $"PersonType = {(int)typeOfPerson}";
            _dataServices.Remove<Com.OldLeaf.Shared.Person.Person>(id, additionalCriteria);
        }
    }
}

// OldLeaf.OldKey.BLL