using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Files
    {
        public static string List()
        {
            string[] files = Directory.GetFiles(Config.SERVER_FILES_DIR).Select(Path.GetFileName).ToArray();
            return string.Join("; ", files) + "\n";
        }

        public static string Get(string fileName)
        {
            string filePath = string.Format("{0}\\{1}", Config.SERVER_FILES_DIR, fileName);

            if (!File.Exists(filePath))
                return "404\n";

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string fileBase64 = Convert.ToBase64String(fileBytes);

            return fileBase64 + "\n";
        }

        public static string Put(string fileName, string base64File)
        {
            string fileServerPath = string.Format("{0}\\{1}", Config.SERVER_FILES_DIR, fileName);
            File.WriteAllBytes(fileServerPath, Convert.FromBase64String(base64File));

            return "File putted successfully\n";
        }
    }
}
