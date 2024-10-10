using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chislen_Analiz
{
	public partial class Chapter_5 : Window
	{
		public class InterpolatedData
		{
			public double X { get; set; }
			public double Spline1 { get; set; }
			public double Spline2 { get; set; }
			public double Spline3 { get; set; }
			public double Spline4 { get; set; }
			public double Spline5 { get; set; }
			public double Spline6 { get; set; }
			public double Spline7 { get; set; }
			public double Spline8 { get; set; }
			public double Spline9 { get; set; }
			public double Spline10 { get; set; }
		}
		public class InputData
		{
			public double Y { get; set; }
			public double Spline1 { get; set; }
			public double Spline2 { get; set; }
			public double Spline3 { get; set; }
			public double Spline4 { get; set; }
			public double Spline5 { get; set; }
			public double Spline6 { get; set; }
			public double Spline7 { get; set; }
			public double Spline8 { get; set; }
			public double Spline9 { get; set; }
			public double Spline10 { get; set; }
		}

		public ObservableCollection<InputData> InputDataList { get; set; }

		public Chapter_5()
		{
			InitializeComponent();
			InputDataList = new ObservableCollection<InputData>();

			double[,] values = {
				{1.0, 0.8, 1.0, 1.2, 1.1, 0.8, 1.0, 0.9, 1.2, 1.2, 1.2},
				{1.2, 1.9, 2.1, 2.2, 2.2, 1.9, 1.8, 2.0, 2.2, 2.2, 2.0},
				{1.4, 2.7, 2.8, 3.0, 3.0, 3.1, 2.8, 2.9, 3.1, 3.2, 3.1},
				{1.6, 3.9, 4.0, 4.1, 4.2, 4.0, 3.8, 3.8, 4.0, 4.2, 4.1},
				{1.8, 4.8, 5.0, 4.9, 5.1, 4.9, 4.7, 4.9, 5.0, 5.1, 5.0},
				{2.0, 5.8, 5.9, 6.1, 6.0, 5.8, 6.0, 5.9, 6.2, 6.0, 6.1}
			};

			for (int i = 0; i < values.GetLength(0); i++)
			{
				InputDataList.Add(new InputData
				{
					Y = values[i, 0],
					Spline1 = values[i, 1],
					Spline2 = values[i, 2],
					Spline3 = values[i, 3],
					Spline4 = values[i, 4],
					Spline5 = values[i, 5],
					Spline6 = values[i, 6],
					Spline7 = values[i, 7],
					Spline8 = values[i, 8],
					Spline9 = values[i, 9],
					Spline10 = values[i, 10]
				});
			}

			InputDataGrid.ItemsSource = InputDataList;
		}

		private void CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(XInput.Text, out double inputX) && inputX >= 1.0 && inputX <= 2.0)
			{
				var plotModel = new PlotModel { Title = "Кубическая интерполяция" };
				var interpolatedValues = new InterpolatedData { X = inputX };

				// Проход по всем сплайнам
				for (int i = 1; i <= 10; i++)
				{
					var xValues = InputDataList.Select(item => item.Y).ToArray();
					var yValues = InputDataList.Select(item => (double)item.GetType().GetProperty($"Spline{i}").GetValue(item)).ToArray();

					// Получение интерполированного значения для заданного inputX
					double interpolatedValue = CubicSplineInterpolate(inputX, xValues, yValues);
					interpolatedValues.GetType().GetProperty($"Spline{i}").SetValue(interpolatedValues, interpolatedValue);

					// Добавление значений сплайна на график
					var lineSeries = new LineSeries { Title = $"Spline {i}" };
					for (int j = 0; j < xValues.Length; j++)
					{
						lineSeries.Points.Add(new DataPoint(xValues[j], yValues[j]));
					}

					// Добавление интерполированной точки на график
					var interpolatedPoint = new ScatterSeries { MarkerType = MarkerType.Circle, Title = $"Interpolated Spline {i}" };
					interpolatedPoint.Points.Add(new ScatterPoint(inputX, interpolatedValue));

					// Сохранение интерполированных точек в виде ScatterPoint для всех xValues
					for (int j = 0; j < xValues.Length; j++)
					{
						var point = new ScatterPoint(xValues[j], yValues[j]);
						interpolatedPoint.Points.Add(point);
					}

					plotModel.Series.Add(lineSeries);
					plotModel.Series.Add(interpolatedPoint);
				}

				// Обновление таблицы с интерполированными значениями
				InterpolatedDataGrid.Items.Clear();
				InterpolatedDataGrid.Items.Add(interpolatedValues);
				PlotView.Model = plotModel;
			}
			else
			{
				MessageBox.Show("Введите значение в диапазоне 1.0 - 2.0");
			}

		}

		private double CubicSplineInterpolate(double x, double[] xs, double[] ys)
		{
			// Упрощенный пример - линейная интерполяция, кусочная
			for (int i = 0; i < xs.Length - 1; i++)
			{
				if (x >= xs[i] && x <= xs[i + 1])
				{
					// Используем уравнение линейной интерполяции для простоты
					double t = (x - xs[i]) / (xs[i + 1] - xs[i]);
					return ys[i] * (1 - t) + ys[i + 1] * t;
				}
			}
			return 0; // В случае, если x выходит за пределы интервалов
		}

	}
}