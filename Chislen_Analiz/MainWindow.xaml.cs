using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Chislen_Analiz
{
	public partial class MainWindow : Window
	{
		private double[,] currentMatrix; // Переменная для хранения текущей матрицы

		public MainWindow()
		{
			InitializeComponent();
			Equation1TextBox.Text = "x1 - x2 + 2x3 - x4 = 1";
			Equation2TextBox.Text = "2x1 + 3x3 + x4 = 4";
			Equation3TextBox.Text = "x1 + x2 + 3x3 - x4 = 2";
			Equation4TextBox.Text = "2x1 + x2 + 5x3 - 2x4 = 3";
		}

		private void SolveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string[] equations = new string[]
				{
					Equation1TextBox.Text,
					Equation2TextBox.Text,
					Equation3TextBox.Text,
					Equation4TextBox.Text
				};

				currentMatrix = new double[equations.Length, equations.Length + 1];
				List<SolutionStep> steps = new List<SolutionStep>();

				for (int i = 0; i < equations.Length; i++)
				{
					ParseEquation(equations[i], currentMatrix, i);
				}

				steps.Add(new SolutionStep { Step = "Исходная матрица:", Result = MatrixToString(currentMatrix) });

				GaussianElimination(currentMatrix, steps);

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
				{
					coeff = 1;
				}
				else if (coeffStr == "-")
				{
					coeff = -1;
				}
				else
				{
					coeff = Convert.ToDouble(coeffStr);
				}

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

		private void GaussianElimination(double[,] matrix, List<SolutionStep> steps)
		{
			int n = matrix.GetLength(0);

			for (int i = 0; i < n; i++)
			{
				double maxElement = Math.Abs(matrix[i, i]);
				int maxRow = i;
				for (int k = i + 1; k < n; k++)
				{
					if (Math.Abs(matrix[k, i]) > maxElement)
					{
						maxElement = Math.Abs(matrix[k, i]);
						maxRow = k;
					}
				}

				for (int k = i; k < matrix.GetLength(1); k++)
				{
					double temp = matrix[maxRow, k];
					matrix[maxRow, k] = matrix[i, k];
					matrix[i, k] = temp;
				}

				steps.Add(new SolutionStep
				{
					Step = $"Шаг {i + 1}: Переставили строку {i + 1} с строкой {maxRow + 1} для улучшения стабильности",
					Result = MatrixToString(matrix)
				});

				double pivot = matrix[i, i];
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] /= pivot;
				}

				steps.Add(new SolutionStep
				{
					Step = $"Шаг {i + 1}: Делим строку {i + 1} на {pivot}",
					Result = MatrixToString(matrix)
				});

				for (int k = i + 1; k < n; k++)
				{
					double factor = matrix[k, i];
					for (int j = 0; j < matrix.GetLength(1); j++)
					{
						matrix[k, j] -= factor * matrix[i, j];
					}

					steps.Add(new SolutionStep
					{
						Step = $"Шаг {i + 1}.{k + 1}: Вычитаем строку {i + 1}, умноженную на {factor}, из строки {k + 1}",
						Result = MatrixToString(matrix)
					});
				}
			}
		}

		private void BackSubstitution(double[,] matrix, List<SolutionStep> steps)
		{
			int n = matrix.GetLength(0);
			double[] results = new double[n];

			for (int i = n - 1; i >= 0; i--)
			{
				double sum = matrix[i, n];
				for (int j = i + 1; j < n; j++)
				{
					sum -= matrix[i, j] * results[j];
				}
				results[i] = sum / matrix[i, i];
				steps.Add(new SolutionStep
				{
					Step = $"Результат уравнения {i + 1}: x{(i + 1)} = {(results[i] % 1 == 0 ? (int)results[i] : results[i])}",
					Result = FormatEquation(matrix, i)
				});
			}
		}

		private void SolveButton1_Click(object sender, RoutedEventArgs e)
		{
			if (currentMatrix == null)
			{
				MessageBox.Show("Сначала выполните метод Гаусса.");
				return;
			}

			List<SolutionStep> steps = new List<SolutionStep>();
			BackSubstitution(currentMatrix, steps);

			ResultDataGrid.ItemsSource = steps;
		}

		private string FormatEquation(double[,] matrix, int row)
		{
			int n = matrix.GetLength(1) - 1;
			StringBuilder equation = new StringBuilder();

			for (int j = 0; j < n; j++)
			{
				if (matrix[row, j] != 0)
				{
					if (equation.Length > 0)
					{
						equation.Append(matrix[row, j] > 0 ? " + " : " - ");
					}
					else if (matrix[row, j] < 0)
					{
						equation.Append(" - ");
					}

					double absValue = Math.Abs(matrix[row, j]);
					equation.Append($"{(absValue % 1 == 0 ? (int)absValue : absValue)}x{j + 1}");
				}
			}

			equation.Append($" = {(matrix[row, n] % 1 == 0 ? (int)matrix[row, n] : matrix[row, n])}");
			return equation.ToString();
		}

		private string MatrixToString(double[,] matrix)
		{
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					result.Append(matrix[i, j].ToString("F2") + "\t");
				}
				result.AppendLine();
			}
			return result.ToString();
		}
	}

	public class SolutionStep
	{
		public string Step { get; set; }
		public string Result { get; set; }
	}
}
