using System;
using System.Collections.Generic;

namespace Com.OldLeaf.Shared.Person
{
    public interface IPersonDirProxy
    {
        Person Get(int id);
        List<Person> Search(string criteria);
        void Save(Person person);
        void Remove(int id, PersonType typeOfPerson);
    }
}