using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Documents;
using Atomic.App.Model.Balance;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using MathNet.Numerics.LinearAlgebra;

namespace Atomic.App.Model
{
    public class BalancedFormula
    {
        public BalancedFormula(string formula, List<string> steps, bool isError)
        {
            Result = formula;
            Steps = new List<string>(steps);
            DisplayFormula = new List<Run>();
            IsError = isError;
        }

        public BalancedFormula()
        {
            Result = string.Empty;
            Steps = new List<string>();
            DisplayFormula = new List<Run>();
            IsError = false;
        }

        public List<Run> DisplayFormula;
        public List<Run> StepsFormula;
        public string Result { get; set; }
        public List<string> Steps { get; set; }
        public bool IsError { get; set; }
        public string AMatrixDisplay { get; set; }
        public string BMatrixDisplay { get; set; }
        public string FirstResultDisplay { get; set; }
        public string SecondResultDisplay { get; set; }
        public string GCD { get; set; }
    }

    public static class FormulaBalancer
    {
        public static BalancedFormula Balance(string formulaText)
        {
            BalancedFormula result = new BalancedFormula();
            try
            {
                Formula formula = FormulaBalancer.Parse(formulaText);

                List<string> equations = FormulaBalancer.CalculateEquations(formula);
                result.Steps.AddRange(equations);

                result.StepsFormula = FormulaBalancer.DisplayTextWithCoeffients(formula);

                var AMatrix = FormulaBalancer.CreateAMatrix(formula);
                result.AMatrixDisplay = AMatrix.display;

                var BMatrix = FormulaBalancer.CreateBMatrix(formula);
                result.BMatrixDisplay = BMatrix.display;

                double[,] dAMatrix = new double[AMatrix.matrix.GetLength(0), AMatrix.matrix.GetLength(0)];
                for(int x = 0; x < AMatrix.matrix.GetLength(0); x++)
                {
                    for(int y = 0; y < AMatrix.matrix.GetLength(0); y++)
                    {
                        dAMatrix[x, y] = (double)AMatrix.matrix[x, y];
                    }        
                }

                double[] dBMatrix = new double[BMatrix.matrix.Count()];
                for(int z = 0; z < BMatrix.matrix.Count(); z++)
                {
                    dBMatrix[z] = (double)BMatrix.matrix[z];
                }

                var firstResult = FormulaBalancer.CalculateCoefficients(dAMatrix, dBMatrix);
                result.FirstResultDisplay = firstResult.display;

                var secondResult = FormulaBalancer.CalculateDeterminant(dAMatrix);
                result.SecondResultDisplay = secondResult.ToString();

                int moleculeCount = formula.Reactants.Count() + formula.Products.Count();
                int[] coefficients = new int[moleculeCount];
                for(int a = 0; a < moleculeCount; a++)
                {
                    if( a != moleculeCount - 1)
                    {
                        coefficients[a] = (int)Math.Round(Math.Abs(firstResult.vector[a]));
                    }
                    else
                    {
                        coefficients[a] = (int)Math.Round(Math.Abs(secondResult));
                    }
                }

                int gcd = GCD(coefficients);
                result.GCD = gcd.ToString();
                for( int b = 0; b < moleculeCount; b++)
                {
                    coefficients[b] = coefficients[b] / gcd;
                }

                result.DisplayFormula = FormulaBalancer.DisplayText(formula, coefficients);

            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Result = "Syntax Error: " + ex.Message;
            }

            return result;
        }

        private static char[] position = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        private static List<Run> DisplayText(Formula formula, int[] coefficients)
        {
            List<Run> display = new List<Run>();
            int count = 0;

            for (int x = 0; x < formula.Reactants.Count; x++)
            {
                display.Add(new Run() { Text = coefficients[count].ToString("F0") });
                display.AddRange(formula.Reactants[x].DisplayText());
                if (x < formula.Reactants.Count - 1)
                {
                    display.Add(new Run() { Text = " + " });
                }
                count++;
            }

            display.Add(new Run() { Text = " → " });

            for (int x = 0; x < formula.Products.Count; x++)
            {
                display.Add(new Run() { Text = coefficients[count].ToString("F0") });
                display.AddRange(formula.Products[x].DisplayText());
                if (x < formula.Products.Count - 1)
                {
                    display.Add(new Run() { Text = " + " });
                }
                count++;
            }

            return display;
        }

        private static List<Run> DisplayTextWithCoeffients(Formula formula)
        {
            List<Run> display = new List<Run>();
            int positionCount = 0;

            SolidColorBrush coefficientBrush = Application.Current.Resources["SystemAccentBrushLightest"] as SolidColorBrush;

            for (int x = 0; x < formula.Reactants.Count; x++)
            {
                display.Add(new Run() { Text = position[positionCount].ToString(), Foreground = coefficientBrush });
                display.AddRange(formula.Reactants[x].DisplayText());
                if (x < formula.Reactants.Count - 1)
                {
                    display.Add(new Run() { Text = " + " });
                }
                positionCount++;
            }

            display.Add(new Run() { Text = " → " });

            for (int x = 0; x < formula.Products.Count; x++)
            {
                display.Add(new Run() { Text = position[positionCount].ToString(), Foreground = coefficientBrush });
                display.AddRange(formula.Products[x].DisplayText());
                if (x < formula.Products.Count - 1)
                {
                    display.Add(new Run() { Text = " + " });
                }
                positionCount++;
            }

            return display;
        }

        static int GCD(int[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private static double CalculateDeterminant(double[,] matrix)
        {
            Matrix<double> dMatrix = Matrix<double>.Build.DenseOfArray(matrix);
            double determinant = dMatrix.Determinant();
            return determinant;
        }

        private static (string display, double[] vector) CalculateCoefficients(double[,] AMatrix, double[] BMatrix)
        {
            Matrix<double> aMatrix = Matrix<double>.Build.DenseOfArray(AMatrix);
            Vector<double> bMatrix = Vector<double>.Build.DenseOfArray(BMatrix);

            Vector<double> result = aMatrix.Inverse().Multiply(bMatrix).Multiply(aMatrix.Determinant());

            string displayString = string.Empty;
            for(int x = 0; x < result.Count(); x++)
            {
                displayString += result[x].ToString("F0");
                if( x != result.Count() - 1)
                {
                    displayString += "\n";
                }
                
            }
            return (displayString, result.ToArray());
        }

        private static (string display, int[,] matrix) CreateAMatrix(Formula formula)
        {
            string display = string.Empty;
            List<string> elements = formula.GetElements();
            int dimension = elements.Count();

            int[,] matrix = new int[dimension, dimension];

            int depth = 0;
            foreach (string element in elements)
            {
                var elementEquation = formula.GetEquationForElement(element);

                for (int x = 0; x < dimension; x++)
                {
                    if (x < elementEquation.coefficients.Count() - 1)
                    {
                        matrix[depth, x] = elementEquation.coefficients[x];
                        display += matrix[depth, x] + " ";
                    }
                    else
                    {
                        matrix[depth, x] = 1;  //We need square
                        display += matrix[depth, x] + " ";
                    }
                }

                if (element != elements.Last())
                {
                    display += "\n";
                }
                depth++;
            }

            return (display, matrix);
        }

        private static (string display, int[] matrix) CreateBMatrix(Formula formula)
        {
            string display = string.Empty;
            List<string> elements = formula.GetElements();
            int dimension = elements.Count();

            int[] matrix = new int[dimension];

            int depth = 0;
            foreach(string element in elements)
            {
                var elementEquation = formula.GetEquationForElement(element);

                matrix[depth] = elementEquation.coefficients.Last();
                display += matrix[depth].ToString();

                if( element != elements.Last())
                {
                    display += "\n";
                }

                depth++;
            }

            return (display, matrix);
        }

        private static List<string> CalculateEquations(Formula formula)
        {
            List<String> balanceEquations = new List<string>();

            List<string> elements = formula.GetElements();

            foreach(string element in elements)
            {
                var elementEquation = formula.GetEquationForElement(element);

                balanceEquations.Add(elementEquation.displayString);
            }

            return balanceEquations;
        }

        private static Formula Parse(string formula)
        {
            string[] formulaParts = formula.Split(new char[]{ '=' });

            if( formulaParts.Count() != 2 )
            {
                throw new Exception("Missing equal sign");
            }
            else
            {
                string[] lhsReactants = formulaParts[0].Split(new char[]{ '+' });
                string[] rhsProducts = formulaParts[1].Split(new char[]{ '+' });

                return new Formula(lhsReactants, rhsProducts);
            }

            
        }





    }
}
