using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration
{
    public class Logs
    {
        public static void Register(string table, string key, Exception ex)
        {
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter("./log.txt", true))
                {
                    var date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    file.WriteLine($"{date} - {table}/{key} : {ex.Message}");
                }
            }
            finally { }
        }
    }
}
