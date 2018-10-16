using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Models.Invariable
{

    public class AddressModel : BaseModel, IBaseModel<AddressModel>
    {

        public AddressModel(Enums.AddressType type = Enums.AddressType.None)
        {
            Type = type;
        }

        [Required]
        [Display(Name = "Tipo")]
        public Enums.AddressType Type { get; set; }

        [Required]
        [Display(Name = "Endereço 1")]
        public string Address1 { get; set; }

        [Display(Name = "Endereço 2")]
        public string Address2 { get; set; }
        [Required]
        [Display(Name = "Bairro")]
        public string Block { get; set; }
        [Required]
        [Display(Name = "Localização (ID)")]
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "Location")]
        [NotMapped]
        public CityModel Location { get; set; }
        [Display(Name = "CEP")]
        public string ZipCode { get; set; }

        [Display(Name = "Google")]
        public string GoogleCode { get; set; }
        [Display(Name = "Geolocalização")]
        public string Geolocation { get; set; }

        public void Update(AddressModel model)
        {
            Type = model.Type;
            Address1 = model.Address1;
            Address2 = model.Address2;
            Block = model.Block;
            LocationId = model.LocationId;
            Location = model.Location;
            ZipCode = model.ZipCode;
            GoogleCode = model.GoogleCode;
            Geolocation = model.Geolocation;

            base.Update(model);
        }

    }
}