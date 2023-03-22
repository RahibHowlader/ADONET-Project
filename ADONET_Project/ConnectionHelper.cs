using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_Project
{
    public static class ConnectionHelper
    {
        public static string ConnectionString
        {
            get
            {

                string db = Path.Combine(Path.GetFullPath(@"..\..\"), "DeviceDB.mdf");
                return $@"Data Source=(localdb)\mssqllocaldb;AttachDbFilename={db};Initial Catalog=OrderManagement;Trusted_Connection=True";
            }
        }
    }
}
