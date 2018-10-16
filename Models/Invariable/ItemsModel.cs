using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Models.Invariable
{
    public class ItemModel : BaseModel, IBaseModel<ItemModel>
    {
        public ItemModel() { }

        public int InternCode { get => (int)Code; set => Code = value; }
        public override string Name { get; set; }
        public string SimpleName { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }

        public ItemModel(ref FbDataReader reader)
        {
            InternCode = int.Parse(reader["INTERNCODE"].ToString());
            Name = reader["NAME"].ToString();
            SimpleName = reader["SIMPLENAME"].ToString();
            Unit = reader["UNIT"].ToString();
            Actived = reader["ACTIVED"].ToString() == "1";
            Barcode = reader["BARCODE"].ToString();
        }

        public void Update(ItemModel model)
        {
            InternCode = model.InternCode;
            Name = model.Name;
            SimpleName = model.SimpleName;
            Unit = model.Unit;
            Barcode = model.Barcode;

            base.Update(model);
        }
    }
}


