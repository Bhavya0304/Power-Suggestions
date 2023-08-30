﻿using PowerSuggestion.Models;
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
            outParam.OptionSet = data.Options;

        }
    }
}
