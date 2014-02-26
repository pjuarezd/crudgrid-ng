using System;
using System.Linq.Expressions;

namespace CrudGridExample.Models.CrudGridModels
{
    public class FilterModel
    {
        public enum SortingOrder
        {
            Asc = 1,
            Desc=2
        }

        private string _search = "";

        public int Page { get; set; }
        public int Size { get; set; }
        public string SortName { get; set; }
        public SortingOrder SortOrder { get; set; }
        
        public Expression<Func<TObject, TField>> GetSortingExpression<TObject, TField>()
        {
            return (x => (TField) x.GetType().GetProperty(this.SortName).GetValue(x));
        }

        public int SkipCount
        {
            get { return Page > 1 ? ((Page - 1)*Size) : 0; }
        }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
    }
}