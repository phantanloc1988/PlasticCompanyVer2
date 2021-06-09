using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticCompany.Common
{
    public static class Tools
    {
        public static string GetRandom(int length = 5)
        {
            var pattern = @"1234567890qazwsxedcrfvtgbyhn@#$%";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(pattern[rd.Next(0, pattern.Length)]);

            return sb.ToString();
        }

        public static void WriteLog(string ActionName)
        {
            try
            {
                var PathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileLog", "Checking.txt");


                using (StreamWriter writer = new StreamWriter(PathFile, true))
                {
                    string DataToWrite = $"{DateTime.UtcNow} - {ActionName}";

                    writer.WriteLine(DataToWrite);
                }


            }
            catch (Exception)
            {

                throw new Exception("Loi ghi File Checking.txt");
            }
        }

        public static string Encoding(string code)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(code);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Decoding(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
