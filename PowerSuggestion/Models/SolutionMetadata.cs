using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PowerSuggestion.Helpers.Enums;

namespace PowerSuggestion.Models
{
    public class SolutionMetadata
    {
        public Guid Id { get; set; }
        public string UniqueName { get; set; }
        public string Prefix { get; set; }
        public List<SolutionComponents> Components { get; set; }

    }

    public class SolutionComponents
    {
        public Guid Id { get; set; }
        public string ComponentName { get; set; }
        public Guid ObjectId { get; set; }
        public SolutionComponentType ComponentType { get; set; }
    }
}
