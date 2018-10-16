using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KIntegration
{
    class Program
    {
        private static int time = 60;
        static void Main(string[] args)
        {
            
            StartThread();
        }

        static void StartThread()
        {
            var bp = new Thread(BusinessPartner);
            bp.Start();

            var it = new Thread(Items);
            it.Start();

            var pl = new Thread(PriceList);
            pl.Start();
        }

        static void PriceList()
        {
            string table, key;
            int count;
            DateTime date;
            var sync = new Firebox.Repositories.SyncRepository();

            while (true)
            {
                var started = DateTime.Now;

                #region Price List
                table = Firebox.Repositories.PriceListRepository.Table;

                sync.GetLast(table, out key, out count, out date);
                var fbPList = new OnClick.Repositories.ListaPrecoRepository();

                var plists = fbPList.GetFrom(date, count);

                if (plists.Count > 0)
                {
                    var msPList = new Firebox.Repositories.PriceListRepository();
                    msPList.Add(plists);
                    sync.Register(table, key, plists.Max(t => t.InternItemCode), started);
                }
                #endregion

                System.Threading.Thread.Sleep(time * 1000);
            }
        }

        static void BusinessPartner()
        {
            string table, key;
            int count;
            DateTime date;
            var sync = new Firebox.Repositories.SyncRepository();

            while (true)
            {
                var started = DateTime.Now;

                #region Business Partner 
                table = Firebox.Repositories.BusinessPartnerRepository.Table;

                sync.GetLast(table, out key, out count, out date);
                var fbClient = new OnClick.Repositories.ClientesRepository();

                var bpartner = fbClient.GetFrom(date, count);

                if (bpartner.Count > 0)
                {
                    var msClient = new Firebox.Repositories.BusinessPartnerRepository();
                    msClient.Add(bpartner);
                    sync.Register(table, key, bpartner.Max(t => t.InterCode), started);
                }
                #endregion

                System.Threading.Thread.Sleep(time * 1000);
            }
        }

        static void Items()
        {
            string table, key;
            int count;
            DateTime date;
            var sync = new Firebox.Repositories.SyncRepository();

            while (true)
            {
                var started = DateTime.Now;

                #region Items
                table = Firebox.Repositories.ItemRepository.Table;

                sync.GetLast(table, out key, out count, out date);
                var fbItem = new OnClick.Repositories.ProdutosRepository();

                var items = fbItem.GetFrom(date, count);

                if (items.Count > 0)
                {
                    var msItem = new Firebox.Repositories.ItemRepository();
                    msItem.Add(items);
                    sync.Register(table, key, items.Max(t => t.InternCode), started);
                }
                #endregion

                System.Threading.Thread.Sleep(time * 1000);
            }
        }
    }
}