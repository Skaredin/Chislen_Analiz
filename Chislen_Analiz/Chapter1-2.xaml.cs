using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chislen_Analiz
{
	/// <summary>
	/// Логика взаимодействия для Chapter1_2.xaml
	/// </summary>
	public partial class Chapter1_2 : Window
	{
		public Chapter1_2()
		{
			InitializeComponent();
		}
		private void SolveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Считывание уравнений из текстовых полей
				string[] equations = new string[]
				{
					Equation1TextBox.Text,
					Equation2TextBox.Text,
					Equation3TextBox.Text,
					Equation4TextBox.Text
				};

				double[,] matrix = new double[equations.Length, equations.Length + 1];
				List<SolutionStep> steps = new List<SolutionStep>();

				for (int i = 0; i < equations.Length; i++)
				{
					ParseEquation(equations[i], matrix, i);
				}

				// Отображаем исходную матрицу
				steps.Add(new SolutionStep { Step = "Исходная матрица:", Result = MatrixToString(matrix) });

				// Применение метода прогонки
				double[] solution = TridiagonalMatrixAlgorithm(matrix, steps);

				// Добавляем окончательное решение в DataGrid
				steps.Add(new SolutionStep
				{
					Step = "Решение:",
					Result = string.Join(", ", solution.Select((value, index) => $"x{index + 1} = {value:F2}"))
				});

				ResultDataGrid.ItemsSource = steps;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private void ParseEquation(string equation, double[,] matrix, int row)
		{
			string pattern = @"([+-]?\d*\.?\d*)?x(\d)";
			MatchCollection matches = Regex.Matches(equation.Replace(" ", ""), pattern);

			foreach (Match match in matches)
			{
				double coeff;
				string coeffStr = match.Groups[1].Value;

				if (string.IsNullOrEmpty(coeffStr) || coeffStr == "+")
					coeff = 1;
				else if (coeffStr == "-")
					coeff = -1;
				else
					coeff = Convert.ToDouble(coeffStr);

				int variableIndex = int.Parse(match.Groups[2].Value) - 1;
				matrix[row, variableIndex] = coeff;
			}

			string rightSidePattern = @"=\s*(-?\d+\.?\d*)";
			Match rightSideMatch = Regex.Match(equation.Replace(" ", ""), rightSidePattern);
			if (rightSideMatch.Success)
			{
				double result = Convert.ToDouble(rightSideMatch.Groups[1].Value);
				matrix[row, matrix.GetLength(1) - 1] = result;
			}
		}

		private double[] TridiagonalMatrixAlgorithm(double[,] matrix, List<SolutionStep> steps)
		{
			int n = matrix.GetLength(0);
			double[] a = new double[n];
			double[] b = new double[n];
			double[] c = new double[n];
			double[] d = new double[n];
			double[] p = new double[n];
			double[] q = new double[n];

			// Инициализация коэффициентов из матрицы
			for (int i = 0; i < n; i++)
			{
				a[i] = i > 0 ? matrix[i, i - 1] : 0;
				b[i] = matrix[i, i];
				c[i] = i < n - 1 ? matrix[i, i + 1] : 0;
				d[i] = matrix[i, n];
			}

			// Прямой ход
			p[0] = -c[0] / b[0];
			q[0] = d[0] / b[0];
			steps.Add(new SolutionStep { Step = "Прямой ход, шаг 0:", Result = $"p[0] = {p[0]:F2}, q[0] = {q[0]:F2}\n" + MatrixToString(a, b, c, d, n) });

			for (int i = 1; i < n; i++)
			{
				double denominator = b[i] + a[i] * p[i - 1];
				p[i] = -c[i] / denominator;
				q[i] = (d[i] - a[i] * q[i - 1]) / denominator;

				steps.Add(new SolutionStep { Step = $"Прямой ход, шаг {i}:", Result = $"p[{i}] = {p[i]:F2}, q[{i}] = {q[i]:F2}\n" + MatrixToString(a, b, c, d, n) });
			}

			// Обратный ход
			double[] x = new double[n];
			x[n - 1] = q[n - 1];
			steps.Add(new SolutionStep { Step = $"Обратный ход, шаг {n - 1}:", Result = MatrixToStringWithSolution(a, b, c, d, x, n, n - 1) });

			for (int i = n - 2; i >= 0; i--)
			{
				x[i] = p[i] * x[i + 1] + q[i];
				steps.Add(new SolutionStep { Step = $"Обратный ход, шаг {i}:", Result = MatrixToStringWithSolution(a, b, c, d, x, n, i) });
			}

			return x;
		}

		private string MatrixToStringWithSolution(double[] a, double[] b, double[] c, double[] d, double[] x, int n, int step)
		{
			string result = "";
			for (int i = 0; i < n; i++)
			{
				result += $"{(i > 0 ? a[i].ToString("F2") : "")}\t{b[i].ToString("F2")}\t{(i < n - 1 ? c[i].ToString("F2") : "")}\t| {d[i].ToString("F2")}\t| ";
				result += i >= step ? $"x[{i}] = {x[i]:F2}" : "\t";
				result += "\n";
			}
			return result;
		}


		private string MatrixToString(double[] a, double[] b, double[] c, double[] d, int n)
		{
			string result = "";
			for (int i = 0; i < n; i++)
			{
				result += $"{(i > 0 ? a[i].ToString("F2") : "")}\t{b[i].ToString("F2")}\t{(i < n - 1 ? c[i].ToString("F2") : "")}\t| {d[i].ToString("F2")}\n";
			}
			return result;
		}

		private string MatrixToString(double[,] matrix)
		{
			string result = "";
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					result += matrix[i, j].ToString("F2") + "\t";
				}
				result += "\n";
			}
			return result;
		}
	}
}

	
