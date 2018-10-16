using System;
using System.Collections.Generic;
using System.Linq;
using FirebirdSql.Data.FirebirdClient;
using KIntegration;
using KIntegration.Firebox.Repositories;
using KIntegration.Models.Invariable;
using KIntegration.Models.Partners;

namespace KIntegration.OnClick.Repositories
{
    public class ProdutosRepository
    {
        public List<ItemModel> GetFrom(DateTime from, int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<ItemModel>();

                var sql = @"SELECT MAX(INTERNCODE) FROM VKS1_PRODUTOS;";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();
                var max = 0;
                if (reader.Read())
                    max = int.Parse(reader[0].ToString());

                reader.Close();
                if (last < max)
                    list.AddRange(GetNew(last));

                list.AddRange(GetUpdate(from, last));

                return list;                
            }
        }

        private List<ItemModel> GetUpdate(DateTime last, int? limite)
        {
            using (var fb = new FBConn())
            {
                var list = new List<ItemModel>();

                var sql = $"SELECT * FROM VKS1_PRODUTOS WHERE UPDATEAT > '{last.ToString("yyyy-MM-dd hh:mm:ss")}' AND INTERNCODE <= {limite};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return (Load(ref reader));
            }
        }

        private List<ItemModel> GetNew(int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<ItemModel>();

                var sql = $"SELECT * FROM VKS1_PRODUTOS WHERE INTERNCODE > {last};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return Load(ref reader);
            }
        }

        private List<ItemModel> Load(ref FbDataReader reader)
        {
            var list = new List<ItemModel>();

            while (reader.Read())
            {
                var item = new ItemModel(ref reader);
                list.Add(item);
            }

            reader.Close();

            return list;
        }
    }
}
