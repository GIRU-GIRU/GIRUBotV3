using GIRUBotV3.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace GIRUBotV3.Models
{
    public static class FilterTypes
    {
        private static List<string> _FilterTypesStrings;
        public static List<string> FilterTypesStrings
        {
            get
            {
                if (_FilterTypesStrings == null)
                {
                    _FilterTypesStrings = new List<string>();
                }
                _FilterTypesStrings.AddMany("Bangs", "Female", "Female_2", "Glasses", "Goatee", "Heisenberg", "Hipster", "Hitman", "Hollywood", "Hot", "Impression", "Lion", "Makeup", "Malke", "Mustache", "Old", "Pan", "Smile", "Smile_2", "Wave", "Young");
                return _FilterTypesStrings;
            }
        }
            
    }
}
