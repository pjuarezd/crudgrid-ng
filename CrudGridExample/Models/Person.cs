using CrudGridExample.Models.CrudGridModels;

namespace CrudGridExample.Models
{
    public class Person
    {
        [Field(Caption = "First", Editable = true)]
        public string FirstName { get; set; }

        [Field(Caption = "Last", Editable = true)]
        public string LastName { get; set; }
    }
}