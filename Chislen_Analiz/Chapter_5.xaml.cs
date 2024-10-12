using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OxyPlot.Annotations;

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
			public double X1 { get; set; }
			public double X2 { get; set; }
			public double X3 { get; set; }
			public double X4 { get; set; }
			public double X5 { get; set; }
			public double X6 { get; set; }
			public double X7 { get; set; }
			public double X8 { get; set; }
			public double X9 { get; set; }
			public double X10 { get; set; }
		}

		public ObservableCollection<InputData> InputDataList { get; set; }
		public ObservableCollection<InterpolatedData> InterpolatedDataList { get; set; } // Новая коллекция для интерполированных данных

		public Chapter_5()
		{
			InitializeComponent();
			InputDataList = new ObservableCollection<InputData>();
			InterpolatedDataList = new ObservableCollection<InterpolatedData>(); // Инициализация коллекции интерполированных данных

			double[,] values = {
				 // Первая таблица
	//{1.0, 1.0, 1.1, 0.9, 0.9, 0.8, 1.1, 1.0, 1.2, 1.2, 1.1},
	//{1.2, 2.1, 2.2, 2.0, 1.9, 2.0, 2.2, 2.1, 1.8, 2.0, 1.9},
	//{1.4, 2.9, 3.2, 3.0, 3.2, 2.9, 3.2, 3.1, 3.2, 3.0, 3.2},
	//{1.6, 3.8, 4.2, 3.8, 3.8, 4.0, 4.2, 3.8, 4.1, 3.8, 3.8},
	//{1.8, 5.2, 5.2, 5.1, 5.1, 5.2, 5.1, 5.2, 5.0, 5.2, 4.9},
	//{2.0, 5.9, 6.0, 5.8, 6.1, 5.8, 5.9, 6.2, 6.1, 6.1, 5.8},
    // Вторая таблица
    {1.0, 0.8, 0.8, 1.1, 0.8, 1.0, 0.9, 1.0, 1.2, 1.2, 1.2},
	{1.2, 2.0, 2.2, 1.9, 1.8, 2.2, 2.2, 1.9, 2.0, 2.2, 2.0},
	{1.4, 2.8, 2.9, 3.0, 3.2, 2.9, 3.0, 3.1, 3.2, 3.0, 3.2},
	{1.6, 4.0, 4.2, 3.8, 4.1, 4.2, 3.8, 4.0, 4.1, 3.8, 4.0},
	{1.8, 5.1, 5.2, 5.2, 4.8, 5.2, 5.1, 5.0, 4.9, 5.2, 4.8},
	{2.0, 6.0, 5.9, 6.1, 6.1, 5.8, 6.2, 6.1, 6.0, 6.1, 5.8},
    // Третья таблица
    //{1.0, 2.8, 3.8, 4.5, 5.4, 10.0, 9.2, 0.12, 0.10, 0.12, 0.12},
	//{1.2, 3.8, 3.7, 4.9, 5.7, 11.0, 8.9, 0.22, 0.21, 0.24, 0.25},
	//{1.4, 5.4, 5.0, 3.2, 12.8, 8.0, 6.8, 0.34, 0.38, 0.30, 0.40},
	//{1.6, 8.2, 8.3, 4.8, 13.1, 13.7, 8.1, 0.48, 0.40, 0.44, 0.42},
	//{1.8, 4.0, 4.2, 4.3, 8.4, 7.2, 5.6, 0.50, 0.48, 0.50, 0.50},
	//{2.0, 6.0, 6.9, 7.1, 6.0, 8.6, 4.6, 0.60, 0.56, 0.60, 0.62}
			};

			for (int i = 0; i < values.GetLength(0); i++)
			{
				InputDataList.Add(new InputData
				{
					Y = values[i, 0],
					X1 = values[i, 1],
					X2 = values[i, 2],
					X3 = values[i, 3],
					X4 = values[i, 4],
					X5 = values[i, 5],
					X6 = values[i, 6],
					X7 = values[i, 7],
					X8 = values[i, 8],
					X9 = values[i, 9],
					X10 = values[i, 10]
				});
			}

			InputDataGrid.ItemsSource = InputDataList;
			InterpolatedDataGrid.ItemsSource = InterpolatedDataList; // Установка источника данных для таблицы интерполированных данных
		}

		private void CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(XInput.Text, out double inputX) && inputX >= 1.0 && inputX <= 2.0)
			{

				// Получаем значения X и Y
				bool isXValid = double.TryParse(XInput.Text, out double inputX1);
				bool isYValid = double.TryParse(YInput.Text, out double inputY1);




				InterpolatedDataList.Clear();

				// Проверяем, что X находится в диапазоне и определяем, был ли введен Y
				if (isXValid && inputX1 >= 1.0 && inputX1 <= 2.0)
				{
					var plotModel = new PlotModel { Title = "Кубическая интерполяция" };
					var interpolatedValues = new InterpolatedData { X = inputX1 };
					InterpolatedDataList.Clear();

					for (int i = 1; i <= 10; i++)
					{
						var xValues = InputDataList.Select(item => item.Y).ToArray();
						var yValues = InputDataList.Select(item => (double)item.GetType().GetProperty($"X{i}").GetValue(item)).ToArray();

						double interpolatedValue = CubicSplineInterpolate(inputX1, xValues, yValues);
						interpolatedValues.GetType().GetProperty($"Spline{i}").SetValue(interpolatedValues, interpolatedValue);

						var lineSeries = new LineSeries { Title = $"Spline {i}" };
						for (int j = 0; j < xValues.Length; j++)
						{
							lineSeries.Points.Add(new DataPoint(xValues[j], yValues[j]));
						}

						var interpolatedData = new InterpolatedData { X = inputX1 };
						interpolatedData.GetType().GetProperty($"Spline{i}").SetValue(interpolatedData, interpolatedValue);
						InterpolatedDataList.Add(interpolatedData);
						plotModel.Series.Add(lineSeries);
					}
					// Отображение точки на графике, если введены оба значения X и Y
					if (isYValid)
					{
						var pointAnnotation = new PointAnnotation
						{
							X = inputX1,
							Y = inputY1,
							Text = $"({inputX}; {inputY1})",
							Shape = MarkerType.Circle,

						};
						plotModel.Annotations.Add(pointAnnotation);
					}
					// Добавление точек исходных данных
					var scatterSeries = new ScatterSeries
					{
						MarkerType = MarkerType.Circle,
						MarkerSize = 6,
						Title = "Исходные данные"
					};

					for (int i = 0; i < InputDataList.Count; i++)
					{
						var inputData = InputDataList[i];
						for (int j = 1; j <= 10; j++)
						{
							var splineValue = (double)inputData.GetType().GetProperty($"X{j}").GetValue(inputData);
							scatterSeries.Points.Add(new ScatterPoint(inputData.Y, splineValue));
						}

					}
					plotModel.Series.Add(scatterSeries); // Добавляем серию точек на график

					// Подписи точек
					for (int i = 0; i < InputDataList.Count; i++)
					{
						var inputData = InputDataList[i];
						for (int j = 1; j <= 10; j++)
						{
							var splineValue = (double)inputData.GetType().GetProperty($"X{j}").GetValue(inputData);
							var textAnnotation = new TextAnnotation
							{
								Text = $"({inputData.Y}; {splineValue})",
								TextPosition = new DataPoint(inputData.Y, splineValue),
								Stroke = OxyColors.Transparent,
								TextColor = OxyColors.Black // Изменено с Fill на TextColor
							};
							plotModel.Annotations.Add(textAnnotation);
						}
					}

					plotModel.InvalidatePlot(true);
					PlotView.Model = plotModel;
				}
				else
				{
					MessageBox.Show("Введите корректное значение x в диапазоне от 1.0 до 2.0.");
				}
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