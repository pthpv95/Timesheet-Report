using System.IO;
using System.Web;

namespace TimesheetReport.WebUI.Helper
{
    public static class HttpPostedFileBaseExtensions
    {
        public static byte[] ReadBytes(this HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}