using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Chislen_Analiz
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Equation1TextBox.Text = "x1 - x2 + 2x3 - x4 =1";
			Equation2TextBox.Text = "2x +3x3 +x4 = 4";
			Equation3TextBox.Text = "x1 +x2 +3x3 -x4 = 2";
			Equation4TextBox.Text = "2x +x2 +5x3 - 2x4 = 3";
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

				double[,] matrix = new double[equations.Length, equations.Length + 1]; // +1 для правой части уравнений
				List<SolutionStep> steps = new List<SolutionStep>();

				for (int i = 0; i < equations.Length; i++)
				{
					ParseEquation(equations[i], matrix, i);
				}

				// Записываем исходную матрицу
				steps.Add(new SolutionStep { Step = "Исходная матрица:", Result = MatrixToString(matrix) });

				// Применение метода Гаусса
				GaussianElimination(matrix, steps);

				// Обновление DataGrid с шагами
				ResultDataGrid.ItemsSource = steps;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private void ParseEquation(string equation, double[,] matrix, int row)
		{
			// Обновленный шаблон для обработки коэффициентов, включая отрицательные значения
			string pattern = @"([+-]?\d*\.?\d*)?x(\d)";
			MatchCollection matches = Regex.Matches(equation.Replace(" ", ""), pattern);

			foreach (Match match in matches)
			{
				double coeff;
				string coeffStr = match.Groups[1].Value;

				// Определение коэффициента
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

			// Считываем правую часть уравнения
			string rightSidePattern = @"=\s*(-?\d+\.?\d*)";
			Match rightSideMatch = Regex.Match(equation.Replace(" ", ""), rightSidePattern);
			if (rightSideMatch.Success)
			{
				double result = Convert.ToDouble(rightSideMatch.Groups[1].Value);
				matrix[row, matrix.GetLength(1) - 1] = result; // Добавляем результат в последнюю колонку
			}
		}

		private void GaussianElimination(double[,] matrix, List<SolutionStep> steps)
		{
			int n = matrix.GetLength(0);
			for (int i = 0; i < n; i++)
			{
				// Делим строку на ведущий элемент
				double pivot = matrix[i, i];
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] /= pivot;
				}

				steps.Add(new SolutionStep
				{
					Step = $"Шаг {i + 1}: Нормализация строки {i + 1}. Делим строку на {pivot}.",
					Result = MatrixToString(matrix)
				});

				// Обнуляем остальные элементы в колонке
				for (int k = 0; k < n; k++)
				{
					if (k != i)
					{
						double factor = matrix[k, i];
						for (int j = 0; j < matrix.GetLength(1); j++)
						{
							matrix[k, j] -= factor * matrix[i, j];
						}

						steps.Add(new SolutionStep
						{
							Step = $"Шаг {i + 1}: Строки {k + 1}. Умножаем строку {i + 1} на {factor} и вычитаем из строки {k + 1}.",
							Result = MatrixToString(matrix)
						});
					}
				}
			}

			// Записываем финальную матрицу
			steps.Add(new SolutionStep { Step = "Финальная матрица:", Result = MatrixToString(matrix) });
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

	public class SolutionStep
	{
		public string Step { get; set; }
		public string Result { get; set; }
	}
}
