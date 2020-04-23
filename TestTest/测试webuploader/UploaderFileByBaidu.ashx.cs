using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace 测试webuploader
{
    /// <summary>
    /// UploaderFileByBaidu 的摘要说明
    /// </summary>
    public class UploaderFileByBaidu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request["REQUEST_METHOD"] == "OPTIONS")
            {
                context.Response.End();
            }

            SaveFile();
        }

        /// <summary>
        /// 文件保存操作
        /// </summary>
        /// <param name="basePath"></param>
        private void SaveFile(string basePath = "~/Upload/Images/")
        {

            basePath = FileHelper.GetUploadPath();
            string Datedir = DateTime.Now.ToString("yy-MM-dd");
            string updir = basePath + "\\" + Datedir;
            string extname = string.Empty;
            string fullname = string.Empty;
            string filename = string.Empty;

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            if (files.Count == 0)
            {
                var result = "{\"jsonrpc\" : \"2.0\", \"error\" :  \"保存失败\",\"id\" :  \"id\"}";
                System.Web.HttpContext.Current.Response.Write(result);
            }
            if (!Directory.Exists(updir))
                Directory.CreateDirectory(updir);

            var suffix = files[0].ContentType.Split('/');
            var _suffix = suffix[1].Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) ? "" : suffix[1];
            var _temp = System.Web.HttpContext.Current.Request["name"];

            if (!string.IsNullOrEmpty(_temp))
            {
                filename = _temp;
            }
            else
            {
                Random rand = new Random(24 * (int)DateTime.Now.Ticks);
                filename = rand.Next() + "." + _suffix;
            }

            fullname = string.Format("{0}\\{1}", updir, filename);
            files[0].SaveAs(fullname);
            var _result = "{\"jsonrpc\" : \"2.0\", \"result\" : null, \"id\" : \"" + filename + "\"}";
            System.Web.HttpContext.Current.Response.Write(_result);

        }

        public bool IsReusable
        {

            get
            {
                return false;
            }
        }
    }
}