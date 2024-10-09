using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Логика взаимодействия для Chapter_3.xaml
	/// </summary>
	public partial class Chapter_3 : Window
	{
		public Chapter_3()
		{
			InitializeComponent();
		}
		private void CalculateLagrange_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var (xValues, yValues) = ParseInput(PointsInput.Text);
				double result = LagrangeInterpolation.Interpolate(xValues, yValues, 2);  // Пример с x = 2
				LagrangeResult.Text = result.ToString();

				// Отобразить график для Лагранжа
				PlotInterpolation(xValues, yValues, LagrangeInterpolation.Interpolate, "Полином Лагранжа");
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
				double result = NewtonInterpolation.Interpolate(xValues, yValues, 2);  // Пример с x = 2
				NewtonResult.Text = result.ToString();

				// Отобразить график для Ньютона
				PlotInterpolation(xValues, yValues, NewtonInterpolation.Interpolate, "Полином Ньютона");
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

		private void PlotInterpolation(List<double> xValues, List<double> yValues, Func<List<double>, List<double>, double, double> interpolate, string title)
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
			var lineSeries = new LineSeries();
			double step = (xValues[xValues.Count - 1] - xValues[0]) / 100.0;
			for (double x = xValues[0]; x <= xValues[xValues.Count - 1]; x += step)
			{
				lineSeries.Points.Add(new DataPoint(x, interpolate(xValues, yValues, x)));
			}
			plotModel.Series.Add(lineSeries);

			PlotView.Model = plotModel;
		}
	}
}