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

        static string Mediafiredownload(string download)
        {
            HttpWebRequest req;
            HttpWebResponse res;
            string str = "";
            req = (HttpWebRequest)WebRequest.Create(download);
            res = (HttpWebResponse)req.GetResponse();
            str = new StreamReader(res.GetResponseStream()).ReadToEnd();
            int indexurl = str.IndexOf("http://download1507");
            int indexend = GetNextIndexOf('"', str, indexurl);
            string direct = str.Substring(indexurl, indexend - indexurl);
            return direct;
        }

        static int GetNextIndexOf(char c, string source, int start)
        {
            if (start < 0 || start > source.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = start; i < source.Length; i++)
            {
                if (source[i] == c)
                {
                    return i;
                }
            }
            return -1;
        }
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

                //string directlink = Mediafiredownload("https://www.mediafire.com/file/xym6f6uwgbbuap8/jre-x64.zip/file");

                if (File.Exists(tempjre + "jre-x64.zip"))
                {
                    File.Delete(tempjre + "jre-x64.zip");
                }

                wc.DownloadFile("https://download1507.mediafire.com/usqmz2xwktkg/xym6f6uwgbbuap8/jre-x64.zip", tempjre + "jre-x64.zip");

                Console.WriteLine("Extracting jre-x64.zip");

                if (Directory.Exists(tempjre + "jre-x64"))
                {
                    Directory.Delete(tempjre + "jre-x64", true);
                }

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
                Directory.Delete(tempjre + "jre-x64", true);
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
