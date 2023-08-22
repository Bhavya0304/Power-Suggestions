using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.Models
{
    public class AttributeMetadata
    {
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }
        public string LogicalName { get; set; }
        public string AttributeType { get; set; }
        public Guid Id { get; set; }
        public List<OptionSet> Options { get; set; }
    }

    public class OptionSet
    {
        public string OptionSetName { get; set; }
        public string OptionSetValue { get; set; }
    }
}
