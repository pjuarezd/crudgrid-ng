using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CrudGridExample.Data;
using CrudGridExample.Models;
using System.Linq;
using CrudGridExample.Models.CrudGridModels;

namespace CrudGridExample.Controllers
{
    public class PersonController : ApiController
    {
        protected string ControllerName
        {
            get { return GetType().Name.Replace("Controller", ""); }
        }
        
        // GET api/Person
        [HttpGet]
        public IEnumerable Get([FromUri] FilterModel filter)
        {
            var persons = DataSources.Persons.AsQueryable();
            //Expression<Func<Person, object>> orderExpression = (x => x.GetType().GetProperty(filter.SortName).GetValue(x));

            persons = persons
                .Skip(filter.SkipCount)
                .Take(filter.Size);

            if (!string.IsNullOrEmpty(filter.SortName))
            {
                persons = filter.SortOrder == FilterModel.SortingOrder.Desc ?
                    persons.OrderByDescending(filter.GetSortingExpression<Person,string>()) :
                    persons.OrderBy(filter.GetSortingExpression<Person, string>());
            }
            return persons.ToList();
        }

        // GET api/Person/John
        [HttpGet]
        public Person GetPerson(string name)
        {
            var person = DataSources.Persons.FirstOrDefault(x => x.FirstName.Equals(name));
            if (person != null)
            {
                return person;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // POST api/Person
        [HttpPost]
        public HttpResponseMessage PostPerson([FromBody]Person entity)
        {
            try
            {
                if (entity != null && !String.IsNullOrEmpty(entity.FirstName))
                {
                    DataSources.Persons.Add(entity);
                    var response = Request.CreateResponse(HttpStatusCode.Created, entity);
                    response.Headers.Location =
                        new Uri(Url.Link("DefaultApi", new { controller = ControllerName, id = entity.FirstName }));
                    return response;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpResponseException ex2)
            {
                return Request.CreateResponse(ex2.Response.StatusCode);

            }
            catch (Exception)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        // PUT api/Person/John
        [HttpPut]
        public HttpResponseMessage PutPerson([FromBody]Person entity)
        {
            try
            {
                if (entity != null && !String.IsNullOrEmpty(entity.FirstName))
                {
                    var person = DataSources.Persons.FirstOrDefault(x => x.FirstName.Equals(entity.FirstName));
                    if (person == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    DataSources.Persons = DataSources.Persons.Where(x => x.FirstName != entity.FirstName).ToList();
                    DataSources.Persons.Add(entity);

                    var response = Request.CreateResponse(HttpStatusCode.Created, entity);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = ControllerName, id = entity.FirstName }));
                    return response;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpResponseException ex2)
            {
                return Request.CreateResponse(ex2.Response.StatusCode);

            }
            catch (Exception)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        // DELETE api/Person/5
        [HttpDelete]
        public void DeletePerson(string id)
        {
            try
            {
                var person = DataSources.Persons.FirstOrDefault(x => x.FirstName.Equals(id));
                if (person != null)
                {
                    DataSources.Persons.Remove(person);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
            catch (HttpResponseException)
            {
                throw;

            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}