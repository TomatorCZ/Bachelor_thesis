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
        public static PhpString GetBrowserFileContent(Context ctx, int id)
        {
            return new PhpString ((ctx as BlazorContext).GetDownloadFile(id));
        }

        public static string CreateUrlObject(Context ctx, int id)
        {
            return GenericHelper.CallJs<string>(ctx, JsResource.createUrlObject, id);
        }

        public static BrowserFile CreateFile(Context ctx, string data, string name, string contentType)
        {
            return GenericHelper.CallJs<BrowserFile>(ctx, JsResource.createFile, data, name, contentType);
        }

        public static void DownLoadFile(Context ctx, int id)
        {
            InteropUtils.CallJsVoid(ctx, JsResource.downloadFile, id);
        }
    }
}
