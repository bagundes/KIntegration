using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Models.Invariable
{
    public class CityModel : BaseModel, IBaseModel<CityModel>
    {
        public CityModel()
        {
            UserIdProper = -1;
            UserIdUpdate = 1;
        }

        public string Country { get; set; }
        public int CountryId { get; set; }
        public string CountryShort { get; set; }
        public string State { get; set; }
        public string StateId { get; set; }
        public string StateShort { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string CityShort { get; set; }

        public void Update(CityModel model)
        {
            Country = model.Country;
            CountryId = model.CountryId;
            State = model.State;
            StateId = model.StateId;
            City = model.City;
            CityId = model.CityId;

            base.Update(model);
        }
    }
}