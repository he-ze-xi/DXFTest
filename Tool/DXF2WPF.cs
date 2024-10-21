using System;
using System.Windows.Controls;
using netDxf;
using System.Diagnostics;
using System.Windows.Media;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using System.Windows;
using System.Windows.Documents;
using System.Collections.Generic;

namespace TestDXF.Tool
{
	public class DXF2WPF
	{
		public static Color Window_bgColor = Color.FromRgb(33, 40, 48);


		public double GridHeight { get; set; }
		public double GridWidth { get; set; }
		public double WinHeight { get; set; }
		public double WinWidth { get; set; }
		public double ViewHeight { get; set; }
		public double ViewWidth { get; set; }
		public netDxf.DxfDocument DxfDoc;
		public ZoomBorder border = new ZoomBorder();
		public Canvas mainCanvas = new Canvas();
		public Grid mainGrid = new Grid();




		public Grid GetMainGrid(netDxf.DxfDocument dxfFile, bool avecGrille, bool avecOrigine, Color bgColor)
		{
			mainCanvas.Children.Clear();
			DrawEntities.RazMaxDim();
			DrawEntities.AddNewMaxDim();


			mainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
			mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
			mainGrid.Background = new SolidColorBrush(bgColor);

			border.Height = GridHeight;
			border.Width = GridWidth;
			border.Background = new SolidColorBrush(bgColor);


			mainCanvas.Name = "mainCanvas";
			if (DxfDoc.DrawingVariables.AcadVer < netDxf.Header.DxfVersion.AutoCad2000) DrawUtils.DrawText("Le format du fichier doit être dxf2000 ou supérieur.", 10, Colors.Red, 20, 50, mainCanvas);
			GetCanvas(DxfDoc, mainCanvas);

			if (avecOrigine == true)
			{
				DrawEntities.AddNewMaxDim();
				DrawUtils.DrawOrigin(mainCanvas);
				DrawEntities.DeleteLastMaxDim();
			}

			if (avecGrille == true)
			{
				Canvas bgCanvas = new Canvas();
				if (border.Child == null)
				{
					bgCanvas = CreateBgCanvas();
					bgCanvas.Children.Add(mainCanvas);

					border.Child = bgCanvas;
				}
			}
			else
			{
				border.Child = mainCanvas;
			}

			Canvas.SetTop(mainCanvas, GridHeight / 2);
			Canvas.SetLeft(mainCanvas, GridWidth / 2);

			//border.Reset(GridHeight,GridWidth,true,ViewHeight,ViewWidth,WinHeight,WinWidth);



			if (mainGrid.Children.Count == 0) mainGrid.Children.Add(border);

			return mainGrid;
		}


		public Grid GetMainGrid(string dxfFile, bool avecGrille, bool avecOrigine, Color bgColor)
		{
			DxfDoc = netDxf.DxfDocument.Load(dxfFile);
			DealDXFItems(DxfDoc);
			return GetMainGrid(DxfDoc, avecGrille, avecGrille, Window_bgColor);
		}

		public void DealDXFItems(DxfDocument dxfDocument)
		{
			DXFDataContainer.DataLineItems.Clear();
			DXFDataContainer.DataCircleItems.Clear();
			DXFDataContainer.DataArcItems.Clear();

			//直线
			if (dxfDocument.Lines.Count > 0 && dxfDocument.Lines != null)
			{
				int i = 0;
				foreach (var item in dxfDocument.Lines)
				{
					i++;
					DataLineItem dataLineItem = new DataLineItem();
					dataLineItem.Number = i;
					dataLineItem.fX1Pos = Math.Round(item.StartPoint.X, 4);
					dataLineItem.fY1Pos = Math.Round(item.StartPoint.Y, 4);
					dataLineItem.fX2Pos = Math.Round(item.EndPoint.X, 4);
					dataLineItem.fY2Pos = Math.Round(item.EndPoint.Y, 4);
					DXFDataContainer.DataLineItems.Add(dataLineItem);
				}
			}

			//圆
			if (dxfDocument.Circles.Count > 0 && dxfDocument.Circles != null)
			{
				int i = 0;
				foreach (var item in dxfDocument.Circles)
				{
					i++;
					DataCircleItem dataCircleItem = new DataCircleItem();
					dataCircleItem.Number = i;
					dataCircleItem.fXPos = Math.Round(item.Center.X, 4);
					dataCircleItem.fYPos = Math.Round(item.Center.Y, 4);
					dataCircleItem.fRPos = Math.Round(item.Radius, 4);
					DXFDataContainer.DataCircleItems.Add(dataCircleItem);
				}
			}

			//圆弧
			if (dxfDocument.Arcs.Count > 0 && dxfDocument.Arcs != null)
			{
				int i = 0;
				foreach (var item in dxfDocument.Arcs)
				{
					i++;
					DataArcItem dataArcItem = new DataArcItem();
					dataArcItem.Number = i;
					dataArcItem.fXPos = Math.Round(item.Center.X, 4);
					dataArcItem.fYPos = Math.Round(item.Center.Y, 4);
					dataArcItem.fRPos = Math.Round(item.Radius, 4);
					dataArcItem.fStartAngle = Math.Round(item.StartAngle, 4);
					dataArcItem.fEndAngle = Math.Round(item.EndAngle, 4);
					DXFDataContainer.DataArcItems.Add(dataArcItem);
				}
			}
		}


		public Grid GetMainGrid(string dxfFile, bool avecGrille, bool avecOrigine)
		{
			return GetMainGrid(dxfFile, avecGrille, avecGrille, Window_bgColor);
		}

		public static Canvas CreateBgCanvas()
		{
			Canvas BgCanvas = new Canvas();
			BgCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
			BgCanvas.VerticalAlignment = VerticalAlignment.Stretch;
			BgCanvas.Background = DrawUtils.GetGridBrush();
			/*mainCanvas.Background =  new SolidColorBrush(Colors.GreenYellow);*/
			return BgCanvas;
		}




		public static void GetCanvas(netDxf.DxfDocument DxfDoc, Canvas mainCanvas)
		{
			mainCanvas.Width = 250;
			mainCanvas.Height = 250;




			foreach (netDxf.Entities.Line xLine in DxfDoc.Lines)
			{
				DrawEntities.DrawLine(xLine, mainCanvas);
			}

			foreach (netDxf.Entities.Polyline xPoly in DxfDoc.Polylines)
			{
				DrawEntities.DrawPolyline(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.LwPolyline xPoly in DxfDoc.LwPolylines)
			{
				DrawEntities.DrawLwPolyline(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.MLine xPoly in DxfDoc.MLines)
			{
				DrawEntities.DrawMLine(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.Solid xPoly in DxfDoc.Solids)
			{
				DrawEntities.DrawSolid(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.PolyfaceMesh xPoly in DxfDoc.PolyfaceMeshes)
			{
				DrawEntities.DrawPolyfaceMesh(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.Wipeout xPoly in DxfDoc.Wipeouts)
			{
				DrawEntities.DrawWipeout(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.Face3d xPoly in DxfDoc.Faces3d)
			{
				DrawEntities.DrawFace3d(xPoly, mainCanvas);
			}

			foreach (netDxf.Entities.Circle xCircle in DxfDoc.Circles)
			{
				DrawEntities.DrawCircle(xCircle, mainCanvas);
			}

			foreach (netDxf.Entities.Ellipse xEllipse in DxfDoc.Ellipses)
			{
				DrawEntities.DrawEllipse(xEllipse, mainCanvas);
			}

			foreach (netDxf.Entities.Arc xArc in DxfDoc.Arcs)
			{
				DrawEntities.DrawArc(xArc, mainCanvas);
			}

			foreach (netDxf.Entities.Text xTxt in DxfDoc.Texts)
			{
				DrawEntities.DrawText(xTxt, mainCanvas);
			}

			foreach (netDxf.Entities.MText xTxt in DxfDoc.MTexts)
			{
				if (xTxt.Reactors.Count == 0) DrawEntities.DrawMText(xTxt, mainCanvas);
			}


			foreach (netDxf.Entities.Point xPoint in DxfDoc.Points)
			{
				DrawEntities.DrawPoint(xPoint, mainCanvas);
			}


			foreach (netDxf.Entities.Dimension xDim in DxfDoc.Dimensions)
			{
				xDim.Lineweight = Lineweight.W0;
				DrawEntities.DrawDimension(xDim, mainCanvas);
			}

			foreach (netDxf.Entities.Insert xInsert in DxfDoc.Inserts)
			{
				DrawEntities.DrawInsert(xInsert, mainCanvas);
			}


			foreach (netDxf.Entities.Leader xLeader in DxfDoc.Leaders)
			{
				DrawEntities.DrawLeader(xLeader, mainCanvas);
			}


			foreach (netDxf.Entities.Spline xSpline in DxfDoc.Splines)
			{
				DrawEntities.DrawSpline(xSpline, mainCanvas);
			}



			foreach (netDxf.Entities.Hatch xHatch in DxfDoc.Hatches)
			{
				DrawEntities.DrawHatch(xHatch, mainCanvas);
			}


			foreach (netDxf.Entities.Image xImage in DxfDoc.Images)
			{
				DrawEntities.DrawImage(xImage, mainCanvas);
			}


			foreach (netDxf.Entities.Underlay xUnderlay in DxfDoc.Underlays)
			{
				DrawEntities.DrawUnderlay(xUnderlay, mainCanvas);
			}

			foreach (netDxf.Entities.Mesh xMesh in DxfDoc.Meshes)
			{
				DrawEntities.DrawMesh(xMesh, mainCanvas);
			}


		}
	}
}
