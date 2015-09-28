using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SqlServer.Server;
using Yomego.Umbraco.Utils;

namespace Website.Domain.Shared.Services
{
    public class FileStatusService : WebsiteService
    {
        private string _fileName { get; }
        private string _filePath { get; }

        private string FullPath => _filePath + _fileName;

        public FileStatusService(string fileName, string filePath = null)
        {
            _fileName = fileName.NotNull();
            _filePath = HttpContext.Current.Server.MapPath("~" + (filePath ?? "/App_Data/"));
        }

        public void Update(string key)
        {
            using (var newTask = new StreamWriter(FullPath, false))
            {
                newTask.WriteLine(key);
            }
        }

        public string CurrentValue()
        {
            using (var reader = new StreamReader(FullPath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    return line;
                }

                return null;
            }
        }
    }
}
