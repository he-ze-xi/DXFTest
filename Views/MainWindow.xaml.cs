using Microsoft.Win32;
using netDxf;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TestDXF.Tool;

namespace TestDXF.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	//public partial class MainWindow : UserControl
	//{
	//	public DXF2WPF myDXF = new DXF2WPF();
	//	ScrollViewer scrollViewer = new ScrollViewer();
	//	StackPanel stack1 = new StackPanel();
	//	DataGrid dataGrid_Line = new DataGrid();
	//	GroupBox groupBox_Circle = new GroupBox();
	//	DataGrid dataGrid_Circle = new DataGrid();
	//	GroupBox groupBox_Arc = new GroupBox();
	//	DataGrid dataGrid_Arc = new DataGrid();

	//	GroupBox groupBox_Line = new GroupBox();
	//	public MainWindow()
	//	{
	//		InitializeComponent();

	//		myDXF.GridHeight = 5000;
	//		myDXF.GridWidth = 5000;
	//		myDXF.ViewHeight = grid1.ActualHeight;
	//		myDXF.ViewWidth = grid1.ActualWidth;
	//		myDXF.WinHeight = this.Height;
	//		myDXF.WinWidth = this.Width;

	//		myDXF.border.Reset(myDXF.GridHeight, myDXF.GridWidth, true, this.Height, this.Width, this.Height, this.Width);
	//		DrawDXF(@"test8.dxf");


	//		Button resetBtn = new Button();
	//		resetBtn.Width = 100;
	//		resetBtn.VerticalAlignment = VerticalAlignment.Top;
	//		resetBtn.HorizontalAlignment = HorizontalAlignment.Left;
	//		resetBtn.Content = "重置";
	//		resetBtn.Margin = new Thickness(5);
	//		resetBtn.Click += ResetDXFCmd_Click;
	//		StackPanel stack = new StackPanel();
	//		stack.Children.Add(resetBtn);


	//		Button openBtn = new Button();
	//		openBtn.Width = 100;
	//		openBtn.VerticalAlignment = VerticalAlignment.Top;
	//		openBtn.HorizontalAlignment = HorizontalAlignment.Left;
	//		openBtn.Content = "打开";
	//		openBtn.Margin = new Thickness(5);
	//		openBtn.Click += OpenDXFCmd_Click;
	//		stack.Children.Add(openBtn);

	//		Button ZoomAutoBtn = new Button();
	//		ZoomAutoBtn.Width = 100;
	//		ZoomAutoBtn.VerticalAlignment = VerticalAlignment.Top;
	//		ZoomAutoBtn.HorizontalAlignment = HorizontalAlignment.Left;
	//		ZoomAutoBtn.Content = "对齐";
	//		ZoomAutoBtn.Margin = new Thickness(5);
	//		ZoomAutoBtn.Click += ZommCenterDXFCmd_Click;

	//		stack.Children.Add(ZoomAutoBtn);


	//		myDXF.mainGrid.Children.Add(stack);

	//		scrollViewer.Background = new SolidColorBrush(Colors.White);
	//		scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
	//		scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
	//		scrollViewer.Width = (this.Width)/3;
	//		scrollViewer.Height = (this.Height/5)*4;
	//		scrollViewer.VerticalAlignment = VerticalAlignment.Top;
	//		scrollViewer.HorizontalAlignment = HorizontalAlignment.Right;
	//		groupBox_Line.Content = dataGrid_Line;
	//		stack1.Background = new SolidColorBrush(Colors.White);
	//		stack1.Children.Add(groupBox_Line);
	//		groupBox_Circle.Content = dataGrid_Circle;
	//		stack1.Children.Add(groupBox_Circle);
	//		groupBox_Arc.Content = dataGrid_Arc;
	//		stack1.Children.Add(groupBox_Arc);
	//		scrollViewer.Content = stack1;
	//		myDXF.mainGrid.Children.Add(scrollViewer);


			
	//	}


	//	private void OpenDXFCmd_Click(object sender, RoutedEventArgs e)
	//	{
	//		OpenFileDialog openFileDialog = new OpenFileDialog();
	//		if (openFileDialog.ShowDialog() == true)
	//		{
	//			String fileDXF = openFileDialog.FileName;
	//			DrawDXF(fileDXF);
	//			InitData();
	//			myDXF.border.ZoomAuto(5000, 5000, mainWin.myDXF.WinHeight, mainWin.myDXF.WinWidth);

	//		}
	//	}

	//	private void ResetDXFCmd_Click(object sender, RoutedEventArgs e)
	//	{
	//		myDXF.border.Zoom(myDXF.GridHeight, myDXF.GridWidth, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth, 0, 0, 1);

	//	}

	//	private void ZommCenterDXFCmd_Click(object sender, RoutedEventArgs e)
	//	{
	//		myDXF.border.ZoomAuto(5000, 5000, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth);

	//	}

	//	private void DrawDXF(string fileDXF)
	//	{
	//		TypeConverter.defaultThickness = 0.01;
	//		myDXF.DxfDoc = new DxfDocument();

	//		if (fileDXF == "")
	//		{
	//			MessageBox.Show("请选择一个DXF文件！");
	//			return;
	//		}
	//		this.Content = myDXF.GetMainGrid(fileDXF, true, true);

	//		myDXF.border.ZoomAuto(5000, 5000, mainWin.myDXF.WinHeight, mainWin.myDXF.WinWidth);

			



	//	}

	//	public void InitData()
	//	{


	//		//直线
	//		groupBox_Line.Header = "直线";
	//		dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
	//		dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "起点X1", Binding = new Binding("fX1Pos") });
	//		dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "端点Y1", Binding = new Binding("fY1Pos") });
	//		dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "起点X2", Binding = new Binding("fX2Pos") });
	//		dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "端点Y2", Binding = new Binding("fY2Pos") });
	//		foreach(var item in DXFDataContainer.DataLineItems)
	//		{
	//			this.dataGrid_Line.Items.Add(item);

	//		}


	//		//圆形
	//		groupBox_Circle.Header = "圆形";
	//		dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
	//		dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "圆心X", Binding = new Binding("fXPos") });
	//		dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "圆心Y", Binding = new Binding("fYPos") });
	//		dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "半径R", Binding = new Binding("fRPos") });
	//		foreach (var item in DXFDataContainer.DataCircleItems)
	//		{
	//			this.dataGrid_Circle.Items.Add(item);

	//		}

	//		//圆弧
	//		groupBox_Arc.Header = "圆弧";
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "圆心X", Binding = new Binding("fXPos") });
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "圆心Y", Binding = new Binding("fYPos") });
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "半径R", Binding = new Binding("fRPos") });
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "起始角度", Binding = new Binding("fStartAngle") });
	//		dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "终点角度", Binding = new Binding("fEndAngle") });
	//		foreach (var item in DXFDataContainer.DataArcItems)
	//		{
	//			this.dataGrid_Arc.Items.Add(item);

	//		}
	//	}
	//}

	public partial class MainWindow : Window
	{
		public DXF2WPF myDXF = new DXF2WPF();
		ScrollViewer scrollViewer = new ScrollViewer();
		StackPanel stack1 = new StackPanel();
		DataGrid dataGrid_Line = new DataGrid();
		GroupBox groupBox_Circle = new GroupBox();
		DataGrid dataGrid_Circle = new DataGrid();
		GroupBox groupBox_Arc = new GroupBox();
		DataGrid dataGrid_Arc = new DataGrid();

		GroupBox groupBox_Line = new GroupBox();
		public MainWindow()
		{
			InitializeComponent();

			myDXF.GridHeight = 5000;
			myDXF.GridWidth = 5000;
			myDXF.ViewHeight = grid1.ActualHeight;
			myDXF.ViewWidth = grid1.ActualWidth;
			myDXF.WinHeight = this.Height;
			myDXF.WinWidth = this.Width;

			myDXF.border.Reset(myDXF.GridHeight, myDXF.GridWidth, true, this.Height, this.Width, this.Height, this.Width);
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test8.dxf");
			DrawDXF(path);


			Button resetBtn = new Button();
			resetBtn.Width = 100;
			resetBtn.VerticalAlignment = VerticalAlignment.Top;
			resetBtn.HorizontalAlignment = HorizontalAlignment.Left;
			resetBtn.Content = "重置";
			resetBtn.Margin = new Thickness(5);
			resetBtn.Click += ResetDXFCmd_Click;
			StackPanel stack = new StackPanel();
			stack.Children.Add(resetBtn);


			Button openBtn = new Button();
			openBtn.Width = 100;
			openBtn.VerticalAlignment = VerticalAlignment.Top;
			openBtn.HorizontalAlignment = HorizontalAlignment.Left;
			openBtn.Content = "打开";
			openBtn.Margin = new Thickness(5);
			openBtn.Click += OpenDXFCmd_Click;
			stack.Children.Add(openBtn);

			Button ZoomAutoBtn = new Button();
			ZoomAutoBtn.Width = 100;
			ZoomAutoBtn.VerticalAlignment = VerticalAlignment.Top;
			ZoomAutoBtn.HorizontalAlignment = HorizontalAlignment.Left;
			ZoomAutoBtn.Content = "对齐";
			ZoomAutoBtn.Margin = new Thickness(5);
			ZoomAutoBtn.Click += ZommCenterDXFCmd_Click;

			stack.Children.Add(ZoomAutoBtn);


			myDXF.mainGrid.Children.Add(stack);

			scrollViewer.Background = new SolidColorBrush(Colors.White);
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
			scrollViewer.Width = 450;
			scrollViewer.Height = 800;
			scrollViewer.VerticalAlignment = VerticalAlignment.Top;
			scrollViewer.HorizontalAlignment = HorizontalAlignment.Right;
			//直线
			groupBox_Line.Header = "直线";
			dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
			dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "起点X1", Binding = new Binding("fX1Pos") });
			dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "端点Y1", Binding = new Binding("fY1Pos") });
			dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "起点X2", Binding = new Binding("fX2Pos") });
			dataGrid_Line.Columns.Add(new DataGridTextColumn() { Header = "端点Y2", Binding = new Binding("fY2Pos") });
			groupBox_Line.Content = dataGrid_Line;
			stack1.Background = new SolidColorBrush(Colors.White);
			stack1.Children.Add(groupBox_Line);
			//圆形
			groupBox_Circle.Header = "圆形";
			dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
			dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "圆心X", Binding = new Binding("fXPos") });
			dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "圆心Y", Binding = new Binding("fYPos") });
			dataGrid_Circle.Columns.Add(new DataGridTextColumn() { Header = "半径R", Binding = new Binding("fRPos") });
			groupBox_Circle.Content = dataGrid_Circle;
			stack1.Children.Add(groupBox_Circle);
			//圆弧
			groupBox_Arc.Header = "圆弧";
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "编号", Binding = new Binding("Number") });
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "圆心X", Binding = new Binding("fXPos") });
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "圆心Y", Binding = new Binding("fYPos") });
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "半径R", Binding = new Binding("fRPos") });
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "起始角度", Binding = new Binding("fStartAngle") });
			dataGrid_Arc.Columns.Add(new DataGridTextColumn() { Header = "终点角度", Binding = new Binding("fEndAngle") });
			groupBox_Arc.Content = dataGrid_Arc;
			stack1.Children.Add(groupBox_Arc);

			scrollViewer.Content = stack1;
			myDXF.mainGrid.Children.Add(scrollViewer);

			myDXF.border.Zoom(myDXF.GridHeight, myDXF.GridWidth, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth, 0, 0, 1);


		}


		private void OpenDXFCmd_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				String fileDXF = openFileDialog.FileName;
				DrawDXF(fileDXF);
				InitData();
				myDXF.border.Zoom(myDXF.GridHeight, myDXF.GridWidth, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth, 0, 0, 1);

			}
		}

		private void ResetDXFCmd_Click(object sender, RoutedEventArgs e)
		{
			myDXF.border.Zoom(myDXF.GridHeight, myDXF.GridWidth, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth, 0, 0, 1);

		}

		private void ZommCenterDXFCmd_Click(object sender, RoutedEventArgs e)
		{
			myDXF.border.ZoomAuto(5000, 5000, ((Grid)Application.Current.MainWindow.Content).ActualHeight, ((Grid)Application.Current.MainWindow.Content).ActualWidth);

		}

		private void DrawDXF(string fileDXF)
		{
			try
			{
				TypeConverter.defaultThickness = 0.01;
				myDXF.DxfDoc = new DxfDocument();

				if (fileDXF == "")
				{
					MessageBox.Show("请选择一个DXF文件！");
					return;
				}
				this.Content = myDXF.GetMainGrid(fileDXF, true, true);

				myDXF.border.ZoomAuto(5000, 5000, mainWin.myDXF.WinHeight, mainWin.myDXF.WinWidth);


			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString()); return;
			}



		}

		bool isInit = false;
		public void InitData()
		{
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				this.dataGrid_Line.Items.Clear();
				this.dataGrid_Circle.Items.Clear();
				this.dataGrid_Arc.Items.Clear();

			}));
			//直线
			foreach (var item in DXFDataContainer.DataLineItems)
			{
				this.dataGrid_Line.Items.Add(item);
			}


			//圆形
			foreach (var item in DXFDataContainer.DataCircleItems)
			{
				this.dataGrid_Circle.Items.Add(item);

			}

			//圆弧
			foreach (var item in DXFDataContainer.DataArcItems)
			{
				this.dataGrid_Arc.Items.Add(item);

			}
		}
	}
}
