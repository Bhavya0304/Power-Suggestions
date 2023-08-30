using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.Helpers
{
    public static class Enums
    {
        public enum CurrentPage
        {
            AllEntities,
            SingleEntity,
            AllAttributes,
            SingleAttributes
        }

        public enum WEBRESOURCETYPE
        {
            HTML = 1,
            CSS = 2,
            JS = 3,
            XML = 4,
            PNG = 5,
            JPG = 6,
            GIF = 7,
            XAP = 8,
            XSL = 9,
            ISO = 10
        }
    }
}
