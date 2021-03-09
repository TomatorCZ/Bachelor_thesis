using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class FileManager
    {
        private BlazorContext _ctx;
        private List<FormFile> _fetched;
        private Dictionary<int, string> _downloaded;

        public FileManager(BlazorContext ctx)
        {
            _ctx = ctx;
            _fetched = new List<FormFile>();
            _downloaded = new Dictionary<int, string>();
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
                _downloaded.Add(item.id, await _ctx.CallJsAsync<string>(JsResource.getFileContentAsBase64, item.id));
            }

            _fetched.Clear();
        }

        public string GetFileData(int id) => _downloaded[id];
    }
}
