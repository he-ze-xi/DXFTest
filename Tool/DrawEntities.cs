﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using netDxf.Entities;
using netDxf.Blocks;
using netDxf;
using System.Collections.Generic;
using netDxfPath = netDxf.Entities.HatchBoundaryPath;

namespace TestDXF.Tool
{
	public class DimMax
	{
		public double minX = double.MaxValue;
		public double minY = double.MaxValue;
		public double maxX = double.MinValue;
		public double maxY = double.MinValue;

		public double Height()
		{
			return maxY - minY;
		}
		public double Width()
		{
			return maxX - minX;
		}
	}

	public class DrawEntities
	{

		public static List<DimMax> listDim = new List<DimMax>();
		public static DimMax dimDoc = new DimMax();

		public DrawEntities()
		{

		}



		/*Draw Line*/
		public static void DrawLine(netDxf.Entities.Line xLine, Canvas mainCanvas)
		{
			double X1 = xLine.StartPoint.X;
			double Y1 = mainCanvas.Height - xLine.StartPoint.Y;
			double X2 = xLine.EndPoint.X;
			double Y2 = mainCanvas.Height - xLine.EndPoint.Y;
			getMaxPt(xLine.StartPoint);
			getMaxPt(xLine.EndPoint);
			System.Windows.Shapes.Line wLine = DrawUtils.GetLine(X1, Y1, X2, Y2);
			TypeConverter.Entity2Shape(xLine, wLine);
			mainCanvas.Children.Add(wLine);
		}


		public static void DrawPolyline(Polyline xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polyline wPoly = new System.Windows.Shapes.Polyline();

			foreach (netDxf.Entities.PolylineVertex xVertex in xPoly.Vertexes)
			{
				System.Windows.Point myPt = TypeConverter.Vertex3ToPoint(xVertex.Position);
				getMaxPt(myPt);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);
			}

			if (xPoly.IsClosed == true)
				wPoly.Points.Add(wPoly.Points[0]);
			TypeConverter.Entity2Shape(xPoly, wPoly);
			mainCanvas.Children.Add(wPoly);
		}

		/*Draw LwPolyline*/
		public static void DrawLwPolyline(LwPolyline xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polyline wPoly = new System.Windows.Shapes.Polyline();

			foreach (netDxf.Entities.LwPolylineVertex xVertex in xPoly.Vertexes)
			{
				System.Windows.Point myPt = TypeConverter.Vertex2ToPoint(xVertex.Position);
				getMaxPt(myPt);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);

			}

			if (xPoly.IsClosed == true)
				wPoly.Points.Add(wPoly.Points[0]);
			TypeConverter.Entity2Shape(xPoly, wPoly);
			mainCanvas.Children.Add(wPoly);
		}


		/*Draw Mline*/
		public static void DrawMLine(MLine xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polyline wPoly = new System.Windows.Shapes.Polyline();

			Vector2 vectTrans = new Vector2();
			var i = 0;
			foreach (netDxf.Entities.MLineVertex xVertex in xPoly.Vertexes)
			{
				Vector2 vect = xVertex.Location;

				if (i > 0 && xPoly.Justification == netDxf.Entities.MLineJustification.Top)
				{
					Vector2 vect2 = vect - xPoly.Vertexes[i - 1].Location;

					vectTrans.X = vect2.Y;
					vectTrans.Y = vect2.X;
					vectTrans.Normalize();
					vectTrans = Vector2.Multiply(vectTrans, xPoly.Scale / 2);
				}
				System.Windows.Point myPt = TypeConverter.Vertex2ToPoint(vect);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);
				i++;
			}

			TranslateTransform translateTransform1 = new TranslateTransform(vectTrans.X, vectTrans.Y);
			wPoly.RenderTransform = translateTransform1;


			if (xPoly.IsClosed == true)
				wPoly.Points.Add(wPoly.Points[0]);
			TypeConverter.Entity2Shape(xPoly, wPoly);

			wPoly.StrokeThickness = xPoly.Scale;

			mainCanvas.Children.Add(wPoly);
		}



		/*Draw Solid*/
		public static void DrawSolid(Solid xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();

			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(xPoly.FirstVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(xPoly.SecondVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(xPoly.FourthVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(xPoly.ThirdVertex, mainCanvas.Height));
			/*TypeConverter.Entity2Shape(xPoly,wPoly);*/
			wPoly.Fill = DrawUtils.GetFillBrush(xPoly.getColor(), xPoly.Transparency.Value);
			mainCanvas.Children.Add(wPoly);
		}


		/*Draw PolyfaceMesh*/
		public static void DrawPolyfaceMesh(PolyfaceMesh xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();

			foreach (netDxf.Entities.PolyfaceMeshVertex xVertex in xPoly.Vertexes)
			{
				System.Windows.Point myPt = TypeConverter.Vertex3ToPoint(xVertex.Location);
				getMaxPt(myPt);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);
			}
			TypeConverter.Entity2Shape(xPoly, wPoly);
			mainCanvas.Children.Add(wPoly);
		}


		/*Draw Wipeout*/
		public static void DrawWipeout(Wipeout xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();

			foreach (netDxf.Vector2 xVertex in xPoly.ClippingBoundary.Vertexes)
			{
				System.Windows.Point myPt = TypeConverter.Vertex2ToPoint(xVertex);
				getMaxPt(myPt);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);
			}
			TypeConverter.Entity2Shape(xPoly, wPoly);
			wPoly.Fill = new SolidColorBrush(DXF2WPF.Window_bgColor);
			mainCanvas.Children.Add(wPoly);
		}


		/*Draw Face3d*/
		public static void DrawFace3d(Face3d xPoly, Canvas mainCanvas)
		{
			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();

			wPoly.Points.Add(TypeConverter.Vertex3ToPoint(xPoly.FirstVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex3ToPoint(xPoly.SecondVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex3ToPoint(xPoly.ThirdVertex, mainCanvas.Height));
			wPoly.Points.Add(TypeConverter.Vertex3ToPoint(xPoly.FourthVertex, mainCanvas.Height));
			TypeConverter.Entity2Shape(xPoly, wPoly);
			mainCanvas.Children.Add(wPoly);
		}

		/*Draw Circle*/
		public static void DrawCircle(Circle xCircle, Canvas mainCanvas)
		{
			System.Windows.Shapes.Ellipse wCircle = new System.Windows.Shapes.Ellipse();
			wCircle.Width = xCircle.Radius * 2;
			wCircle.Height = xCircle.Radius * 2;
			getMaxPt(xCircle.Center.X + xCircle.Radius, xCircle.Center.Y + xCircle.Radius);
			getMaxPt(xCircle.Center.X - xCircle.Radius, xCircle.Center.Y - xCircle.Radius);
			Canvas.SetLeft(wCircle, xCircle.Center.X - xCircle.Radius);
			Canvas.SetTop(wCircle, mainCanvas.Height - (xCircle.Center.Y + xCircle.Radius));
			TypeConverter.Entity2Shape(xCircle, wCircle);
			mainCanvas.Children.Add(wCircle);
		}


		/*Draw Ellipse*/
		public static void DrawEllipse(Ellipse xEllipse, Canvas mainCanvas)
		{
			System.Windows.Shapes.Ellipse wEllipse = new System.Windows.Shapes.Ellipse();
			wEllipse.Width = xEllipse.MajorAxis;
			wEllipse.Height = xEllipse.MinorAxis;
			getMaxPt(xEllipse.Center.X + xEllipse.MajorAxis / 2, xEllipse.Center.Y + xEllipse.MinorAxis / 2);
			getMaxPt(xEllipse.Center.X - xEllipse.MajorAxis / 2, xEllipse.Center.Y - xEllipse.MinorAxis / 2);
			Canvas.SetLeft(wEllipse, xEllipse.Center.X - xEllipse.MajorAxis / 2);
			Canvas.SetTop(wEllipse, mainCanvas.Height - (xEllipse.Center.Y + xEllipse.MinorAxis / 2));
			TypeConverter.Entity2Shape(xEllipse, wEllipse);
			mainCanvas.Children.Add(wEllipse);
		}


		/*Draw Arc*/
		public static void DrawArc(netDxf.Entities.Arc arc, Canvas mainCanvas)
		{



			System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
			path.Stroke = System.Windows.Media.Brushes.Black;
			path.StrokeThickness = 1;

			System.Windows.Point endPoint = new System.Windows.Point(
				(arc.Center.X + Math.Cos(arc.StartAngle * Math.PI / 180) * arc.Radius),
				mainCanvas.Height - (arc.Center.Y + Math.Sin(arc.StartAngle * Math.PI / 180) * arc.Radius));

			System.Windows.Point startPoint = new System.Windows.Point(
				(arc.Center.X + Math.Cos(arc.EndAngle * Math.PI / 180) * arc.Radius),
				mainCanvas.Height - (arc.Center.Y + Math.Sin(arc.EndAngle * Math.PI / 180) * arc.Radius));


			Debug.WriteLine("debut:" + startPoint.Y + " fin:" + endPoint.Y);


			getMaxPt((arc.Center.X + Math.Cos(arc.StartAngle * Math.PI / 180) * arc.Radius),
					 (arc.Center.Y + Math.Sin(arc.StartAngle * Math.PI / 180) * arc.Radius));

			getMaxPt((arc.Center.X + Math.Cos(arc.EndAngle * Math.PI / 180) * arc.Radius),
					 (arc.Center.Y + Math.Sin(arc.EndAngle * Math.PI / 180) * arc.Radius));



			ArcSegment arcSegment = new ArcSegment();
			double sweep = 0.0;
			if (arc.EndAngle < arc.StartAngle)
				sweep = (360 + arc.EndAngle) - arc.StartAngle;
			else sweep = Math.Abs(arc.EndAngle - arc.StartAngle);

			arcSegment.IsLargeArc = sweep >= 180;
			arcSegment.Point = endPoint;
			arcSegment.Size = new System.Windows.Size(arc.Radius, arc.Radius);
			arcSegment.SweepDirection = arc.Normal.Z >= 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;

			PathGeometry geometry = new PathGeometry();
			PathFigure pathFigure = new PathFigure();
			pathFigure.StartPoint = startPoint;
			pathFigure.Segments.Add(arcSegment);
			geometry.Figures.Add(pathFigure);

			path.Data = geometry;
			TypeConverter.Entity2Shape(arc, path);


			if (arc.StartAngle <= 90 && arc.EndAngle >= 90)
			{
				//Debug.WriteLine("ARC Blue "+arc.Center.ToString()+" R:"+arc.Radius+" start:"+arc.StartAngle+" end:"+arc.EndAngle);
				getMaxPt((arc.Center.X),
						 (arc.Center.Y + arc.Radius));
			}


			if (arc.StartAngle <= 180 && arc.EndAngle >= 180)
			{
				//Debug.WriteLine("ARC Red "+arc.Center.ToString()+" R:"+arc.Radius+" start:"+arc.StartAngle+" end:"+arc.EndAngle);
				getMaxPt((arc.Center.X - arc.Radius),
						 (arc.Center.Y));
			}


			if (arc.StartAngle <= 270 && arc.EndAngle >= 270)
			{
				//Debug.WriteLine("ARC Green "+arc.Center.ToString()+" R:"+arc.Radius+" start:"+arc.StartAngle+" end:"+arc.EndAngle);
				getMaxPt((arc.Center.X),
						 (arc.Center.Y - arc.Radius));
			}


			if (arc.StartAngle <= 380 && arc.EndAngle >= 0 && arc.StartAngle > arc.EndAngle)
			{
				//Debug.WriteLine("ARC Green "+arc.Center.ToString()+" R:"+arc.Radius+" start:"+arc.StartAngle+" end:"+arc.EndAngle);
				getMaxPt((arc.Center.X + arc.Radius),
						 (arc.Center.Y));
			}


			mainCanvas.Children.Add(path);

		}


		/*Draw Text*/
		public static void DrawText(netDxf.Entities.Text xTxt, Canvas mainCanvas)
		{
			TextBlock wTxt = new TextBlock();
			/*wTxt.Text = xTxt.Value;*/
			TextUtils.CADTxtToInlineCollection(wTxt.Inlines, xTxt.Value.ToString(), wTxt.FontSize);
			wTxt.FontWeight = FontWeights.Bold;
			wTxt.FontSize = TypeConverter.PointsToPixels(xTxt.Height);
			wTxt.FontFamily = new FontFamily(xTxt.Style.FontFamilyName);

			Size txtSize = TypeConverter.MeasureString(wTxt, xTxt.Value);
			wTxt.Width = txtSize.Width;
			wTxt.Height = txtSize.Height;

			wTxt.HorizontalAlignment = HorizontalAlignment.Center;
			wTxt.VerticalAlignment = VerticalAlignment.Center;
			wTxt.TextAlignment = System.Windows.TextAlignment.Center;
			wTxt.Foreground = TypeConverter.AciColorToBrush(xTxt.getColor());

			Canvas.SetLeft(wTxt, xTxt.Position.X - wTxt.Width / 2);
			Canvas.SetTop(wTxt, mainCanvas.Height - (xTxt.Position.Y + wTxt.Height / 2));

			mainCanvas.Children.Add(wTxt);
		}

		/*Draw MText*/
		public static Size DrawMText(MText xTxt, Canvas mainCanvas)
		{
			System.Windows.Controls.TextBlock wTxt = new System.Windows.Controls.TextBlock();
			/*wTxt.Text = xTxt.Value;*/




			wTxt.FontSize = TypeConverter.PointsToPixels(xTxt.Height);
			wTxt.FontFamily = new FontFamily(xTxt.Style.FontFamilyName);
			wTxt.Foreground = TypeConverter.AciColorToBrush(xTxt.getColor());

			wTxt.LineHeight = xTxt.LineSpacingFactor * xTxt.Height * 1.66;
			wTxt.Padding = new Thickness(0, 0, 0, 0);
			wTxt.Margin = new Thickness(0, 0, 0, 0);
			wTxt.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
			wTxt.TextAlignment = TypeConverter.AttachmentPointToAlign(xTxt.AttachmentPoint);
			//wTxt.TextAlignment = TypeConverter.AttachmentPointToAlign(MTextAttachmentPoint.TopRight);
			wTxt.FontStretch = FontStretches.UltraExpanded;
			wTxt.TextWrapping = TextWrapping.Wrap;

			TextUtils.CADTxtToInlineCollection(wTxt.Inlines, xTxt.Value.ToString(), wTxt.FontSize);

			Size txtSize = TypeConverter.MeasureString(wTxt, xTxt.Value, xTxt.RectangleWidth);
			if (xTxt.RectangleWidth > 0)
			{
				wTxt.Width = xTxt.RectangleWidth;
				wTxt.MaxWidth = xTxt.RectangleWidth;
				wTxt.Height = xTxt.Height;
			}
			else
			{
				wTxt.Width = txtSize.Width;
				wTxt.Height = txtSize.Height;
			}
			//Vector3 newPosition = TypeConverter.TextAttachmentToPosition(MTextAttachmentPoint.TopLeft, xTxt.Position, wTxt.Width, txtSize.Height);
			//Vector3 newPosition = TypeConverter.TextAttachmentToPosition(xTxt.AttachmentPoint, xTxt.Position, wTxt.Width, txtSize.Height);
			Vector3 newPosition = TypeConverter.TextAttachmentToPosition(xTxt.AttachmentPoint, xTxt.Position, wTxt.Width, wTxt.Height, xTxt.Rotation);
			RotateTransform rotat = new RotateTransform(-xTxt.Rotation);
			rotat.CenterX = wTxt.Width / 2;
			rotat.CenterY = txtSize.Height / 2;
			wTxt.RenderTransform = rotat;


			/*DrawUtils.DrawPoint(Canvas.GetLeft(mainCanvas),-Canvas.GetTop(mainCanvas),mainCanvas,Colors.Green,5,0.1);*/
			/*DrawUtils.DrawPoint(xTxt.Position,mainCanvas,Colors.Blue,5,0.1);*/

			Canvas.SetLeft(wTxt, newPosition.X);
			Canvas.SetTop(wTxt, mainCanvas.Height - newPosition.Y);
			mainCanvas.Children.Add(wTxt);
			return txtSize;
		}


		/*Draw Point*/
		public static void DrawPoint(netDxf.Entities.Point xPoint, Canvas mainCanvas)
		{
			double size = 15.0;
			AciColor myColor = xPoint.getColor();
			Canvas canvas1 = DrawUtils.GetPoint(TypeConverter.ToMediaColor(myColor.ToColor()), size, 0);
			getMaxPt(xPoint.Position);
			Canvas.SetLeft(canvas1, xPoint.Position.X - size);
			Canvas.SetTop(canvas1, mainCanvas.Height - (xPoint.Position.Y + size));
			mainCanvas.Children.Add(canvas1);

		}


		/*Draw Dimension*/
		public static void DrawDimension(Dimension xDim, Canvas mainCanvas)
		{
			/*Debug.WriteLine("Dim:"+xDim.Handle+" Txt="+xDim.UserText+" Type="+xDim.DimensionType.ToString());*/
			//Debug.WriteLine("LineweightDim=" + xDim.Lineweight.ToString());
			xDim.Block.Layer = xDim.Layer;
			Canvas canvas1 = GetBlock(xDim.Block, xDim.Color, xDim.Lineweight);

			Canvas.SetLeft(canvas1, 0);
			Canvas.SetTop(canvas1, mainCanvas.Height - canvas1.Height);
			mainCanvas.Children.Add(canvas1);
		}


		/*Get Block in canvas*/
		public static Canvas GetBlock(Block xBlock, AciColor blockColor, Lineweight linew)
		{

			Canvas canvas1 = new Canvas();
			canvas1.Height = 0;
			canvas1.Width = 0;
			/*Debug.WriteLine("Block:"+xBlock.Handle+" Owner:"+xBlock.Owner+" OriginX:"+xBlock.Origin.X);*/
			foreach (netDxf.Entities.EntityObject xEntity in xBlock.Entities)
			{
				xEntity.Layer = xBlock.Layer;
				/*Debug.WriteLine("Entity:"+xEntity.CodeName+" Handle:"+xEntity.Handle+" Color:"+xEntity.getColor().ToString());*/

				if (xEntity.getColor().IsByBlock)
					xEntity.Color = blockColor;
				if (xEntity.getLineweightValue() == -2)
					xEntity.Lineweight = linew;

				if (xEntity.Type == EntityType.Line)
					DrawLine((netDxf.Entities.Line)xEntity, canvas1);
				if (xEntity.Type == EntityType.Circle)
					DrawCircle((netDxf.Entities.Circle)xEntity, canvas1);
				if (xEntity.Type == EntityType.MText)
					DrawMText((netDxf.Entities.MText)xEntity, canvas1);
				if (xEntity.Type == EntityType.Solid)
					DrawSolid((netDxf.Entities.Solid)xEntity, canvas1);
				if (xEntity.Type == EntityType.Arc)
					DrawArc((netDxf.Entities.Arc)xEntity, canvas1);
				if (xEntity.Type == EntityType.Insert)
					DrawInsert((netDxf.Entities.Insert)xEntity, canvas1);
				if (xEntity.Type == EntityType.LightWeightPolyline)
					DrawLwPolyline((netDxf.Entities.LwPolyline)xEntity, canvas1);

			}



			/*DxfObject xOwner = xBlock.Owner;*/
			/*Debug.WriteLine("Owner:"+xOwner.Handle+" Owner:"+xOwner.Owner+" Owner:"+xOwner.Owner.Owner);*/

			return canvas1;
		}


		/*Draw Insert*/
		public static void DrawInsert(Insert xInsert, Canvas mainCanvas)
		{
			xInsert.Block.Layer = xInsert.Layer;
			Canvas canvas1 = new Canvas();

			AddNewMaxDim();
			canvas1 = GetBlock(xInsert.Block, xInsert.Color, xInsert.Lineweight);
			DimMax dim = GetLastMaxDim();

			mainCanvas.Children.Add(canvas1);
			double x1, x2, y1, y2;



			Vector2 vCenter = getBlockCenter();


			x1 = dim.maxX * xInsert.Scale.X + xInsert.Position.X;
			y1 = dim.maxY * xInsert.Scale.Y + xInsert.Position.Y;
			x2 = dim.minX * xInsert.Scale.X + xInsert.Position.X;
			y2 = dim.minY * xInsert.Scale.Y + xInsert.Position.Y;

			dim.maxX = Math.Max(x1, x2);
			dim.minX = Math.Min(x1, x2);
			dim.maxY = Math.Max(y1, y2);
			dim.minY = Math.Min(y1, y2);

			Canvas.SetLeft(canvas1, xInsert.Position.X);
			//if (xInsert.Scale.X < 0) Canvas.SetLeft(canvas1, dim.maxX);
			Canvas.SetTop(canvas1, mainCanvas.Height - (xInsert.Position.Y + canvas1.Height));
			//if (xInsert.Scale.Y < 0) Canvas.SetTop(canvas1, mainCanvas.Height - (dim.maxY));;

			TransformGroup trgr = new TransformGroup();
			canvas1.RenderTransform = trgr;
			if (xInsert.Rotation != 0.0)
			{
				trgr.Children.Add(new RotateTransform(xInsert.Rotation, 0, 0));

			}

			if ((xInsert.Scale.X != 1.0 || xInsert.Scale.Y != 1.0) && xInsert.Scale.X != 0 && xInsert.Scale.Y != 0)
			{
				trgr.Children.Add(new ScaleTransform(xInsert.Scale.X, xInsert.Scale.Y, 0, 0));
			}


			foreach (netDxf.Entities.Attribute xAttrib in xInsert.Attributes)
			{
				xAttrib.Layer = xInsert.Layer;
				DrawAttribute(xAttrib, mainCanvas);
			}



		}


		/*Draw Attribute*/
		public static void DrawAttribute(netDxf.Entities.Attribute xTxt, Canvas mainCanvas)
		{
			System.Windows.Controls.TextBlock wTxt = new System.Windows.Controls.TextBlock();
			TextUtils.CADTxtToInlineCollection(wTxt.Inlines, xTxt.Value.ToString(), wTxt.FontSize);

			wTxt.FontSize = TypeConverter.PointsToPixels(xTxt.Height);
			wTxt.FontFamily = new FontFamily(xTxt.Style.FontFamilyName);
			wTxt.Foreground = TypeConverter.AciColorToBrush(xTxt.getColor());

			wTxt.LineHeight = xTxt.Height * 1.66;

			wTxt.Padding = new Thickness(0, 0, 0, 0);
			wTxt.Margin = new Thickness(-1, -1, 0, 0);
			wTxt.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;

			wTxt.FontStretch = FontStretches.UltraExpanded;
			wTxt.TextWrapping = TextWrapping.Wrap;

			Size txtSize = TypeConverter.MeasureString(wTxt, xTxt.Value.ToString());
			wTxt.Width = txtSize.Width;
			wTxt.Height = txtSize.Height;

			Vector3 newPosition = TypeConverter.TextAlignmentToPosition(xTxt.Alignment, xTxt.Position, wTxt.Width, xTxt.Height);

			Canvas.SetLeft(wTxt, newPosition.X);
			Canvas.SetTop(wTxt, mainCanvas.Height - newPosition.Y);

			mainCanvas.Children.Add(wTxt);
			/*DrawUtils.DrawPoint(newPosition,mainCanvas,Colors.Red,10,1);*/


		}


		/*Draw Leader*/
		public static void DrawLeader(Leader xLeader, Canvas mainCanvas)
		{
			Size txtSize = new Size(0, 0);
			/*ajout du texte*/
			if (xLeader.Annotation.Type == EntityType.MText)
			{
				netDxf.Entities.MText mText = (netDxf.Entities.MText)xLeader.Annotation;
				txtSize = DrawMText(mText, mainCanvas);
			}

			if (xLeader.Annotation.Type == EntityType.Text)
			{
				netDxf.Entities.Text mText = (netDxf.Entities.Text)xLeader.Annotation;
				DrawText(mText, mainCanvas);
			}
			if (xLeader.Annotation.Type == EntityType.Insert)
			{
				netDxf.Entities.Insert mText = (netDxf.Entities.Insert)xLeader.Annotation;
				DrawInsert(mText, mainCanvas);
			}





			System.Windows.Shapes.Polyline wPoly = new System.Windows.Shapes.Polyline();

			foreach (netDxf.Vector2 xVertex in xLeader.Vertexes)
			{
				System.Windows.Point myPt = TypeConverter.Vertex2ToPoint(xVertex);
				myPt.Y = mainCanvas.Height - myPt.Y;
				wPoly.Points.Add(myPt);
			}
			System.Windows.Point myPt2 = TypeConverter.Vertex2ToPoint(xLeader.Hook);
			myPt2.Y = mainCanvas.Height - myPt2.Y;
			wPoly.Points.Add(myPt2);

			if (txtSize.Width > 0)
			{
				myPt2.X = myPt2.X + txtSize.Width;
				wPoly.Points.Add(myPt2);
			}

			xLeader.Lineweight = Lineweight.W0;

			TypeConverter.Entity2Shape(xLeader, wPoly);

			if (xLeader.ShowArrowhead == true)
			{
				System.Windows.Shapes.Polygon arrow = DrawUtils.GetArrowhead(xLeader.Vertexes[0], xLeader.Vertexes[1], mainCanvas);
				TypeConverter.Entity2Shape(xLeader, arrow);
				arrow.StrokeThickness = 0.1;
				arrow.Fill = arrow.Stroke;
				mainCanvas.Children.Add(arrow);
			}

			mainCanvas.Children.Add(wPoly);





		}


		/*Draw Spline*/
		public static void DrawSpline(Spline xLine, Canvas mainCanvas)
		{

			System.Windows.Shapes.Path wPath = new System.Windows.Shapes.Path();


			/*List<PolyQuadraticBezierSegment> segments = new List<PolyQuadraticBezierSegment>(1);
			PolyQuadraticBezierSegment segment = new PolyQuadraticBezierSegment();
			 */
			List<PolyBezierSegment> segments = new List<PolyBezierSegment>();
			PolyBezierSegment segment = new PolyBezierSegment();

			int i = 0;
			foreach (netDxf.Entities.SplineVertex spVertex in xLine.ControlPoints)
			{
				if (i > 0)
					segment.Points.Add(TypeConverter.Vertex3ToPoint(spVertex.Position, mainCanvas.Height));
				/*DrawUtils.DrawPoint(spVertex.Position,i.ToString(),mainCanvas,Colors.Red,5,0.5);*/
				i++;
			}
			segments.Add(segment);
			List<PathFigure> figures = new List<PathFigure>();
			System.Windows.Point p0 = TypeConverter.Vertex3ToPoint(xLine.ControlPoints[0].Position, mainCanvas.Height);

			/*System.Windows.Point p0 = new System.Windows.Point(710,mainCanvas.Height+100);*/
			PathFigure pf = new PathFigure(p0, segments, true);
			pf.IsClosed = false;
			figures.Add(pf);

			Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
			wPath.Data = g;

			TypeConverter.Entity2Shape(xLine, wPath);
			mainCanvas.Children.Add(wPath);

		}



		/*Draw Hatch*/
		public static void DrawHatch(Hatch xHatch, Canvas mainCanvas)
		{

			System.Windows.Point p0 = new System.Windows.Point();
			System.Windows.Shapes.Path wPath = new System.Windows.Shapes.Path();

			List<PathSegment> segments = new List<PathSegment>();
			PathSegment segment;
			List<PathFigure> figures = new List<PathFigure>();


			int i = 0;
			foreach (netDxf.Entities.HatchBoundaryPath xPath in xHatch.BoundaryPaths)
			{
				segments = new List<PathSegment>();

				Debug.WriteLine("xpath:" + xPath.PathType);
				int j = 0;
				foreach (netDxf.Entities.HatchBoundaryPath.Edge xEdge in xPath.Edges)
				{

					Debug.WriteLine("xEdge:" + xEdge.Type);

					if (xEdge.Type == netDxfPath.EdgeType.Line)
					{
						netDxfPath.Line wLine = (netDxfPath.Line)xEdge;
						segment = new LineSegment(TypeConverter.Vertex2ToPoint(wLine.End, mainCanvas.Height), true);
						segments.Add(segment);
						if (j == 0)
							p0 = TypeConverter.Vertex2ToPoint(wLine.Start, mainCanvas.Height);
					}
					j++;


					/*if(xEdge.Type==netDxf.Entities.HatchBoundaryPath.EdgeType.Line) DrawLine((netDxf.Entities.Line)xEdge.ConvertTo(),mainCanvas);
					if(xEdge.Type==netDxf.Entities.HatchBoundaryPath.EdgeType.Ellipse) DrawEllipse((netDxf.Entities.Ellipse)xEdge.ConvertTo(),mainCanvas);
					if(xEdge.Type==netDxf.Entities.HatchBoundaryPath.EdgeType.Spline) DrawSpline((netDxf.Entities.Spline)xEdge.ConvertTo(),mainCanvas);
					if(xEdge.Type==netDxf.Entities.HatchBoundaryPath.EdgeType.Arc) DrawArc((netDxf.Entities.Arc)xEdge.ConvertTo(),mainCanvas);
					if(xEdge.Type==netDxf.Entities.HatchBoundaryPath.EdgeType.Polyline) DrawLwPolyline((netDxf.Entities.LwPolyline)xEdge.ConvertTo(),mainCanvas);
					 */
				}

				PathFigure pf = new PathFigure(p0, segments, true);
				pf.IsClosed = false;
				figures.Add(pf);

				i++;
			}

			Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
			wPath.Data = g;
			wPath.Fill = TypeConverter.PatternToBrush(xHatch.Pattern, xHatch.getColor());


			TypeConverter.Entity2Shape(xHatch, wPath);
			wPath.StrokeThickness = 0;
			mainCanvas.Children.Add(wPath);

		}


		/*Draw Image*/
		public static void DrawImage(netDxf.Entities.Image xImg, Canvas mainCanvas)
		{
			string local = AppDomain.CurrentDomain.BaseDirectory;
			System.Windows.Controls.Image wImg = new System.Windows.Controls.Image();
			System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage();
			bi3.BeginInit();
			/*Uri img_uri = new Uri(xImg.Definition.FileName.Replace(@".\", local));*/
			//Uri img_uri;
			try
			{
				//Uri.TryCreate(xImg.Definition.FileName.Replace(@".\", local),System.UriKind.Absolute,out img_uri);
				Uri img_uri = new Uri(xImg.Definition.FileName.Replace(@".\", local));
				if (img_uri.IsFile == true)
				{
					bi3.UriSource = img_uri;
					/*bi3.UriSource = new Uri(@"C:\Users\Michel\Documents\SharpDevelop Projects\NetDXFViewer\NetDXFViewer\bin\Debug\image.jpg");*/
					bi3.EndInit();
					wImg.Stretch = Stretch.Fill;
					wImg.Source = bi3;
					wImg.Height = xImg.Height;
					wImg.Width = xImg.Width;
					Canvas.SetLeft(wImg, xImg.Position.X);
					Canvas.SetTop(wImg, mainCanvas.Height - xImg.Position.Y - wImg.Height);
					mainCanvas.Children.Add(wImg);
				}
			}
			catch
			{
				Debug.WriteLine("Image introuvable:" + xImg.Definition.FileName.ToString());
			}


			//if (Uri.TryCreate(xImg.Definition.FileName.Replace(@".\", local),System.UriKind.Absolute,out img_uri))
			//if (Uri.TryCreate(xImg.Definition.FileName,System.UriKind.Relative,out img_uri))
			/*if (img_uri.IsFile==true)
			{
				bi3.UriSource = img_uri;
				//bi3.UriSource = new Uri(@"C:\Users\Michel\Documents\SharpDevelop Projects\NetDXFViewer\NetDXFViewer\bin\Debug\image.jpg");
				bi3.EndInit();
				wImg.Stretch = Stretch.Fill;
				wImg.Source = bi3;
				wImg.Height = xImg.Height;
				wImg.Width = xImg.Width;
				Canvas.SetLeft(wImg, xImg.Position.X);
				Canvas.SetTop(wImg, mainCanvas.Height - xImg.Position.Y - wImg.Height);
				mainCanvas.Children.Add(wImg);
			}*/
		}

		/*Draw Underlay*/
		public static void DrawUnderlay(Underlay xUnderlay, Canvas mainCanvas)
		{
			/*DrawUtils.DrawPoint(xUnderlay.Position, mainCanvas, Colors.Red, 10, 0.5);*/
			DrawUtils.DrawText(xUnderlay.Definition.FileName, 10, TypeConverter.ToMediaColor(xUnderlay.getColor().ToColor()), xUnderlay.Position.X, xUnderlay.Position.Y + 20, mainCanvas);
		}



		/*Draw Mesh*/
		public static void DrawMesh(Mesh xMesh, Canvas mainCanvas)
		{

			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();

			foreach (netDxf.Entities.MeshEdge xEdge in xMesh.Edges)
			{

				System.Windows.Point myPt1 = TypeConverter.Vertex3ToPoint(xMesh.Vertexes[xEdge.StartVertexIndex]);
				System.Windows.Point myPt2 = TypeConverter.Vertex3ToPoint(xMesh.Vertexes[xEdge.EndVertexIndex]);
				myPt1.Y = mainCanvas.Height - myPt1.Y;
				myPt2.Y = mainCanvas.Height - myPt2.Y;

				System.Windows.Shapes.Line wLine = DrawUtils.GetLine(myPt1.X, myPt1.Y, myPt2.X, myPt2.Y);


				TypeConverter.Entity2Shape(xMesh, wLine);

				mainCanvas.Children.Add(wLine);
			}

		}



		public static void getMaxPt(System.Windows.Point pt)
		{
			DimMax dim = GetLastMaxDim();
			if (dim != null)
			{
				if (pt.X > dim.maxX) dim.maxX = pt.X;
				if (pt.Y > dim.maxY) dim.maxY = pt.Y;
				if (pt.X < dim.minX) dim.minX = pt.X;
				if (pt.Y < dim.minY) dim.minY = pt.Y;
			}

		}

		public static void getMaxPt(double X, double Y)
		{
			System.Windows.Point pt = new System.Windows.Point(X, Y);
			getMaxPt(pt);
		}

		public static void getMaxPt(Vector3 V3)
		{
			System.Windows.Point pt = new System.Windows.Point(V3.X, V3.Y);
			getMaxPt(pt);
		}

		public static void getMaxPt(Vector2 V2)
		{
			System.Windows.Point pt = new System.Windows.Point(V2.X, V2.Y);
			getMaxPt(pt);
		}

		/*public static void initMaxPt()
		{
			maxX=Double.MinValue;
			minX=Double.MaxValue;
			maxY=Double.MinValue;
			minY=Double.MaxValue;
		}*/


		public static Vector2 getBlockCenter()
		{
			DimMax dim = GetLastMaxDim();
			Vector2 v1 = new Vector2(dim.minX, dim.minY);
			Vector2 v2 = new Vector2(dim.maxX, dim.maxY);
			Vector2 vres = (v1 + v2) / 2;

			return vres;
		}

		public static DimMax GetLastMaxDim()
		{
			int pos = listDim.Count - 1;
			if (pos >= 0) return listDim[pos]; else return null;

		}

		public static void DeleteLastMaxDim()
		{
			int pos = listDim.Count - 1;
			if (pos >= 0) listDim.RemoveAt(pos);

		}

		public static void AddNewMaxDim()
		{
			DimMax newDim = new DimMax();
			listDim.Add(newDim);
		}

		public static void RazMaxDim()
		{
			dimDoc.maxX = double.MinValue;
			dimDoc.maxY = double.MinValue;
			dimDoc.minX = double.MaxValue;
			dimDoc.minY = double.MaxValue;
			listDim.Clear();
		}

		public static void CalcMaxDimDoc()
		{
			foreach (DimMax dim in listDim)
			{
				if (dim.maxX > dimDoc.maxX) dimDoc.maxX = dim.maxX;
				if (dim.maxY > dimDoc.maxY) dimDoc.maxY = dim.maxY;
				if (dim.minX < dimDoc.minX) dimDoc.minX = dim.minX;
				if (dim.minY < dimDoc.minY) dimDoc.minY = dim.minY;
			}

			//Debug.WriteLine("DimMaxDoc: maxX="+dimDoc.maxX+" maxY="+dimDoc.maxY+" minX="+dimDoc.minX+" minY="+dimDoc.minY);
		}

	}
}
