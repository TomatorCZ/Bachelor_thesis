using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    [PhpHidden]
    public static class JsResource
    {
        public static readonly string IsPost = "window.php.isPost";
        public static readonly string IsFiles = "window.php.isFiles";
        public static readonly string getPost = "window.php.forms.getPostData";
        public static readonly string getFiles = "window.php.forms.getFilesData";
        public static readonly string getFileContentAsBase64 = "window.php.files.readAllFileAsBase64";
        public static readonly string createUrlObject = "window.php.files.createUrlObject";
    }
}
