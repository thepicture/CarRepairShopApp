using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRepairShopApp.Model
{
    class Manager
    {
        public static CarRepairShopBaseEntities Context = new CarRepairShopBaseEntities();
        public static User CurrentUser = new User();
    }
}
