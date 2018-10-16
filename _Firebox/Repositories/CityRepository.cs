using KIntegration.Models.Invariable;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Firebox.Repositories
{
    public class CityRepository
    {
        public int GetId(string uf, string city)
        {
            city = RemAcentos(city.ToUpper().Replace(" ", "_"));
            uf = uf.ToUpper();

            using (var ms = new MSConn())
            {
                var id = 0;
                var sql = $"SELECT Id FROM CITIS0 WHERE StateShort = '{uf}' AND CityShort = '{city}'";
                var command = new SqlCommand(sql, ms.Conn);
                var reader = command.ExecuteReader();

                if(reader.Read())
                    id = int.Parse(reader[0].ToString());

                reader.Close();

                return id;
            }
    
        }

        private string RemAcentos(string val)
        {
            val = val.Replace("´", "").Replace("`", "").Replace("'", "");
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                val = val.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return val;
        }

        public static int GetModel(string uf, string city)
        {
            var model = new CityRepository();
            return model.GetId(uf, city); 
        }
    }
}
