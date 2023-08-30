using PowerSuggestion.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.ViewModels
{
    public class WebResourceModel : ViewModel
    {
        private string jsFile;
        public string JSFile
        {
            get

            {
                return "Loaded Js: " + jsFile;
            }
            set
            {
                jsFile = value;
                OnPropertyChanged();
            }
        }



    }
}
