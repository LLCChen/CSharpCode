using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileUsage
{
    class FileSystemWatcherUsage
    {
        static void DOFileSystemWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"D:\Project\UX\searchgold_ux_dev\deploy\builds\data\uxtest\excelsior\XmlBags\OrionPreview";
            watcher.NotifyFilter = NotifyFilters.LastWrite;  // this filter only care about the add/update, not care about delete
            watcher.Filter = "*.*";
            watcher.Changed += watcher_Changed;
            watcher.EnableRaisingEvents = true;
            Console.Read();
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string xmlBagsPath = @"D:\Project\XmlBags\OrionPreview";
            string binBagsPath = @"D:\Project\BinaryBags\OrionPreview";
            List<string> filenameList = new List<string>();

            //1. do sd add
            var info = new ProcessStartInfo("sd", string.Format("{0} {1}", "add", xmlBagsPath + @"\..."));
            info.UseShellExecute = false;
            var proc = Process.Start(info);
            proc.WaitForExit();

            //2. do sd open to get the sd file list
            info = new ProcessStartInfo("sd", string.Format("{0} {1}", "opened", xmlBagsPath + @"\..."));
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            proc = Process.Start(info);
            var sdOpenedOut = proc.StandardOutput.ReadToEnd();
            string pattern = @"//depot/deploy/builds/data/uxtest/excelsior/XmlBags/OrionPreview/(.*.xml)#.*";
            Regex re = new Regex(pattern);
            MatchCollection matchCollection = re.Matches(sdOpenedOut);
            foreach (Match match in matchCollection)
            {
                if (match.Groups.Count > 1)
                {
                    filenameList.Add(match.Groups[1].ToString());
                }
            }

            File.WriteAllText(Path.Combine(binBagsPath, "temp.txt"), sdOpenedOut);
            proc.WaitForExit();

            //3. convert xml to bin
            info = new ProcessStartInfo(@"D:\Project\UX\searchgold_ux_dev\tools\vlad\EnforceBinaryBagsForExcelsior\bin\XmlBagConverter.exe");
            info.UseShellExecute = false;
            proc = Process.Start(info);
            proc.WaitForExit();

            //4. do sd revert for deletion in future
            info = new ProcessStartInfo("sd", string.Format("{0} {1}", "revert", xmlBagsPath + @"\..."));
            info.UseShellExecute = false;
            proc = Process.Start(info);
            proc.WaitForExit();

            info = new ProcessStartInfo("sd", string.Format("{0} {1}", "revert", binBagsPath + @"\..."));
            info.UseShellExecute = false;
            proc = Process.Start(info);
            proc.WaitForExit();

            //5. do file deletion             
            int i = 0;
            foreach (string filename in filenameList)
            {
                if (File.Exists(Path.Combine(xmlBagsPath, filename)))
                {
                    File.Delete(Path.Combine(xmlBagsPath, filename));
                    i++;
                }
            }
            Console.WriteLine("file delete " + filenameList.Count().ToString() + " actually " + i.ToString());
        }
    }
}
