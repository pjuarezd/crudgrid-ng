using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using CrudGridExample.Data;
using CrudGridExample.Models.CrudGridModels;

namespace CrudGridExample.Controllers
{
    public class MetadataController : ApiController
    {
        private static readonly string ModelsNamespace = "CrudGridExample.Models";
        private static readonly Regex CamelCaseRegEx = new Regex(@"[a-z][A-Z]", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex UnderscoreRegex = new Regex(@"_", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        [HttpGet]
        public MetadataItem GetMetadataItem(string id)
        {
            var type = Type.GetType(ModelsNamespace + "." + id);

            if (type == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                var properties = type.GetProperties();
                var dynamicFields = new List<Field>();
                foreach (var prop in properties)
                {
                    Field field;
                    var customAttribute = (FieldAttribute)prop.GetCustomAttribute(typeof(FieldAttribute));
                    if (customAttribute != null)
                    {
                        field = new Field
                        {
                            Name = prop.Name,
                            Caption = !string.IsNullOrEmpty(customAttribute.Caption) ?
                                customAttribute.Caption :
                                Humanize(prop.Name),
                            Editable = customAttribute.Editable,
                            Datatype = !string.IsNullOrEmpty(customAttribute.Datatype) ?
                                customAttribute.Datatype : prop.PropertyType.Name,
                            Order = customAttribute.Order,
                            ColumnVisible = customAttribute.ColumnVisible,
                            FieldVisible = customAttribute.FieldVisible
                        };
                    }
                    else
                    {
                        field = new Field
                        {
                            Name = prop.Name,
                            Datatype = prop.PropertyType.Name,
                            Caption = Humanize(prop.Name)
                        };
                    }
                    dynamicFields.Add(field);
                }

                //dynamicFields = dynamicFields.OrderBy(x => x.Order).ToList();

                var item = new MetadataItem
                    {
                        Name = id,
                        Key = dynamicFields.First().Name,
                        Fields = dynamicFields,
                        RowCount = GetTypeRowCount(type)
                    };

                return item;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        private static int GetTypeRowCount(Type objectType)
        {
            IList data = null;
            var datasourcePropertyType = typeof(DataSources)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(
                        x =>
                        x.FieldType.IsGenericType &&
                        x.FieldType.Name.StartsWith("List") &&
                        x.FieldType.GetGenericArguments().Any(y => y == objectType));
            if (datasourcePropertyType != null)
            {
                data = (IList)datasourcePropertyType.GetValue(null);
            }
            return data != null ? data.Count : 0;
        }

        private static string SplitCamel(Match m)
        {
            var x = m.ToString();
            return x[0] + " " + x.Substring(1, x.Length - 1);
        }

        public static string Humanize(string camelCaseString)
        {
            var returnValue = camelCaseString ?? string.Empty;
            returnValue = UnderscoreRegex.Replace(returnValue, " ");
            return CamelCaseRegEx.Replace(returnValue, SplitCamel);
        }
    }
}
