using KIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Administration
{
    public class SyncModel : BaseModel, IBaseModel<SyncModel>
    {
        public string Table { get; set; }
        public DateTime Updated { get; set; }

        public void Update(SyncModel model)
        {
            throw new NotImplementedException();
        }
    }
}