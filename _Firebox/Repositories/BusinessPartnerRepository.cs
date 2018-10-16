using KIntegration.Models.Invariable;
using KIntegration.Models.Partners;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Firebox.Repositories
{
    public class BusinessPartnerRepository
    {
        public const string Table = "BusinessPartner";

        public void Add(List<BusinessPartnerModel> bparters)
        {
            foreach (var bpartner in bparters)
                Add(bpartner);
        }

        private bool Add(BusinessPartnerModel bpartner)
        {
            bpartner = Get(bpartner);

            using (var ms = new MSConn())
            {
                //var transaction = ms.Conn.BeginTransaction();

                try
                {
                    #region Update Address
                    if(bpartner.AddressShipId == 0)
                    {
                        if (bpartner.AddressShip.LocationId > 0)
                        {
                            var id = ms.ExecuteNonQuery(AddAddress(bpartner.AddressShip));
                            bpartner.AddressShipId = int.Parse(id);
                        }
                        
                    } else
                        ms.ExecuteNonQuery(UpdateAddress(bpartner.AddressShipId, bpartner.AddressShip));

                    if (bpartner.AddressBillId == 0)
                    {
                        if (bpartner.AddressBill.LocationId > 0)
                        {
                            var id = ms.ExecuteNonQuery(AddAddress(bpartner.AddressBill));
                            bpartner.AddressBillId = int.Parse(id);
                        }

                    }
                    else
                        ms.ExecuteNonQuery(UpdateAddress(bpartner.AddressBillId, bpartner.AddressBill));
                    #endregion

                    #region Update BPartner
                    if(bpartner.Id == 0)
                    {
                        var id = ms.ExecuteNonQuery(AddBPartner(bpartner));
                        bpartner.Id = int.Parse(id);
                    }
                    else
                        ms.ExecuteNonQuery(UpdateBPartner(bpartner.Id, bpartner));
                    #endregion

                    //transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Logs.Register(Table, $"{bpartner.InterCode}/{bpartner.CardCode}", ex);

                    return false;
                }

            }
        }

        private BusinessPartnerModel Get(BusinessPartnerModel bpartner)
        {
            using (var ms = new MSConn())
            {
                var sql = @"SELECT Id, AddressShipId, AddressBillId FROM BPART0 WHERE InterCode = {0};";
                var command = new SqlCommand(String.Format(sql, bpartner.InterCode), ms.Conn);
                var reader = command.ExecuteReader();
                var id = 0;
                if (reader.Read())
                {
                    bpartner.Id = int.Parse(reader[0].ToString());
                    bpartner.AddressShipId = int.Parse(reader[1].ToString());
                    bpartner.AddressBillId = int.Parse(reader[2].ToString());
                }

                return bpartner;
            }
        }

        public string AddAddress(AddressModel address)
        {
            address.Actived = true;
                var sql = @"
INSERT INTO [ADDRS0]
           ([FatherId],[UUID],[Code],[Name],[Actived],[Blocked],[Status]
           ,[VisOrder],[UserIdProper],[CreatedAt],[UserIdUpdate]
           ,[UpdateAt],[Type],[Address1],[Address2],[Block],[CityId],[ZipCode],[Type])
     VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',{11},'{12}','{13}','{14}','{15}','{16}','{17}',{18})";

                sql = String.Format(sql,
                    address.FatherId, address.UUID, address.Code, address.Name, address.Actived ? 1 : 0, address.Blocked ? 1 : 0, 20,
                    address.VisOrder, address.UserIdProper, "GETDATE()", address.UserIdUpdate,
                    "GETDATE()", (int)address.Type, address.Address1.Replace("'","''"), address.Address2.Replace("'", "''")
                    , address.Block.Replace("'", "''"), address.LocationId, address.ZipCode, (int)address.Type);

            return sql;
        }

        public string UpdateAddress(int id, AddressModel address)
        {
            var sql = @"
UPDATE [ADDRS0]
   SET [UpdateAt] = '{1}'
      ,[Address1] = '{2}'
      ,[Address2] = '{3}'
      ,[Block] = '{4}'
      ,[CityId] = '{5}'
      ,[ZipCode] = '{6}'
      ,[Type] = '{7}'
 WHERE Id = '{0}'
";
            return String.Format(sql, id
                , address.UpdateAt.ToString("yyyy-MM-dd hh:mm:ss")
                , address.Address1
                , address.Address2
                , address.Block
                , address.LocationId
                , address.ZipCode
                , (int)address.Type);
        }

        public string AddBPartner(BusinessPartnerModel bpartner)
        {
            var sql = @"
INSERT INTO [BPART0]
           ([FatherId],[UUID],[Code],[Name],[Actived],[Blocked]
           ,[Status],[UserIdProper],[CreatedAt],[UserIdUpdate]
           ,[UpdateAt],[InterCode],[CardCode],[CompanyName],[TradingName],[TaxId1]
           ,[TaxId2],[TaxId3],[AddressShipId],[AddressBillId],[Salesman],[VisOrder])
     VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}',
           '{6}','{7}',{8},'{9}',
           {10},'{11}','{12}','{13}','{14}','{15}',
           '{16}','{17}','{18}','{19}','{20}',{21})
";
            return String.Format(sql,
                bpartner.FatherId, bpartner.UUID, bpartner.Code, bpartner.Name, 1, 0,
                20, -1, "GETDATE()", -1,
                "GETDATE()", bpartner.InterCode, bpartner.CardCode, bpartner.CompanyName.Replace("'","''"), bpartner.TradingName.Replace("'", "''"), bpartner.TaxId1,
                bpartner.TaxId2, bpartner.TaxId3, bpartner.AddressShipId, bpartner.AddressBillId, bpartner.Salesman, 0);
        }

        public string UpdateBPartner(int id, BusinessPartnerModel bpartner)
        {
            var sql = $@"
UPDATE [BPART0]
   SET [Code] = '{bpartner.Code}'
      ,[Name] = '{bpartner.Name}'
      ,[Actived] ='{(bpartner.Actived ? 1 : 0)}'    
      ,[UpdateAt] = GETDATE()
      ,[CardCode] = '{bpartner.CardCode}'
      ,[CompanyName] ='{bpartner.CompanyName}'
      ,[TradingName] = '{bpartner.TradingName}'
      ,[TaxId1] = '{bpartner.TaxId1}'
      ,[TaxId2] = '{bpartner.TaxId2}'
      ,[TaxId3] = '{bpartner.TaxId3}'
      ,[AddressShipId] = '{bpartner.AddressShipId}'
      ,[AddressBillId] = '{bpartner.AddressBillId}'
      ,[Salesman] = '{bpartner.Salesman}'
 WHERE Id = {id}";

            return sql;
        }
    }
}
