using System;
using System.Collections.Generic;
using System.Text;
using GIRUBotV3.Modules;

namespace GIRUBotV3.Models
{
    class ExclusiveRoles
    {
        public static string EU
        {
            get { return "EU"; }
        }
        public static string NA
        {
            get { return "NA"; }
        }
        public static string RU
        {
            get { return "RU"; }
        }
        public static string SA
        {
            get { return "SA"; }
        }
        public static string OCE
        {
            get { return "OCE"; }
        }
        public static string ZA
        {
            get { return "ZA"; }
        }
        public static string noob
        {
            get { return "noob"; }
        }
        private static List<string> _exclusiveRoles;
        public static List<string> Exclusive_roles
        {
            get
            {
                if (_exclusiveRoles == null)
                {
                    _exclusiveRoles = new List<string>();
                }
                _exclusiveRoles.AddMany(EU, NA, RU, SA, OCE, ZA, noob);
                return _exclusiveRoles;
            }
        }
    }
}

