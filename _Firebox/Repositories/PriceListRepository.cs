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
    public class PriceListRepository
    {
        public const string Table = "PriceList";

        public void Add(List<PriceListModel> plists)
        {
            foreach (var plist in plists)
                Add(plist);
        }

        private bool Add(PriceListModel item)
        {
            item = Get(item);

            using (var ms = new MSConn())
            {
                //var transaction = ms.Conn.BeginTransaction();

                try
                {
                    #region Items
                    if(item.Id == 0)
                    {
                        var id = ms.ExecuteNonQuery(AddPList(item));
                        item.Id = int.Parse(id);
                    }
                    else
                        ms.ExecuteNonQuery(UpdatePList(item.Id, item));
                    #endregion

                    //transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Logs.Register(Table, $"{item.InternItemCode}/{item.Name}", ex);

                    return false;
                }

            }
        }

        private PriceListModel Get(PriceListModel plist)
        {
            using (var ms = new MSConn())
            {
                var sql = @"SELECT Id FROM PRLIST0 WHERE InternItemCode = '{0}' AND Code = '{1}';";
                var command = new SqlCommand(String.Format(sql, plist.InternItemCode, plist.Code), ms.Conn);
                var reader = command.ExecuteReader();
                var id = 0;
                if (reader.Read())
                {
                    plist.Id = int.Parse(reader[0].ToString());
                }

                return plist;
            }
        }

        public string AddPList(PriceListModel plist)
        {
            var sql = $@"
INSERT INTO [PRLIST0]
           ([FatherId]
           ,[UUID]
           ,[Code]
           ,[Name]
           ,[Actived]
           ,[Blocked]
           ,[Status]
           ,[VisOrder]
           ,[UserIdProper]
           ,[CreatedAt]
           ,[UserIdUpdate]
           ,[UpdateAt]
           ,[PriceList]
           ,[ListName]
           ,[InternItemCode]
           ,[Barcode]
           ,[Price])
     VALUES
           (0
           ,'{plist.UUID}'
           ,{plist.Code}
           ,'{plist.Name}'
           ,1
           ,0
           ,10
           ,0
           ,-1
           ,GETDATE()
           ,-1
           ,GETDATE()
           ,{plist.PriceList}
           ,'{plist.ListName}'
           ,{plist.InternItemCode}
           ,'{plist.Barcode}'
           ,{plist.Price})";

            return sql;
        }

        public string UpdatePList(int id, PriceListModel plist)
        {
            var sql = $@"
UPDATE [PRLIST0]
   SET [Code] = {plist.Code}
      ,[Name] = '{plist.Name}'
      ,[UpdateAt] = GETDATE()
      ,[PriceList] = {plist.PriceList}
      ,[ListName] = '{plist.ListName}'
      ,[InternItemCode] = {plist.InternItemCode}
      ,[Barcode] = '{plist.Barcode}'
      ,[Price] = {plist.Price}
 WHERE Id = {id}";

            return sql;
        }
    }
}
