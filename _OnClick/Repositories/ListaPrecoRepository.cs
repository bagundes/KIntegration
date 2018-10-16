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
    public class ListaPrecoRepository
    {
        public List<PriceListModel> GetFrom(DateTime from, int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<PriceListModel>();

                var sql = @"SELECT MAX(INTERNCODE) FROM VKS1_LISTAPRECO;";
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

        private List<PriceListModel> GetUpdate(DateTime last, int? limite)
        {
            using (var fb = new FBConn())
            {
                var list = new List<ItemModel>();

                var sql = $"SELECT * FROM VKS1_LISTAPRECO WHERE UPDATEAT > '{last.ToString("yyyy-MM-dd hh:mm:ss")}' AND INTERNCODE <= {limite};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return (Load(ref reader));
            }
        }

        private List<PriceListModel> GetNew(int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<PriceListModel>();

                var sql = $"SELECT * FROM VKS1_LISTAPRECO WHERE INTERNCODE > {last};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return Load(ref reader);
            }
        }

        private List<PriceListModel> Load(ref FbDataReader reader)
        {
            var list = new List<PriceListModel>();

            while (reader.Read())
            {
                var item = new PriceListModel(ref reader);
                list.Add(item);
            }

            reader.Close();

            return list;
        }
    }
}
