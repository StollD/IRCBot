using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotBackup
{
    // Class to make Backups of the .json Files
    public class BotBackup
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                // Copy the files
                string path = Directory.GetCurrentDirectory() + "/Backups/" + DateTime.UtcNow.ToString("yyyy-MM-dd");
                Directory.CreateDirectory(path);
                foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json"))
                    File.Copy(file, path + "/" + Path.GetFileName(file));
                Thread.Sleep(86400000); // Sleep for one day
            }
        }
    }
}
