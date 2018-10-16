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
    public class ItemRepository
    {
        public const string Table = "Item";

        public void Add(List<ItemModel> items)
        {
            foreach (var item in items)
                Add(item);
        }

        private bool Add(ItemModel item)
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
                        var id = ms.ExecuteNonQuery(AddItem(item));
                        item.Id = int.Parse(id);
                    }
                    else
                        ms.ExecuteNonQuery(UpdateItem(item.Id, item));
                    #endregion

                    //transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Logs.Register(Table, $"{item.InternCode}/{item.Name}", ex);

                    return false;
                }

            }
        }

        private ItemModel Get(ItemModel item)
        {
            using (var ms = new MSConn())
            {
                var sql = @"SELECT Id FROM ITEMS0 WHERE Code = {0};";
                var command = new SqlCommand(String.Format(sql, item.InternCode), ms.Conn);
                var reader = command.ExecuteReader();
                var id = 0;
                if (reader.Read())
                {
                    item.Id = int.Parse(reader[0].ToString());
                }

                return item;
            }
        }

        public string AddItem(ItemModel item)
        {
            var sql = $@"
INSERT INTO [ITEMS0]
           ([FatherId]
           ,[UUID]
           ,[Code]
           ,[Actived]
           ,[Blocked]
           ,[Status]
           ,[VisOrder]
           ,[UserIdProper]
           ,[CreatedAt]
           ,[UserIdUpdate]
           ,[UpdateAt]
           ,[Name]
           ,[SimpleName]
           ,[Unit]
           ,[Barcode])
     VALUES
           (0
           ,'{item.UUID}'
           ,{item.Code}
           ,{(item.Actived ? "1" : "0")}
           ,0
           ,10
           ,0
           ,-1
           ,GETDATE()
           ,-1
           ,GETDATE()
           ,'{item.Name.Replace("'", "''")}'
           ,'{item.SimpleName.Replace("'","''")}'
           ,'{item.Unit}'
           ,'{item.Barcode}')";

            return sql;
        }

        public string UpdateItem(int id, ItemModel item)
        {
            var sql = $@"
UPDATE [ITEMS0]
   SET [Code] = {item.Code}
      ,[Actived] = {(item.Actived ? "1" : "0")}
      ,[UpdateAt] = GETDATE()
      ,[Name] = '{item.Name}'
      ,[SimpleName] = '{item.SimpleName}'
      ,[Unit] = '{item.Unit}'
      ,[Barcode] = '{item.Barcode}'
 WHERE Id = {id}";

            return sql;
        }
    }
}
