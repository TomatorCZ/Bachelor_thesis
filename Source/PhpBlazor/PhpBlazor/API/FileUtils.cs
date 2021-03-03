using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    [PhpType]
    public class BrowserFile
    {
        public int id { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
    }

    [PhpType]
    public class FormFile : BrowserFile
    {
        public string fieldName {get; set;}
    }
}
