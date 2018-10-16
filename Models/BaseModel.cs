using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIntegration.Models
{
    public interface IBaseModel<T> where T : BaseModel
    {
        void Update(T model);
    }
    public class BaseModel
    {
        public BaseModel()
        {
            UUID = System.Guid.NewGuid();
            var data = System.DateTime.Now;
            CreatedAt = data;
            UpdateAt = data;
        }

        public int Id { get; set; }
        public int Indentify { get; set; }
        public virtual int FatherId { get; protected set; }
        public Guid UUID { get; set; }
        public virtual int? Code { get; set; }
        public virtual String Name { get; set; }
        public String Unique { get => UUID.ToString("N"); }
        public bool Actived { get; set; }
        public bool Blocked { get; set; }
        public Enums.BaseStatus Status { get; set; }
        public DateTime? ValidUntil { get; set; }
        public virtual int VisOrder { get; set; }
        public int UserIdProper { get; set; }
        public DateTime CreatedAt { get; protected set; }
        public int UserIdUpdate { get; set; } 
        public DateTime UpdateAt { get; protected set; }

        public virtual void Update(BaseModel model)
        {
            Code = model.Code;
            Name = model.Name;
            Actived = model.Actived;
            Blocked = model.Blocked;
            ValidUntil = model.ValidUntil;
            UpdateAt = System.DateTime.Now;
        }
    }
}
