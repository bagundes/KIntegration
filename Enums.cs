using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KIntegration
{
    public class Enums
    {
        public const string USERID = "UserId";
        public const string PERFILID = "PerfilId";
        public enum DocumentType
        {
            None = 0,
            SalesCotation = 10,
            SalesOrder = 20,
        }

        public enum BaseStatus
        {
            Open = 10,
            Close = 20,
            Cancelled = 30,
            Pedent = 40,
        }

        public enum ActionRepository
        {
            None = 0,
            Add = 10,
            Update = 20,
            Delete = 30,
            All = 40,
        }

        public enum PermissionsControl
        {
            Yes = 1,
            No = 0,
            YesAll = 2,
        }

        public enum AddressType
        {
            None = 0,
            BPartnerShip = 10,
            BPartnerBill = 20,
        }

        public enum BPartnerType
        {
            None = 0,
            Customer = 20,
            Lead = 21,
            Supplier = 30,
            Transporter = 40,

        }
    }
}