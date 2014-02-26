using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrudGridExample.Models;

namespace CrudGridExample.Data
{
    public static class DataSources
    {
        public static List<Person> Persons = new List<Person>
            {
                new Person
                    {
                        FirstName = "Lara",
                        LastName = "Croft"
                    },
                new Person
                    {
                        FirstName = "John",
                        LastName = "Smith"
                    }
            };
    }
}