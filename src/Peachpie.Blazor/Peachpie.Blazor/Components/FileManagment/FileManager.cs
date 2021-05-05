using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    public class FileManager
    {
        private BlazorContext _ctx;
        private List<FormFile> _fetched;
        private Dictionary<int, string> _downloaded;
        private ILogger<FileManager> _logger;

        public FileManager(BlazorContext ctx, ILoggerFactory factory)
        {
            _ctx = ctx;
            _fetched = new List<FormFile>();
            _downloaded = new Dictionary<int, string>();
            _logger = factory.CreateLogger<FileManager>();
        }

        public List<FormFile> FetchFiles()
        {
            if (_ctx.CallJs<bool>(JsResource.IsFiles))
            {
                var files = _ctx.CallJs<FormFile[]>(JsResource.getFiles);
                foreach (var file in files)
                {
                    _fetched.Add(file);
                }
            }

            return _fetched;
        }

        public async Task DownloadFilesAsync()
        {
            if (_fetched.Count == 0)
                return;

            foreach (var item in _fetched)
            {
                Log.DownloadFile(_logger, item);
                _downloaded.Add(item.id, await _ctx.CallJsAsync<string>(JsResource.getFileContentAsBase64, item.id));
            }

            _fetched = new List<FormFile>();
        }

        public string GetFileData(int id)
        {
            if (_downloaded.TryGetValue(id, out string result))
                return result;
            else
                return null;
        }
        
    }
}
