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

    public static class FileUtils
    {
        public static void GetBrowserFileContent(Context ctx, int id, IPhpCallable callback)
        {
            var Task = GenericHelper.CallJsAsync<string>(ctx, JsResource.getFileContentAsBase64, id);

            Task.AsTask().ContinueWith((result) => callback.Invoke(ctx, new PhpString(result.Result), PhpValue.Create(id)));
        }

        public static string CreateUrlObject(Context ctx, int id)
        {
            return GenericHelper.CallJs<string>(ctx, JsResource.createUrlObject, id);
        }
    }
}
