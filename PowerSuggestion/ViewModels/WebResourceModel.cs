using PowerSuggestion.Helpers;
using PowerSuggestion.Models;
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

        private string jsFileName;
        public string JSFileName
        {
            get

            {
                return jsFileName;
            }
            set
            {
                jsFileName = value;
                OnPropertyChanged();
            }
        }

        private string jsFilepath;
        public string JSFilepath
        {
            get

            {
                return jsFilepath;
            }
            set
            {
                jsFilepath = value;
                OnPropertyChanged();
            }
        }

        private string wRName;
        public string WRName
        {
            get

            {
                return wRName;
            }
            set
            {
                wRName = value;
                OnPropertyChanged();
            }
        }

        private string wRDisplayName;
        public string WRDisplayName
        {
            get

            {
                return wRDisplayName;
            }
            set
            {
                wRDisplayName = value;
                OnPropertyChanged();
            }
        }

        private string wRDesc;
        public string WRDesc
        {
            get

            {
                return wRDesc;
            }
            set
            {
                wRDesc = value;
                OnPropertyChanged();
            }
        }

        private string wRPrefix;
        public string WRPrefix
        {
            get

            {
                return wRPrefix + "_";
            }
            set
            {
                wRPrefix = value;
                OnPropertyChanged();
            }
        }



        private List<SolutionMetadata> Solutions { get; set; }
        public List<SolutionMetadata> solutions
        {
            get

            {
                return Solutions;
            }
            set
            {
                Solutions = value;
                OnPropertyChanged();
            }
        }

        private List<WebResourceMetedata> webResource { get; set; }
        public List<WebResourceMetedata> WebResource
        {
            get
            {
                return webResource;
            }
            set
            {
                webResource = value;
                OnPropertyChanged();
            }
        }

    }
}
