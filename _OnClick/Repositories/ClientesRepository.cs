using System;
using System.Collections.Generic;
using System.Linq;
using FirebirdSql.Data.FirebirdClient;
using KIntegration;
using KIntegration.Firebox.Repositories;
using KIntegration.Models.Partners;

namespace KIntegration.OnClick.Repositories
{
    public class ClientesRepository
    {
        public List<BusinessPartnerModel> GetFrom(DateTime from, int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<BusinessPartnerModel>();

                var sql = @"SELECT MAX(INTERCODE) FROM VKS1_CLIENTES;";
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

        private List<BusinessPartnerModel> GetUpdate(DateTime last, int? limite)
        {
            using (var fb = new FBConn())
            {
                var list = new List<BusinessPartnerModel>();

                var sql = $"SELECT * FROM VKS1_CLIENTES WHERE UPDATEAT > '{last.ToString("yyyy-MM-dd hh:mm:ss")}' AND INTERCODE <= {limite};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return (Load(ref reader));
            }
        }

        private List<BusinessPartnerModel> GetNew(int last)
        {
            using (var fb = new FBConn())
            {
                var list = new List<BusinessPartnerModel>();

                var sql = $"SELECT * FROM VKS1_CLIENTES WHERE INTERCODE > {last};";
                var command = new FbCommand(sql, fb.Conn);
                var reader = command.ExecuteReader();

                return Load(ref reader);
            }
        }

        private List<BusinessPartnerModel> Load(ref FbDataReader reader)
        {
            var list = new List<BusinessPartnerModel>();

            while (reader.Read())
            {
                var bp = new BusinessPartnerModel(Enums.BPartnerType.Customer,ref reader);
                list.Add(bp);
            }

            reader.Close();

            return list;
        }
    }
}
