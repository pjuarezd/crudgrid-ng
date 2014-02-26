using System;

namespace CrudGridExample.Models.CrudGridModels
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldAttribute : Attribute
    {
        private bool _editable = true;
        private bool _columnVisible = true;
        private bool _fieldVisible = true;

        public FieldAttribute()
        {
            Order = 0;
        }

        public string Caption { get; set; }

        public bool Editable
        {
            get { return _editable; }
            set { _editable = value; }
        }

        public bool ColumnVisible
        {
            get { return _columnVisible; }
            set { _columnVisible = value; }
        }

        public bool FieldVisible
        {
            get { return _fieldVisible; }
            set { _fieldVisible = value; }
        }

        public string Datatype { get; set; }

        public int Order { get; set; }
    }
}