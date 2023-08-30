using PowerSuggestion.Helpers;
using PowerSuggestion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.ViewModels
{
    public class AttributeMetadataModel : ViewModel
    {
        private string logicalName;
        public string LogicalName
        {
            get

            {
                return logicalName;
            }
            set
            {
                logicalName = value;
                OnPropertyChanged();
            }
        }

        private string displayName;
        public string DisplayName
        {
            get

            {
                return displayName;
            }
            set
            {
                displayName = value;
                OnPropertyChanged();
            }
        }

        private string schemaName;
        public string SchemaName
        {
            get

            {
                return schemaName;
            }
            set
            {
                schemaName = value;
                OnPropertyChanged();
            }
        }

        private string pluralName;
        public string PluralName
        {
            get

            {
                return pluralName;
            }
            set
            {
                pluralName = value;
                OnPropertyChanged();
            }
        }

        private string attributeType;
        public string AttributeType
        {
            get

            {
                return attributeType;
            }
            set
            {
                attributeType = value;
                OnPropertyChanged();
            }
        }

        private List<OptionSet> optionSet;
        public List<OptionSet> OptionSet
        {
            get

            {
                return optionSet;
            }
            set
            {
                optionSet = value;
                OnPropertyChanged();
            }
        }
    }
}
