using System.Net.Http;
using System.Net.Http.Headers;

namespace MyWebApplication
{
    public class UploadFileMultiparProvider : MultipartFormDataStreamProvider
    {
        public UploadFileMultiparProvider(string rootPath) : base(rootPath) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            if (headers != null && headers.ContentDisposition != null)
            {
                return headers.ContentDisposition.FileName.TrimEnd('"').TrimStart('"');
            }

            return base.GetLocalFileName(headers);
        }
    }
}
