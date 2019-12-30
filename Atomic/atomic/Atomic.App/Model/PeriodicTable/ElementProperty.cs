using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomic.Elements
{
    public class ChemElementProperty
    {
        public ChemElementProperty(string title, string value, Unit.UnitTypes unitType)
        {
            Title = title;
            Value = value;
            UnitType = Unit.UnitFactory(unitType);
        }

        public string Title
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public Unit UnitType
        {
            get;
            set;
        }
    }
}
