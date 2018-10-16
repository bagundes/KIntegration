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
    public class PriceListModel : BaseModel, IBaseModel<PriceListModel>
    {
        public PriceListModel() { }

        [Required]
        [Display(Name = "Lista de Preço (ID)")]
        public int PriceList { get => (int)Code; set => Code = value; }
        [Required]
        [Display(Name = "Lista de Preço")]
        public string ListName { get => Name; set => Name = value; }
        [Required]
        [Display(Name = "Cód. Item")]
        public int InternItemCode { get; set; }
        [Required]
        [Display(Name = "Código de barras")]
        public string Barcode { get; set; }
        [Required]
        [Display(Name = "Preço")]
        public double Price { get; set; }

        public void Update(PriceListModel model)
        {
            PriceList = model.PriceList;
            ListName = model.ListName;
            InternItemCode = model.InternItemCode;
            Barcode = model.Barcode;
            Price = model.Price;

            base.Update(model);
        }


        public PriceListModel(ref FbDataReader reader)
        {
            PriceList = int.Parse(reader["PRICELIST"].ToString());
            ListName = reader["NAME"].ToString();
            InternItemCode = int.Parse(reader["INTERNCODE"].ToString());
            Barcode = reader["BARCODE"].ToString();
            var price = String.IsNullOrEmpty(reader["PRICE"].ToString()) ? "0" : reader["PRICE"].ToString();
            Price = double.Parse(price);

        }

    }
}


