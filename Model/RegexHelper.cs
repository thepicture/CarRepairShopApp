using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarRepairShopApp.Model
{
    class RegexHelper
    {
        public static Regex NumberRegex = new Regex(pattern: @"[0-9]+");
        public static Regex NameRegex = new Regex(pattern: @"\w+\s\w+\s\w+");
        public static Regex PassCodeRegex = new Regex(pattern: @"[0-9]{6}");
        public static Regex PassNumRegex = new Regex(pattern: @"[0-9]{4}");
    }
}
