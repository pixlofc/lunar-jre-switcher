using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lunar_jre_switcher
{
    class Program
    {
        static WebClient wc = new WebClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Lunar jre switcher by Pixel#8194\n");
            Console.WriteLine("WARNING: The only safe place to download this is on my github (github.com/pixlofc/lunar-jre-switcher/)" +
                "if you downloaded from another place, close this and delete it NOW!");

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string lunarpath = home + "\\.lunarclient\\jre";

            if (Directory.Exists(lunarpath))
            {
                Console.WriteLine("\nFound base jre folder (" + lunarpath + "\\" + ")");
            }
            else
            {
                Console.WriteLine("\nFailed to find jre folder, close this program");
                while (true) { }
            }

            bool is64bit = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
            if (is64bit)
            {
                Console.WriteLine("Detected system architecture (x64)");
                
                Console.WriteLine("Downloading jre-x64");
                string tempjre = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";

                wc.DownloadFile("https://cdn-101.anonfiles.com/v1teX8C0p9/2cdbcd23-1611686934/jre-x64.zip", tempjre + "jre-x64.zip");

                Console.WriteLine("Extracting jre-x64.zip");

                ZipFile.ExtractToDirectory(tempjre + "jre-x64.zip", tempjre + "jre-x64");

                Console.WriteLine("Switching jre");

                string[] jres = Directory.GetDirectories(lunarpath);
                foreach(string jresdirs in jres)
                {
                    Console.WriteLine(jresdirs);
                    if (Directory.Exists(jresdirs + "\\bin\\") || Directory.Exists(jresdirs + "\\lib\\"))
                    {
                        string bin = jresdirs.ToString() + "\\bin\\";
                        string lib = jresdirs.ToString() + "\\lib\\";
                        Directory.Delete(bin, true);
                        Directory.Delete(lib, true);
                        Console.WriteLine("Switching " + jresdirs);
                        Directory.CreateDirectory(jresdirs + "\\bin\\");
                        Directory.CreateDirectory(jresdirs + "\\lib\\");
                        string SourcePath1 = tempjre + "jre-x64\\bin\\";
                        string DestinationPath1 = jresdirs + "\\bin\\";
                        string SourcePath2 = tempjre + "jre-x64\\lib\\";
                        string DestinationPath2 = jresdirs + "\\lib\\";

                        foreach (string dirPath in Directory.GetDirectories(SourcePath1, "*",
                            SearchOption.AllDirectories))
                            Directory.CreateDirectory(dirPath.Replace(SourcePath1, DestinationPath1));

                        foreach (string newPath in Directory.GetFiles(SourcePath1, "*.*",
                            SearchOption.AllDirectories))
                            File.Copy(newPath, newPath.Replace(SourcePath1, DestinationPath1), true);


                        foreach (string dirPath in Directory.GetDirectories(SourcePath2, "*",
                            SearchOption.AllDirectories))
                            Directory.CreateDirectory(dirPath.Replace(SourcePath2, DestinationPath2));

                        foreach (string newPath in Directory.GetFiles(SourcePath2, "*.*",
                            SearchOption.AllDirectories))
                            File.Copy(newPath, newPath.Replace(SourcePath2, DestinationPath2), true);

                        Console.WriteLine("Switched " + jresdirs);
                    } else
                    {
                        Console.WriteLine("Failed to switch this jre! If all failed try launching lunar client then switching");
                    }
                }
                Console.WriteLine("Deleting temp files");
                File.Delete(tempjre + "jre-x64.zip");
                Directory.Delete(tempjre + "jre-x64");
                Console.WriteLine("Done, you can close this now");
                while (true) { }
            }
            else
            {
                Console.WriteLine("Detected system architecture (x86)");
                Console.WriteLine("32bit isnt supported yet, close this application");
                while (true) { }
            }
        }
    }
}
