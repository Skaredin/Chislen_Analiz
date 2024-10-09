using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chislen_Analiz
{
	public partial class Chapter_3 : Window
	{
		public Chapter_3()
		{
			InitializeComponent();
			PointsInput.Text= "x,0,2,3; y,2,0,4";
		}

		private void CalculateLagrange_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var (xValues, yValues) = ParseInput(PointsInput.Text);
				List<CalculationStep> steps;
				double result = LagrangeInterpolation.Interpolate(xValues, yValues, 2, out steps);  // Пример с x = 2
				LagrangeResult.Text = result.ToString();

				// Заполняем таблицу шагами
				StepsDataGrid.ItemsSource = steps;

				// Отобразить график для Лагранжа
				PlotInterpolation(xValues, yValues, (xVals, yVals, x) => LagrangeInterpolation.Interpolate(xVals, yVals, x, out steps), "Полином Лагранжа", OxyColors.Blue);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private void CalculateNewton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var (xValues, yValues) = ParseInput(PointsInput.Text);
				List<CalculationStep> steps;
				double result = NewtonInterpolation.Interpolate(xValues, yValues, 2, out steps);  // Пример с x = 2
				NewtonResult.Text = result.ToString();

				// Заполняем таблицу шагами
				StepsDataGrid.ItemsSource = steps;

				// Отобразить график для Ньютона
				PlotInterpolation(xValues, yValues, (xVals, yVals, x) => NewtonInterpolation.Interpolate(xVals, yVals, x, out steps), "Полином Ньютона", OxyColors.Red);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private (List<double>, List<double>) ParseInput(string input)
		{
			var xValues = new List<double>();
			var yValues = new List<double>();

			var parts = input.Split(';');
			if (parts.Length != 2)
				throw new FormatException("Введите данные в формате: x,1,3,4 ; y,1,2,1");

			var xPart = parts[0].Trim();
			var yPart = parts[1].Trim();

			// Парсим x значения
			if (xPart.StartsWith("x,"))
			{
				var xData = xPart.Substring(2).Split(',');
				foreach (var x in xData)
				{
					xValues.Add(double.Parse(x));
				}
			}
			else
			{
				throw new FormatException("Первая часть должна начинаться с 'x,'");
			}

			// Парсим y значения
			if (yPart.StartsWith("y,"))
			{
				var yData = yPart.Substring(2).Split(',');
				foreach (var y in yData)
				{
					yValues.Add(double.Parse(y));
				}
			}
			else
			{
				throw new FormatException("Вторая часть должна начинаться с 'y,'");
			}

			if (xValues.Count != yValues.Count)
				throw new FormatException("Количество значений x и y должно совпадать.");

			return (xValues, yValues);
		}

		private void PlotInterpolation(List<double> xValues, List<double> yValues, Func<List<double>, List<double>, double, double> interpolate, string title, OxyColor lineColor)
		{
			var plotModel = new PlotModel { Title = title };

			// Добавляем точки
			var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };
			for (int i = 0; i < xValues.Count; i++)
			{
				scatterSeries.Points.Add(new ScatterPoint(xValues[i], yValues[i]));
			}
			plotModel.Series.Add(scatterSeries);

			// Добавляем интерполяционную кривую
			var lineSeries = new LineSeries { Color = lineColor };
			double step = (xValues[xValues.Count - 1] - xValues[0]) / 100.0;
			for (double x = xValues[0]; x <= xValues[xValues.Count - 1]; x += step)
			{
				lineSeries.Points.Add(new DataPoint(x, interpolate(xValues, yValues, x)));
			}
			plotModel.Series.Add(lineSeries);

			PlotView.Model = plotModel;
		}
	}

	public static class LagrangeInterpolation
	{
		public static double Interpolate(List<double> xValues, List<double> yValues, double x, out List<CalculationStep> steps)
		{
			steps = new List<CalculationStep>(); // Заполните шаги
			double result = 0;

			for (int i = 0; i < xValues.Count; i++)
			{
				double term = yValues[i];
				string stepDescription = $"L({i}) = {yValues[i]}";
				for (int j = 0; j < xValues.Count; j++)
				{
					if (i != j)
					{
						term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
						stepDescription += $" * (({x} - {xValues[j]}) / ({xValues[i]} - {xValues[j]}))";
					}
				}
				result += term;
				stepDescription += $" = {term}";
				steps.Add(new CalculationStep { Step = stepDescription, Result = term });
			}
			return result;
		}
	}

	public static class NewtonInterpolation
	{
		public static double Interpolate(List<double> xValues, List<double> yValues, double x, out List<CalculationStep> steps)
		{
			steps = new List<CalculationStep>();
			int n = xValues.Count;
			double[] dividedDifferences = new double[n];

			// Заполнение массива значениями y
			for (int i = 0; i < n; i++)
			{
				dividedDifferences[i] = yValues[i];
			}

			// Вычисление разделенных разностей
			for (int j = 1; j < n; j++)
			{
				for (int i = n - 1; i >= j; i--)
				{
					dividedDifferences[i] = (dividedDifferences[i] - dividedDifferences[i - 1]) / (xValues[i] - xValues[i - j]);
					// Сохраняем шаги вычисления
					steps.Add(new CalculationStep
					{
						Step = $"f[{i},{j}] = (f[{i}] - f[{i - 1}]) / ({xValues[i]} - {xValues[i - j]})",
						Result = dividedDifferences[i]
					});
				}
			}

			// Вычисление значения интерполяционного многочлена
			double result = dividedDifferences[0];
			double term = 1;

			for (int i = 1; i < n; i++)
			{
				term *= (x - xValues[i - 1]);
				result += dividedDifferences[i] * term;

				// Сохраняем шаги вычисления
				steps.Add(new CalculationStep
				{
					Step = $"P({x}) += f[{i}] * (x - x[{i - 1}])",
					Result = result
				});
			}

			return result;
		}
	}

	public class CalculationStep
	{
		public string Step { get; set; }
		public double Result { get; set; }
	}
}
