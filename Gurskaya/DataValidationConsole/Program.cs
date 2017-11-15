using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataValidationConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());

                }
                else
                {
                    Console.WriteLine("Hello console");
                    if (args.Length != 3) throw new Exception("Wrong ammount of arguments\n");
                    String argumentHash = args[0];
                    String format = args[1];
                    format = format.ToLower();
                    if (format.CompareTo("-md5") != 0 && format.CompareTo("-crc2") != 0) throw new Exception("Whong format\n");
                    String fileName = args[2];
                    String hash = "";
                    //Opening file
                    FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    if (format.CompareTo("-crc2") == 0)
                    {
                        string pattern = "[0-9A-Fa-f]{8}";
                        if (!Regex.IsMatch(argumentHash, pattern, RegexOptions.IgnoreCase)) throw new Exception("Wrong CRC32 format\n");
                        Crc32 crc = new Crc32();
                        byte[] h = crc.ComputeHash(file);
                        hash = BitConverter.ToString(h);
                        hash = hash.Replace("-", "");
                    }
                    if (format.CompareTo("-md5") == 0)
                    {
                        string pattern = "[0-9A-Fa-f]{32}";
                        if (!Regex.IsMatch(argumentHash, pattern, RegexOptions.IgnoreCase)) throw new Exception("Wrong MD5 format\n");
                        StreamReader reader = new StreamReader(file);
                        //Reading file
                        String contents = reader.ReadToEnd();
                        reader.Close();
                        MD5 md = new MD5();
                        md.Value = contents;
                        hash = md.FingerPrint;
                    }
                    file.Close();
                    //Comparing
                    if (argumentHash.CompareTo(hash) == 0)
                        Console.WriteLine("Success\n");
                    else Console.WriteLine("Hashes don't match\n");
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
    }
    }
}
