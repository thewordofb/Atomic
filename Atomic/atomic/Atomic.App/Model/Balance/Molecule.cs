using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;

namespace Atomic.App.Model.Balance
{
    public class Molecule
    {
        public string SourceText { get; set; }
        public int Coefficient { get; set; }
        public List<MoleculeItem> Items { get; set; }

        public Molecule(string molecule)
        {
            Items = new List<MoleculeItem>();
            SourceText = molecule;
            Coefficient = 1;

            Parse(molecule);
        }

        private void Parse(string SourceText)
        {
            for (int position = 0; position < SourceText.Length; position++)
            {
                if (char.IsUpper(SourceText[position]))
                {
                    int startPosition = position;
                    while (position + 1 < SourceText.Length && SourceText[position + 1] != '(' && !char.IsUpper(SourceText[position + 1]))
                    {
                        position++;
                    }
                    string element = SourceText.Substring(startPosition, position + 1 - startPosition);

                    Items.Add(new Element(element));
                }
                else if (SourceText[position] == '(')
                {
                    int startPosition = position;
                    bool foundClosingBrace = false;
                    while (position + 1 < SourceText.Length)
                    {
                        if (SourceText[position + 1] == ')')
                        {
                            foundClosingBrace = true;
                        }

                        if (foundClosingBrace && (SourceText[position + 1] == '(' || char.IsUpper(SourceText[position + 1])))
                        {
                            break;
                        }

                        position++;
                    }

                    string groupItem = SourceText.Substring(startPosition, position + 1 - startPosition);
                    Items.Add(new ElementGroup(groupItem));
                }

            }
        }

        public List<Run> DisplayText()
        {
            List<Run> display = new List<Run>();

            if (Coefficient > 1)
            {
                display.Add(new Run() { Text = Coefficient.ToString() });
            }

            foreach (MoleculeItem mi in Items)
            {
                display.AddRange(mi.DisplayText());
            }

            return display;
        }
    }

    public abstract class MoleculeItem
    {
        public MoleculeItem() {; }
        public abstract List<Run> DisplayText();

        public string SourceText { get; set; }
    }
}
