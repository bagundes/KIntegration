using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration
{
    public class FBConn : IDisposable
    {
        public FbConnection Conn { get; protected set; }

        public FBConn()
        {
            Connect();
        }

        private void Connect()
        {
            var strConn = Properties.Resources.FBConn;

            Conn = new FbConnection(strConn);
            Conn.Open();
        }

        public void Dispose()
        {
            Conn.Close();
        }
    }
}
