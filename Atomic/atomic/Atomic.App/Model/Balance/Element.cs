using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;

namespace Atomic.App.Model.Balance
{
    public class Element : MoleculeItem
    {
        public int Subscript { get; set; }
        public int Charge { get; set; }
        public string Symbol { get; set; }

        public Element(string element)
        {
            SourceText = element;
            Subscript = 1;
            Charge = 0;

            Parse(element);
        }

        private void Parse(string element)
        {
            for (int position = 0; position < SourceText.Length; position++)
            {
                if (char.IsLetter(element[position]))
                {
                    int startPosition = position;
                    while (position + 1 < element.Length && element[position + 1] != '[' && !char.IsNumber(element[position + 1]))
                    {
                        position++;
                    }
                    Symbol = element.Substring(startPosition, position + 1 - startPosition);
                }
                else if (element[position] == '[')
                {
                    int startPosition = position;
                    while (position + 1 < element.Length && element[position + 1] != ']')
                    {
                        position++;
                    }

                    string charge = element.Substring(startPosition + 1, position - startPosition);
                    Charge = int.Parse(charge);
                }
                else if (char.IsNumber(element[position]))
                {
                    int startPosition = position;
                    while (position + 1 < element.Length && char.IsNumber(element[position + 1]))
                    {
                        position++;
                    }
                    Subscript = int.Parse(element.Substring(startPosition, position + 1 - startPosition));
                }
            }
        }

        public override List<Run> DisplayText()
        {
            List<Run> display = new List<Run>();

            display.Add(new Run() { Text = Symbol });
            if (Subscript > 1)
            {
                Run r = new Run() { Text = Subscript.ToString() };
                Typography.SetVariants(r, Windows.UI.Xaml.FontVariants.Subscript);
                display.Add(r);
            }
            if (Charge != 0)
            {
                if (Charge < 0)
                {
                    //This won't convert a - to a ⁻ for some reason?
                    Run r1 = new Run() { Text = "⁻" };
                    display.Add(r1);
                }

                Run r = new Run() { Text = Math.Abs(Charge).ToString() };
                Typography.SetVariants(r, Windows.UI.Xaml.FontVariants.Superscript);
                display.Add(r);
            }

            return display;
        }
    }

    public class ElementGroup : MoleculeItem
    {
        public int Subscript { get; set; }
        public List<Element> Elements { get; set; }

        public ElementGroup(string elementGroup)
        {
            Elements = new List<Element>();
            SourceText = elementGroup;
            Subscript = 1;

            Parse(elementGroup);
        }

        private void Parse(string elementGroup)
        {
            for (int position = 0; position < SourceText.Length; position++)
            {
                if (char.IsUpper(SourceText[position]))
                {
                    int startPosition = position;
                    while (position + 1 < SourceText.Length && SourceText[position + 1] != ')' && !char.IsUpper(SourceText[position + 1]))
                    {
                        position++;
                    }
                    string element = SourceText.Substring(startPosition, position + 1 - startPosition);

                    Elements.Add(new Element(element));
                }
                else if (SourceText[position] == ')')
                {
                    Subscript = int.Parse(SourceText.Substring(position + 1));
                }
            }
        }

        public override List<Run> DisplayText()
        {
            List<Run> display = new List<Run>();

            display.Add(new Run() { Text = "(" });
            foreach (Element e in Elements)
            {
                display.AddRange(e.DisplayText());
            }
            display.Add(new Run() { Text = ")" });
            if (Subscript > 1)
            {
                Run r = new Run() { Text = Subscript.ToString() };
                Typography.SetVariants(r, Windows.UI.Xaml.FontVariants.Subscript);
                display.Add(r);
            }

            return display;
        }
    }
}
