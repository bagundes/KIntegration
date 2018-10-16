using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace KIntegration.Firebox.Repositories
{
    public class SyncRepository
    {
        public void Register(string table, string key, int count, DateTime date)
        {
            try
            {
                var sql = String.Empty;

                if (Exist(table))
                    sql = Update(table, key, count, date);
                else
                    sql = Add(table, key, count, date);

                using (var ms = new MSConn())
                {
                    ms.ExecuteNonQuery(sql);
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void GetLast(string table, out string key, out int count, out DateTime date)
        {
            key = String.Empty;
            count = 0;
            date = new DateTime();

            var sql = $"SELECT [Updated], [Key], [Count] FROM SYNCM0 WHERE [Table] ='{table}'";

            using (var ms = new MSConn())
            {
                var reader = ms.Reader(sql);
                if(reader.Read())
                {
                    date = DateTime.ParseExact(reader["Updated"].ToString(),
                        "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    count = int.Parse(reader["Count"].ToString());
                    key = reader["Key"].ToString();
                }

                reader.Close();
            }
        }

        private bool Exist(string table)
        {
            using (var ms = new MSConn())
            {
                var sql = $"SELECT 1 FROM [SYNCM0] WHERE [Table] = '{table}'";

                var reader = ms.Reader(sql);
                var exist = reader.Read();
                reader.Close();

                return exist;
            }
        }

        private string Add(string table, string key, int count, DateTime date)
        {
            var sql = $@"
INSERT INTO [SYNCM0]
           ([FatherId]
           ,[UUID]
           ,[Actived]
           ,[Blocked]
           ,[Status]
           ,[VisOrder]
           ,[UserIdProper]
           ,[CreatedAt]
           ,[UserIdUpdate]
           ,[UpdateAt]
           ,[Table]
           ,[Updated]
           ,[Key]
           ,[Count])
     VALUES
           (0
           ,'{System.Guid.NewGuid()}'
           ,1
           ,2
           ,10
           ,0
           ,-1
           ,GETDATE()
           ,-1
           ,GETDATE()
           ,'{table}'
           ,'{date.ToString("yyyy-MM-dd hh:mm:ss")}'
           ,'{key}'
           ,{count})";

            return sql;
        }

        private string Update(string table, string key, int count, DateTime date)
        {
            var sql = $@"
UPDATE [SYNCM0]
   SET [Updated] = '{date.ToString("yyyy-MM-dd hh:mm:ss")}'
      ,[Key] = '{key}'
      ,[Count] = {count}
 WHERE [Table] = '{table}'";


            return sql;
        }
    }
}