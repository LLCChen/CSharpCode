using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessUsage
{
    class ProcessUsage
    {
        public bool Test_Usage1_RunVladTest(string vladPath, string searchgoldRoot, string changeNumber)
        {
 
            const int VladTimeoutSeconds = 300; // 5 min

            Regex vladRegex = new Regex(@"Testing completed\.\s+(?<passed>\d+) Tests passed\.\s+(?<failed>\d+) Tests failed\.\s+(?<notRan>\d+) Tests not ran\.\s+(?<total>\d+) Total tests ran\.", RegexOptions.Compiled);

            var proc = new Process();

            proc.StartInfo.FileName = vladPath;

            if (!File.Exists(proc.StartInfo.FileName))
            {                
                return false;
            }
            proc.StartInfo.Arguments = changeNumber.ToString(); // "-errorsOnly -c " +
            proc.StartInfo.EnvironmentVariables["inetroot"] = searchgoldRoot;
            proc.StartInfo.WorkingDirectory = searchgoldRoot;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            var t0 = DateTime.Now;
            if (!proc.WaitForExit(VladTimeoutSeconds * 1000))
            {
                Console.WriteLine("Warning: timeout after waiting ({0}) for vlad.exe", DateTime.Now - t0);
                return false;
            }
            Console.WriteLine("vlad elapsed time: {0}", DateTime.Now - t0);

            var vladStdout = proc.StandardOutput.ReadToEnd();


            var m = vladRegex.Match(vladStdout);
            if (!m.Success)
            {
                Console.WriteLine("Warning: can't parse vlad output.");
                return false;
            }

            int passed = 0;
            if (!int.TryParse(m.Groups["passed"].Value, out passed))
            {
                Console.WriteLine("Warning: could not parse vlad passed tests");
            }

            int failed = 0;
            if (!int.TryParse(m.Groups["failed"].Value, out failed))
            {
                Console.WriteLine("Warning: could not parse vlad failed tests");
            }

            int notRan = 0;
            if (!int.TryParse(m.Groups["notRan"].Value, out notRan))
            {
                Console.WriteLine("Warning: could not parse vlad not-ran tests");
            }

            int total = 0;
            if (!int.TryParse(m.Groups["total"].Value, out total))
            {
                Console.WriteLine("Warning: could not parse vlad total tests");
            }

            Console.WriteLine("Vlad tests results:\tpassed:{0}\tfailed:{1}\tnot-run:{2}\ttotal{3}", passed, failed, notRan, total);

            if (failed > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Starts the process associated with the given file under the working directory of the file and returns
        /// a process object related to the process
        /// </summary>
        /// <param name="fileName">The name of the executable file to start</param>
        /// <param name="arguments">Command line arguments that are passed to the process</param>
        /// <param name="standardInput">Standard input to the process</param>
        /// <param name="environmentVariables">A dictionary of environment variables to be set on the process object.</param>
        /// <returns>A process object associated with the process just started</returns>
        /// 
        public static Process Test_Usage2_StartProcess(string fileName, string arguments,
                bool redirectOutput, string standardInput, StringDictionary environmentVariables)
        {
            //Create a start info for the process based on filename
            //and arguments
            ProcessStartInfo startInfo =
                new ProcessStartInfo(fileName, arguments);

            // add the requested environment variables
            //
            foreach (string environmentVariableName in environmentVariables.Keys)
            {
                if (startInfo.EnvironmentVariables[environmentVariableName] != null)
                    startInfo.EnvironmentVariables[environmentVariableName] = environmentVariables[environmentVariableName];
                else
                    startInfo.EnvironmentVariables.Add(environmentVariableName, environmentVariables[environmentVariableName]);
            }

            //Set the working directory for the process
            FileInfo file = new FileInfo(fileName);
            startInfo.WorkingDirectory = file.DirectoryName;
            startInfo.UseShellExecute = false;
            if (redirectOutput)
            {
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
            }
            if (standardInput != null)
            {
                startInfo.RedirectStandardInput = true;
            }

            //Create a new process based on the startinfo
            Process process = new Process();
            process.StartInfo = startInfo;

            //Start the process and return the process object
            process.Start();
            if (standardInput != null)
            {
                process.StandardInput.Write(standardInput);
            }

            return process;
        }


    }

    class FileSystemChangeWatcher
    {
        public static void Test_Usage3_DOFileSystemWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"D:\Project\UX\searchgold_ux_dev\deploy\builds\data\uxtest\excelsior\XmlBags\";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += watcher_Changed;
            watcher.EnableRaisingEvents = true;
            Console.Read();
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var info = new ProcessStartInfo("sd", @"add D:\Project\UX\searchgold_ux_dev\deploy\builds\data\uxtest\excelsior\XmlBags\...");
            info.UseShellExecute = false;
            var proc = Process.Start(info);
            proc.WaitForExit();

            info = new ProcessStartInfo(@"D:\Project\UX\searchgold_ux_dev\tools\vlad\EnforceBinaryBagsForExcelsior\bin\XmlBagConverter.exe");
            info.UseShellExecute = false;
            proc = Process.Start(info);
            proc.WaitForExit();
        }
    }
}
