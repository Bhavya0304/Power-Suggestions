using PowerSuggestion.Models;
using PowerSuggestion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.Helpers
{
    public static class Convertors
    {
        public static void ConvertAttributeModelToViewModel(AttributeMetadata data, AttributeMetadataModel outParam)
        {
            outParam.LogicalName = data.LogicalName;
            outParam.DisplayName = data.DisplayName;
            outParam.SchemaName = data.SchemaName;
            outParam.AttributeType = data.AttributeType;
            outParam.OptionSet = data.Options;

        }

        public static void ConvertEntityModelToViewModel(EntityMetadata data, EntityMetadataModel outParam)
        {
            outParam.LogicalName = data.LogicalName;
            outParam.DisplayName = data.DisplayName;
            outParam.SchemaName = data.SchemaName;
            outParam.PluralName = data.PluralName;

        }
    }
}
