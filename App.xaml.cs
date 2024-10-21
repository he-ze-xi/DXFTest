﻿using Prism.Ioc;
using System.Windows;
using TestDXF.Views;

namespace TestDXF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<Window1>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{

		}
	}
}