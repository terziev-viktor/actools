extern alias Original;
using System.Net;
using System.Threading.Tasks;
using Orig = Original::AcManager.Internal;

namespace AcManager.Internal {
    public static class WebClientExtension {
        public static Task<byte[]> DownloadDataTaskAsyncSafe(this WebClient c, string url) {
            return Orig.WebClientExtension.DownloadDataTaskAsyncSafe(c, url);
        }
    }
}
