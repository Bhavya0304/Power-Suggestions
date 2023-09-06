using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.Models
{
    public class EntityMetadata
    {
        public string LogicalName { get; set; }
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }
        public string PluralName { get; set; }
        public List<EntityAttributes> Attributes { get; set; }
        public Guid Id { get; set; }

    }
    public class EntityAttributes
    {
        public string AttributeDisplayName { get; set; }
        public string AttributeSchemaName { get; set; }
        public string AttributeLogicalName { get; set; }
        public string AttributeType { get; set; }
    }
}
