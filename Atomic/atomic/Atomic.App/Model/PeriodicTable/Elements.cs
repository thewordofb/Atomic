using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.ApplicationModel;
using Windows.Storage;


namespace Atomic.Elements
{
    [XmlRoot("Element")]
    public class ChemElement
    {
        [XmlAttribute("Englishelementname")]
        public string ElementName
        {
            get;
            set;
        }

        [XmlAttribute("atomicnum")]
        public string AtomicNumber
        {
            get;
            set;
        }

        [XmlAttribute("atomicsymbol")]
        public string AtomicSymbol
        {
            get;
            set;
        }

        [XmlAttribute("atomicmass")]
        public string AtomicMass
        {
            get;
            set;
        }

        [XmlAttribute("atomicmassIUPAC")]
        public string AtomicMassIUPAC
        {
            get;
            set;
        }

        [XmlAttribute("MeltingPointC")]
        public string MeltingPointC
        {
            get;
            set;
        }

        [XmlAttribute("BoilingPointC")]
        public string BoilingPointC
        {
            get;
            set;
        }

        [XmlAttribute("State")]
        public string State
        {
            get;
            set;
        }

        [XmlAttribute("densitygmL")]
        public string DensitygmL
        {
            get;
            set;
        }

        [XmlAttribute("electronegativityPauling")]
        public string ElectronegativityPauling
        {
            get;
            set;
        }

        [XmlAttribute("firstionizationpotentialeV")]
        public string FirstIonizationPotentialeV
        {
            get;
            set;
        }

        [XmlAttribute("secondionizationpotentialeV")]
        public string SecondIonizationPotentialeV
        {
            get;
            set;
        }

        [XmlAttribute("thirdionizationpotentialeV")]
        public string ThirdIonizationPotentialeV
        {
            get;
            set;
        }

        [XmlAttribute("firstionizationpotentialkJmol")]
        public string FirstIonizationPotentialkJmol
        {
            get;
            set;
        }

        [XmlAttribute("secondionizationpotentialkJmol")]
        public string SecondIonizationPotentialkJmol
        {
            get;
            set;
        }

        [XmlAttribute("thirdionizationpotentialkJmol")]
        public string ThirdIonizationPotentialkJmol
        {
            get;
            set;
        }

        [XmlAttribute("ElectronAffinityeV")]
        public string ElectronAffinityeV
        {
            get;
            set;
        }

        [XmlAttribute("ElectronAffinitykJmol")]
        public string ElectronAffinitykJmol
        {
            get;
            set;
        }

        [XmlAttribute("PredictedElectronConfiguration")]
        public string PredictedElectronConfiguration
        {
            get;
            set;
        }

        [XmlAttribute("ObservedElectronConfiguration")]
        public string ObservedElectronConfiguration
        {
            get;
            set;
        }

        [XmlAttribute("atomicradiusA")]
        public string AtomicRadiusA
        {
            get;
            set;
        }

        [XmlAttribute("ionicradiusA")]
        public string IonicRadiusA
        {
            get;
            set;
        }

        [XmlAttribute("covalentradiusA")]
        public string CovalentRadiusA
        {
            get;
            set;
        }

        [XmlAttribute("Period")]
        public string Period
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int Row
        {
            get
            {
                int row = int.Parse(Period);

                if (row <= 7)
                {
                    return int.Parse(Period) - 1;
                }
                else
                {
                    return row;
                }
            }
            set
            {
            }
        }

        [XmlAttribute("Groupnum")]
        public string GroupNum
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int Column
        {
            get
            {
                return int.Parse(GroupNum) - 1;
            }
            set
            {
            }
        }

        [XmlIgnore()]
        public Windows.UI.Xaml.Media.Brush ElementColor
        {
            get
            {
                int group = int.Parse(GroupNum);
                int period = int.Parse(Period);

                if (group == 1)
                {
                    return new SolidColorBrush(Colors.DimGray);
                }
                else if (group == 2)
                {
                    Color c = Colors.DodgerBlue;
                    /*new Color();
                    c.A = 0xff;
                    c.R = 0x62;
                    c.G = 0x8F;
                    c.B = 0x4F;*/

                    return new SolidColorBrush(c);
                }
                else if ((group >= 3 && group <= 12) || (period > 7))
                {
                    Color c = new Color();
                    c.A = 0xff;
                    c.R = 0x19;
                    c.G = 0xb4;
                    c.B = 0x19;

                    return new SolidColorBrush(c);
                }
                else if ((group == 13 && period >= 3) || (group == 14 && period >= 4) || (group == 15 && period >= 5) || (group == 16 && period >= 6))
                {
                    Color c = Colors.Crimson;
                        /*new Color();
                    c.A = 0xff;
                    c.R = 0x7d;
                    c.G = 0x15;
                    c.B = 0x6;*/

                    return new SolidColorBrush(c);
                }
                else if (group == 18)
                {
                    //Indigo
                    Color c = Colors.DarkOrchid;
                        /*new Color();
                    c.A = 0xff;
                    c.R = 0x4c;
                    c.G = 0x2f;
                    c.B = 0x4c;*/

                    return new SolidColorBrush(c);
                    //4c2f4c
                }
                else
                {
                    Color c = Colors.Goldenrod;
                    /*new Color();
                    c.A = 0xff;
                    c.R = 0xE6;
                    c.G = 0x2C;
                    c.B = 0x00;*/

                    return new SolidColorBrush(c);
                }

            }
            set
            {
            }
        }

        public ChemElement()
        {
            Properties = new ObservableCollection<ChemElementProperty>();
        }

        [XmlIgnore()]
        public ObservableCollection<ChemElementProperty> Properties
        {
            get;
            set;
        }

        public ChemElement(string atomicnum, string symbol)
        {
            AtomicNumber = atomicnum;
            AtomicSymbol = symbol;
            Properties = new ObservableCollection<ChemElementProperty>();
        }

        public void CreatePropertyList()
        {
            Properties.Add( new ChemElementProperty("Atomic Weight", AtomicMass, Unit.UnitTypes.None) );
            Properties.Add( new ChemElementProperty("Atomic Number", AtomicNumber, Unit.UnitTypes.None) );
            Properties.Add( new ChemElementProperty("Atomic Radius", AtomicRadiusA, Unit.UnitTypes.Angstrom) );
            Properties.Add( new ChemElementProperty("Atomic Volume", "", Unit.UnitTypes.CubicCentimeterPerMole) );
            Properties.Add( new ChemElementProperty("Melting Point", MeltingPointC, Unit.UnitTypes.Celsius) );
            Properties.Add( new ChemElementProperty("Boiling Point", BoilingPointC, Unit.UnitTypes.Celsius) );
            Properties.Add( new ChemElementProperty("Density", DensitygmL, Unit.UnitTypes.GramsPerLiter) );
            Properties.Add( new ChemElementProperty("Oxidation States", "", Unit.UnitTypes.None) );
            Properties.Add( new ChemElementProperty("Electron Configuration", ObservedElectronConfiguration, Unit.UnitTypes.None) );
            Properties.Add( new ChemElementProperty("Electronegativity", ElectronegativityPauling, Unit.UnitTypes.Pauling) );
            Properties.Add( new ChemElementProperty("Specific Heat", "", Unit.UnitTypes.JoulesPerKelvin) );
            Properties.Add( new ChemElementProperty("Heat of Fusion", "", Unit.UnitTypes.KiloJoulesPerMole) );
            Properties.Add( new ChemElementProperty("Ionization Potential", FirstIonizationPotentialkJmol, Unit.UnitTypes.KiloJoulesPerMole) );
            Properties.Add( new ChemElementProperty("Heat of Vaporization", "", Unit.UnitTypes.KiloJoulesPerMole) );
            Properties.Add( new ChemElementProperty("Thermal Conductivity", "", Unit.UnitTypes.WattsPerMeterKelvin) );
        }
    }



}
