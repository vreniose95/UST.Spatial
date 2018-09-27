using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Ccr.Core.Extensions;
using Ccr.PresentationCore.Helpers.DependencyHelpers;
using Ccr.PresentationCore.Helpers.EventHelpers;
using UST.Spatial.GeoJSON;


namespace UST.Spatial.Controls
{
	public class ZipCodeGeometryPresenter
		: Control
	{
		private Path PART_ZipCodeBoundaryPath;


		public static readonly DependencyProperty ZipCodeDeclarationProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, ZipCodeDeclaration>());

		public static readonly DependencyProperty DeliveryScheduleProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, ZipCodeDeliverySchedule>());



		public ZipCodeDeclaration ZipCodeDeclaration
		{
			get => (ZipCodeDeclaration) GetValue(ZipCodeDeclarationProperty);
			set => SetValue(ZipCodeDeclarationProperty, value);
		}

		public ZipCodeDeliverySchedule DeliverySchedule
		{
			get => (ZipCodeDeliverySchedule) GetValue(DeliveryScheduleProperty);
			set => SetValue(DeliveryScheduleProperty, value);
		}



		//public static readonly RoutedEvent ZipCodeGeometryPresenterMouseOverEvent
		//	= EM.Register<ZipCodeGeometryPresenter, RoutedEventHandler>(RoutingStrategy.Bubble);

		//public event RoutedEventHandler ZipCodeGeometryPresenterMouseOver
		//{
		//	add { AddHandler(ZipCodeGeometryPresenterMouseOverEvent, value); }
		//	remove { RemoveHandler(ZipCodeGeometryPresenterMouseOverEvent, value); }
		//}
		


		static ZipCodeGeometryPresenter()
		{
			ControlExtensions.OverrideStyleKey<ZipCodeGeometryPresenter>();
		}

		public ZipCodeGeometryPresenter()
		{
			DeliverySchedule = new ZipCodeDeliverySchedule(
				false,
				false,
				false,
				false,
				false,
				false,
				false);

			//Enumerable.Repeat(false, 7)
			//	.ToArray());
			MouseDown += OnMouseDown;
			MouseEnter += OnMouseEnter;
		}




		private static int brushIndex = 0;

		private static readonly List<Brush> _brushes = new List<Brush>()
		{
			Brushes.DarkRed,
			Brushes.Red,
			Brushes.OrangeRed,
			Brushes.DarkOrange,
			Brushes.Orange,
			Brushes.Yellow,
			Brushes.Chartreuse,
			Brushes.Lime,
			Brushes.Green,
			Brushes.CadetBlue,
			Brushes.Blue,
			Brushes.BlueViolet,
			Brushes.MediumPurple,
			Brushes.Purple
		};


		private void OnMouseDown(object Sender, MouseButtonEventArgs E)
		{
			PART_ZipCodeBoundaryPath.Fill = _brushes[brushIndex];
			brushIndex++;
			if (brushIndex >= _brushes.Count - 1)
				brushIndex = 1;
		}

		private void OnMouseEnter(object Sender, MouseEventArgs E)
		{
			//RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterMouseOverEvent, this));
		}



		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_ZipCodeBoundaryPath = GetTemplateChild(nameof(PART_ZipCodeBoundaryPath)).As<Path>();
		}
	}
}
