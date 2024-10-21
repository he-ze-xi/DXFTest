using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDXF.Tool
{
	/// <summary>
	/// 存放解析DXF图形数据的类
	/// </summary>
	public static class DXFDataContainer
	{
		public static List<DataLineItem> DataLineItems { get; set; }=new List<DataLineItem>();
		public static List<DataArcItem> DataArcItems { get; set; } = new List<DataArcItem>();
		public static List<DataCircleItem> DataCircleItems { get; set; } = new List<DataCircleItem>();

	}


	#region 图形结构
	public class DataLineItem
	{
		public int Number { get; set; }
		public double fX1Pos { get; set; }
		public double fY1Pos { get; set; }
		public double fX2Pos { get; set; }
		public double fY2Pos { get; set; }
	}

	public class DataArcItem
	{
		public int Number { get; set; }
		public double fXPos { get; set; }
		public double fYPos { get; set; }
		public double fRPos { get; set; }
		public double fStartAngle { get; set; }
		public double fEndAngle { get; set; }

	};

	public class DataCircleItem
	{
		public int Number { get; set; }
		public double fXPos { get; set; }
		public double fYPos { get; set; }
		public double fRPos { get; set; }
	};
	#endregion
}
