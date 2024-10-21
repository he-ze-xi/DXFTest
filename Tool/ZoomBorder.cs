﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;

namespace TestDXF.Tool
{
	public class ZoomBorder : Border
	{
		private UIElement _child = null;
		private Point _origin;
		private Point _start;

		private TranslateTransform GetTranslateTransform(UIElement element)
		{
			return (TranslateTransform)((TransformGroup)element.RenderTransform)
				.Children.First(tr => tr is TranslateTransform);
		}

		private ScaleTransform GetScaleTransform(UIElement element)
		{
			return (ScaleTransform)((TransformGroup)element.RenderTransform)
				.Children.First(tr => tr is ScaleTransform);
		}
		public override UIElement Child
		{
			get { return base.Child; }
			set
			{
				if (value != null && value != this.Child)
				{
					this.Initialize(value);
				}
				base.Child = value;
			}
		}

		public void Initialize(UIElement element)
		{
			_child = element;
			if (_child != null)
			{
				var group = new TransformGroup();
				var st = new ScaleTransform();
				group.Children.Add(st);
				var tt = new TranslateTransform();
				group.Children.Add(tt);
				_child.RenderTransform = group;
				_child.RenderTransformOrigin = new Point(0.0, 0.0);
				this.MouseWheel += Child_MouseWheel;
				this.MouseLeftButtonDown += Child_MouseLeftButtonDown;
				this.MouseLeftButtonUp += Child_MouseLeftButtonUp;
				this.MouseMove += Child_MouseMove;
				this.PreviewMouseRightButtonDown += Child_PreviewMouseRightButtonDown;
			}
		}

		public void Reset(double gridHeight, double gridWidth, bool init, double ViewHeight, double ViewWidth, double WinHeight, double WinWidth)
		{
			if (_child != null)
			{
				double hauteurVu;
				double largeurVu;
				double scaleY;
				double scaleX;
				double scale;

				DrawEntities.CalcMaxDimDoc();




				if (init == true)
				{

					hauteurVu = ViewHeight;
					largeurVu = ViewWidth;

					if (hauteurVu == 0.0) hauteurVu = 462;
					if (largeurVu == 0.0) largeurVu = 500;
				}
				else
				{
					hauteurVu = DrawEntities.dimDoc.maxY - DrawEntities.dimDoc.minY;
					largeurVu = DrawEntities.dimDoc.maxX - DrawEntities.dimDoc.minX;
				}

				scaleY = ViewHeight / hauteurVu;
				scaleX = ViewWidth / largeurVu;
				scale = Math.Min(scaleY, scaleX);
				if (scale == 0)
				{
					scale = 1.0;

				}
				var st = GetScaleTransform(_child);

				st.ScaleX = scale;
				st.ScaleY = scale;
				var tt = GetTranslateTransform(_child);

				if (WinHeight == 0) WinHeight = ViewHeight;
				if (WinWidth == 0) WinWidth = ViewWidth;

				if (init == true)
				{
					tt.X = -gridWidth * scale / 2;
					tt.Y = -gridHeight * scale / 2;
				}
				else
				{
					tt.X = -gridWidth * scale / 2 - DrawEntities.dimDoc.minX * scale;
					tt.Y = -gridHeight * scale / 2 - (WinHeight * scale) / 2 + hauteurVu * scale + DrawEntities.dimDoc.minY * scale;
				}

			}
		}



		public void Reset()
		{
			Reset(this.Height, this.Width, true, 250, 250, 250, 250);
		}


		public void ZoomAuto(double gridHeight, double gridWidth, double WinHeight, double WinWidth)
		{
			if (_child != null)
			{
				double hauteurVu;
				double largeurVu;
				double hauteurUtil;
				double largeurUtil;
				double scaleY;
				double scaleX;
				double scale;

				DrawEntities.CalcMaxDimDoc();
				if (DrawEntities.dimDoc.maxY == double.MinValue || DrawEntities.dimDoc.maxX == double.MinValue) return;
				hauteurVu = Math.Abs(DrawEntities.dimDoc.maxY - DrawEntities.dimDoc.minY);
				largeurVu = Math.Abs(DrawEntities.dimDoc.maxX - DrawEntities.dimDoc.minX);

				scaleY = (WinHeight) / (hauteurVu);
				scaleX = WinWidth / (largeurVu);

				scale = Math.Min(scaleY, scaleX);
				if (scale == 0)
				{
					scale = 1.0;
					hauteurVu = 462;
				}

				hauteurUtil = WinHeight - hauteurVu * scale;
				largeurUtil = WinWidth - largeurVu * scale;

				var tt = GetTranslateTransform(_child);

				tt.X = -gridWidth * scale / 2 - DrawEntities.dimDoc.minX * scale;
				tt.Y = -(gridHeight * scale) / 2 - 250 * scale + (WinHeight) + DrawEntities.dimDoc.minY * scale;

				var st = GetScaleTransform(_child);

				st.ScaleX = Math.Abs(scale);
				st.ScaleY = Math.Abs(scale);

			}
		}

		public Point CurrentPosition(double gridHeight, double gridWidth, double WinHeight, double WinWidth)
		{
			var st = GetScaleTransform(_child);
			var tt = GetTranslateTransform(_child);
			Point current = new Point();


			current.X = ((((-gridWidth * st.ScaleX * 0.5) - tt.X) * st.ScaleX + (WinWidth * st.ScaleX * 0.5)) / st.ScaleX) / st.ScaleX;
			current.Y = ((tt.Y + (gridHeight * st.ScaleY) / 2 + 250 * st.ScaleY - WinHeight) + (((WinHeight * st.ScaleY) / 2) / st.ScaleY)) / st.ScaleY;

			return current;
		}

		public double CurrentZoom()
		{
			var st = GetScaleTransform(_child);

			return st.ScaleX;
		}

		public void Zoom(double gridHeight, double gridWidth, double WinHeight, double WinWidth, double scale)
		{
			Zoom(gridHeight, gridWidth, WinHeight, WinWidth, CurrentPosition(gridHeight, gridWidth, WinHeight, WinWidth), scale);

		}

		public void Zoom(double gridHeight, double gridWidth, double WinHeight, double WinWidth, Point currentPos, double scale)
		{
			Zoom(gridHeight, gridWidth, WinHeight, WinWidth, currentPos.X, currentPos.Y, scale);

		}

		public void Zoom(double gridHeight, double gridWidth, double WinHeight, double WinWidth, double currentX, double currentY, double scale)
		{
			if (_child != null)
			{

				double hauteurVu;
				double largeurVu;

				var st = GetScaleTransform(_child);
				var tt = GetTranslateTransform(_child);

				hauteurVu = WinHeight;
				largeurVu = WinWidth;


				tt.X = (-gridWidth * scale * 0.5) - (currentX * scale - (WinWidth * scale * 0.5)) / scale;
				tt.Y = (-(gridHeight * scale) / 2 - 250 * scale + WinHeight) + (currentY * scale - (WinHeight * scale) / 2) / scale;
				tt.Y = (-(gridHeight * scale) / 2 - 250 * scale + WinHeight) + (currentY * scale * scale - (WinHeight * scale) / 2) / scale;
				tt.X = -gridWidth * scale * 0.5 - (currentX * scale * scale - (WinWidth * scale * 0.5)) / scale;

				st.ScaleX = scale;
				st.ScaleY = scale;

			}
		}




		private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (_child != null)
			{
				var st = GetScaleTransform(_child);
				var tt = GetTranslateTransform(_child);

				double zoom = e.Delta > 0 ? .2 : -.2;
				if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
				{
					return;
				}

				Point relative = e.GetPosition(_child);
				double abosuluteX;
				double abosuluteY;
				abosuluteX = relative.X * st.ScaleX + tt.X;
				abosuluteY = relative.Y * st.ScaleY + tt.Y;
				st.ScaleX += zoom;
				st.ScaleY += zoom;
				tt.X = abosuluteX - relative.X * st.ScaleX;
				tt.Y = abosuluteY - relative.Y * st.ScaleY;

			}
		}



		private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_child != null)
			{
				var tt = GetTranslateTransform(_child);
				_start = e.GetPosition(this);
				_origin = new Point(tt.X, tt.Y);
				this.Cursor = Cursors.Hand;
				_child.CaptureMouse();
			}
		}

		private void Child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_child != null)
			{
				_child.ReleaseMouseCapture();
				this.Cursor = Cursors.Arrow;
			}
		}

		void Child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.Reset();
		}

		private void Child_MouseMove(object sender, MouseEventArgs e)
		{
			if (_child != null)
			{
				if (_child.IsMouseCaptured)
				{
					var tt = GetTranslateTransform(_child);
					Vector v = _start - e.GetPosition(this);
					tt.X = _origin.X - v.X;
					tt.Y = _origin.Y - v.Y;
					//Debug.WriteLine("MouseMove tt.X:"+tt.X+" tt.Y:"+tt.Y);
				}
			}
		}
	}
}
