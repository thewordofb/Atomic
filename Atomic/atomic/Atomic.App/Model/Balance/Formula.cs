using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;
using Atomic.App.Model.Balance;

namespace Atomic.App.Model
{
    public class Formula
    {
        public List<Molecule> Reactants { get; set; }
        public List<Molecule> Products { get; set; }

        public Formula(string[] reactants, string[] products)
        {
            Reactants = new List<Molecule>();
            Products = new List<Molecule>();

            foreach (string reactant in reactants)
            {
                Reactants.Add(new Molecule(reactant));
            }

            foreach (string product in products)
            {
                Products.Add(new Molecule(product));
            }
        }

        public List<string> GetElements()
        {
            List<string> elements = new List<string>();

            foreach(Molecule m in Reactants)
            {
                foreach(MoleculeItem mi in m.Items)
                {
                    if(mi is Element)
                    {
                        if( !elements.Exists( x => x == ((Element)mi).Symbol) )
                        {
                            elements.Add( ((Element)mi).Symbol );
                        }
                    }
                    if(mi is ElementGroup)
                    {
                        foreach( Element e in ((ElementGroup)mi).Elements)
                        {
                            if (!elements.Exists(x => x == e.Symbol))
                            {
                                elements.Add(e.Symbol);
                            }
                        }
                    }
                }
            }

            return elements;
        }

        private static char[] position = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public (List<int> coefficients, string displayString) GetEquationForElement(string symbol)
        {
            List<int> coefficientList = new List<int>();
            string displayString = symbol + ": \t";

            int positionCount = 0;

            bool isFirstReactant = true;
            foreach (Molecule m in Reactants)
            {
                if( !isFirstReactant )
                {
                    displayString += " + ";
                }
                else
                {
                    isFirstReactant = false;
                }

                int coefficient = 0;
                foreach (MoleculeItem mi in m.Items)
                {
                    if (mi is Element)
                    {
                        if(((Element)mi).Symbol == symbol)
                        {
                            coefficient += ((Element)mi).Subscript;
                        }
                    }
                    if (mi is ElementGroup)
                    {
                        foreach (Element e in ((ElementGroup)mi).Elements)
                        {
                            if (((Element)e).Symbol == symbol)
                            {
                                coefficient += (((Element)e).Subscript * ((ElementGroup)mi).Subscript);
                            }
                        }
                    }
                }

                displayString += coefficient.ToString() + "·" + position[positionCount];
                positionCount++;

                coefficientList.Add(coefficient);
            }

            displayString += " = ";

            bool isFirstProduct = true;
            foreach (Molecule m in Products)
            {
                if (!isFirstProduct)
                {
                    displayString += " + ";
                }
                else
                {
                    isFirstProduct = false;
                }

                int coefficient = 0;
                foreach (MoleculeItem mi in m.Items)
                {
                    if (mi is Element)
                    {
                        if (((Element)mi).Symbol == symbol)
                        {
                            coefficient += ((Element)mi).Subscript;
                        }
                    }
                    if (mi is ElementGroup)
                    {
                        foreach (Element e in ((ElementGroup)mi).Elements)
                        {
                            if (((Element)e).Symbol == symbol)
                            {
                                coefficient += (((Element)e).Subscript * ((ElementGroup)mi).Subscript);
                            }
                        }
                    }
                }

                displayString += coefficient.ToString() + "·" + position[positionCount];
                positionCount++;

                coefficientList.Add(coefficient);
            }


            return (coefficientList, displayString);
        }
    }

}
