using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomic.Elements
{
    public class Unit
    {
        public enum UnitTypes
        {
            None,
            Celsius,
            Angstrom,
            CubicCentimeterPerMole,
            GramsPerLiter,
            Pauling,
            JoulesPerKelvin,
            KiloJoulesPerMole,
            WattsPerMeterKelvin
        }

        public static Unit UnitFactory(UnitTypes unit)
        {
            Unit u = null;

            if (unit == UnitTypes.None)
            {
                u = new None();
            }
            if (unit == UnitTypes.Celsius)
            {
                u = new Celsius();
            }
            else if (unit == UnitTypes.Angstrom)
            {
                u = new Angstrom();
            }
            else if (unit == UnitTypes.CubicCentimeterPerMole)
            {
                u = new CubicCentimeterPerMole();
            }
            else if (unit == UnitTypes.GramsPerLiter)
            {
                u = new GramsPerLiter();
            }
            else if (unit == UnitTypes.JoulesPerKelvin)
            {
                u = new JoulesPerKelvin();
            }
            else if (unit == UnitTypes.KiloJoulesPerMole)
            {
                u = new KiloJoulesPerMole();
            }
            else if (unit == UnitTypes.Pauling)
            {
                u = new Pauling();
            }
            else if (unit == UnitTypes.WattsPerMeterKelvin)
            {
                u = new WattsPerMeterKelvin();
            }

            return u;
        }
    }

    public class None : Unit
    {
    }

    public class Celsius : Unit
    {
    }

    public class Angstrom : Unit
    {
    }

    public class CubicCentimeterPerMole : Unit
    {
    }

    public class GramsPerLiter : Unit
    {
    }

    public class Pauling : Unit
    {
    }

    public class JoulesPerKelvin : Unit
    {
    }

    public class KiloJoulesPerMole : Unit
    {
    }

    public class WattsPerMeterKelvin : Unit
    {
    }
}
