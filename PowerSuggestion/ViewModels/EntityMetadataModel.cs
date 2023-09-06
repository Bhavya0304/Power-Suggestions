using PowerSuggestion.Helpers;
using PowerSuggestion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.ViewModels
{
    public class EntityMetadataModel : ViewModel
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


    }
}
