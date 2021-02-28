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

        public int foo = 3;
    }

    public static class FileUtils
    {
        public static PhpArray GetFilesInfo(Context ctx, string elementId)
        {
            return GenericHelper.CallJsArray<BrowserFile>(ctx, JsResource.GetFilesInfo, elementId);
        }

        public static string CreateUrl(Context ctx, string base64Data, string contentType)
        {
            return GenericHelper.CallJs<string>(ctx, JsResource.CreateUrlObj, base64Data, contentType);
        }

        public static void GetData(Context ctx, int fileId, string functionCallback)
        {
            InteropUtils.CallJsCustomVoid(ctx, JsResource.GetData, fileId, functionCallback);
        }

        public static void DownloadData(Context ctx, string base64Data, string contentType, string filename)
        {
            InteropUtils.CallJsVoid(ctx, JsResource.DownloadData, base64Data, contentType, filename);
        }
    }
}
