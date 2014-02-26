using System.Collections.Generic;

namespace CrudGridExample.Models.CrudGridModels
{
    public class MetadataItem
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public long RowCount { get; set; }
        public List<Field> Fields { get; set; }
    }
}