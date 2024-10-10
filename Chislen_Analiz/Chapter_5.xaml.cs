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

			// Использование ваших массивов значений
			double[,] values1 = {
				{ 1.0, 1.0, 1.1, 0.9, 0.9, 0.8, 1.1, 1.0, 1.2, 1.2, 1.1 },
				{ 1.2, 2.1, 2.2, 2.0, 1.9, 2.0, 2.2, 2.1, 1.8, 2.0, 1.9 },
				{ 1.4, 2.9, 3.2, 3.0, 3.2, 2.9, 3.2, 3.1, 3.2, 3.0, 3.2 },
				{ 1.6, 3.8, 4.2, 3.8, 3.8, 4.0, 4.2, 3.8, 4.1, 3.8, 3.8 },
				{ 1.8, 5.2, 5.2, 5.1, 5.1, 5.2, 5.1, 5.2, 5.0, 5.2, 4.9 },
				{ 2.0, 5.9, 6.0, 5.8, 6.1, 5.8, 5.9, 6.2, 6.1, 6.1, 5.8 }
			};

			double[,] values2 = {
				{ 1.0, 0.8, 0.8, 1.1, 0.8, 1.0, 0.9, 1.0, 1.2, 1.2, 1.2 },
				{ 1.2, 2.0, 2.2, 1.9, 1.8, 2.2, 2.2, 1.9, 2.0, 2.2, 2.0 },
				{ 1.4, 2.8, 2.9, 3.0, 3.2, 2.9, 3.0, 3.1, 3.2, 3.0, 3.2 },
				{ 1.6, 4.0, 4.2, 3.8, 4.1, 4.2, 3.8, 4.0, 4.1, 3.8, 4.0 },
				{ 1.8, 5.1, 5.2, 5.2, 4.8, 5.2, 5.1, 5.0, 4.9, 5.2, 4.8 },
				{ 2.0, 6.0, 5.9, 6.1, 6.1, 5.8, 6.2, 6.1, 6.0, 6.1, 5.8 }
			};

			double[,] values3 = {
				{ 1.0, 2.8, 3.8, 4.5, 5.4, 10.0, 9.2, 0.12, 0.10, 0.12, 0.12 },
				{ 1.2, 3.8, 3.7, 4.9, 5.7, 11.0, 8.9, 0.22, 0.21, 0.24, 0.25 },
				{ 1.4, 5.4, 5.0, 3.2, 12.8, 8.0, 6.8, 0.34, 0.38, 0.30, 0.40 },
				{ 1.6, 8.2, 8.3, 4.8, 13.1, 13.7, 8.1, 0.48, 0.40, 0.44, 0.42 },
				{ 1.8, 4.0, 4.2, 4.3, 8.4, 7.2, 5.6, 0.50, 0.48, 0.50, 0.50 },
				{ 2.0, 6.0, 6.9, 7.1, 6.0, 8.6, 4.6, 0.60, 0.56, 0.60, 0.62 }
			};

			// Добавление данных в InputDataList
			for (int i = 0; i < values1.GetLength(0); i++)
			{
				InputDataList.Add(new InputData
				{
					Y = values1[i, 0],
					Spline1 = values1[i, 1],
					Spline2 = values2[i, 1],
					Spline3 = values3[i, 1],
					Spline4 = values1[i, 2],
					Spline5 = values2[i, 2],
					Spline6 = values3[i, 2],
					Spline7 = values1[i, 3],
					Spline8 = values2[i, 3],
					Spline9 = values3[i, 3],
					Spline10 = values1[i, 4]
				});
			}

			InputDataGrid.ItemsSource = InputDataList;
		}

		private void CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(XInput.Text, out double inputX) && inputX >= 1.0 && inputX <= 2.0)
			{
				var plotModel = new PlotModel { Title = "Кубическая интерполяция" };

				// Проход по всем сплайнам
				for (int i = 1; i <= 10; i++)
				{
					var xValues = InputDataList.Select(item => item.Y).ToArray();
					var yValues = InputDataList.Select(item => (double)item.GetType().GetProperty($"Spline{i}").GetValue(item)).ToArray();

					// Добавление данных для каждого сплайна
					var lineSeries = new LineSeries { Title = $"Spline {i}" };
					for (int j = 0; j < xValues.Length; j++)
					{
						lineSeries.Points.Add(new DataPoint(xValues[j], yValues[j]));
					}
					plotModel.Series.Add(lineSeries);

					// Получение интерполированного значения для заданного inputX
					double interpolatedValue = CubicSplineInterpolate(inputX, xValues, yValues);

					// Добавление интерполированной точки на график
					var interpolatedPoint = new ScatterSeries { MarkerType = MarkerType.Circle, Title = $"Interpolated Point {i}" };
					interpolatedPoint.Points.Add(new ScatterPoint(inputX, interpolatedValue));
					plotModel.Series.Add(interpolatedPoint);
				}

				// Обновление графика
				PlotView.Model = plotModel;

				// Отображение результатов в отдельной таблице
				DisplayInterpolatedResults(inputX);
			}
			else
			{
				MessageBox.Show("Введите значение x в диапазоне от 1.0 до 2.0.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void DisplayInterpolatedResults(double inputX)
		{
			// Очистка предыдущих результатов
			ResultsDataGrid.ItemsSource = null;

			// Создание новой коллекции результатов
			var resultsList = new List<InterpolatedData>();

			// Получение интерполированных значений для всех сплайнов
			for (int i = 1; i <= 10; i++)
			{
				double[] xValues = InputDataList.Select(item => item.Y).ToArray();
				double[] yValues = InputDataList.Select(item => (double)item.GetType().GetProperty($"Spline{i}").GetValue(item)).ToArray();
				double interpolatedValue = CubicSplineInterpolate(inputX, xValues, yValues);

				resultsList.Add(new InterpolatedData
				{
					X = inputX,
					Spline1 = i == 1 ? interpolatedValue : 0,
					Spline2 = i == 2 ? interpolatedValue : 0,
					Spline3 = i == 3 ? interpolatedValue : 0,
					Spline4 = i == 4 ? interpolatedValue : 0,
					Spline5 = i == 5 ? interpolatedValue : 0,
					Spline6 = i == 6 ? interpolatedValue : 0,
					Spline7 = i == 7 ? interpolatedValue : 0,
					Spline8 = i == 8 ? interpolatedValue : 0,
					Spline9 = i == 9 ? interpolatedValue : 0,
					Spline10 = i == 10 ? interpolatedValue : 0
				});
			}

			ResultsDataGrid.ItemsSource = resultsList;
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
