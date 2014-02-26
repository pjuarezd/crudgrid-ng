using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CrudGridExample.Data;
using CrudGridExample.Models;
using CrudGridExample.Models.CrudGridModels;

namespace CrudGridExample.Controllers
{

    class PersonFullName : Person
    {
        public string FullName { get; set; }
    }

    public class CustomController : PersonController
    {
        [HttpGet]
        public MetadataItem GetMetadata([FromUri] FilterModel filter)
        {
            try
            {
                var metadata = new MetadataItem();

                var fields = new List<Field>()
                            {
                                
                                new Field()
                                    {
                                        Name = "FirstName",
                                        Caption = "First Name",
                                        Datatype = typeof (string).Name,
                                        Order = 1
                                    },
                                new Field()
                                    {
                                        Name = "LastName",
                                        Caption = "Last Name",
                                        Datatype = typeof (string).Name,
                                        Order = 2
                                    },
                                    new Field()
                                {
                                    Name = "FullName",
                                    Caption = "Full Name",
                                    Datatype = typeof (string).Name,
                                    Editable = false,
                                    Order = 0
                                }
                            };

                metadata = new MetadataItem
                {
                    Name = ControllerName,
                    Key = "FirstName",
                    Fields = fields,
                    RowCount = DataSources.Persons.Count
                };
                return metadata;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public IEnumerable GetWithFullname([FromUri] FilterModel filter)
        {
            var customObjectCollection =
                DataSources.Persons.AsQueryable()
                           .Skip(filter.SkipCount)
                           .Take(filter.Size)
                           .Select(x =>
                                   new PersonFullName
                                   {
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       FullName = x.FirstName + " " + x.LastName
                                   });

            if (!string.IsNullOrEmpty(filter.SortName))
            {
                customObjectCollection = filter.SortOrder == FilterModel.SortingOrder.Desc ?
                    customObjectCollection.OrderByDescending(filter.GetSortingExpression<PersonFullName, string>()) :
                    customObjectCollection.OrderBy(filter.GetSortingExpression<PersonFullName, string>());
            }
            return customObjectCollection.ToList();
        }
    }
}
