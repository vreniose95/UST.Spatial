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
using UST.Spatial.Views;
using Xceed.Wpf.AvalonDock.Controls;


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

		public static readonly DependencyProperty DeliveryConfigurationCurrentlyPaintingIdProperty = DP.Register(
			new Meta<ZipCodeGeometryPresenter, sbyte>());

		public sbyte DeliveryConfigurationCurrentlyPaintingId
		{
			get => (sbyte) GetValue(DeliveryConfigurationCurrentlyPaintingIdProperty);
			set => SetValue(DeliveryConfigurationCurrentlyPaintingIdProperty, value);
		}
		 
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


		public static readonly RoutedEvent ZipCodeGeometryPresenterMouseOverEvent
			= EM.Register<ZipCodeGeometryPresenter, RoutedEventHandler>(RoutingStrategy.Bubble);

		public event RoutedEventHandler ZipCodeGeometryPresenterMouseOver
		{
			add { AddHandler(ZipCodeGeometryPresenterMouseOverEvent, value); }
			remove { RemoveHandler(ZipCodeGeometryPresenterMouseOverEvent, value); }
		}


		public static readonly RoutedEvent ZipCodeGeometryPresenterMouseLeaveEvent
			= EM.Register<ZipCodeGeometryPresenter, RoutedEventHandler>(RoutingStrategy.Bubble);

		public event RoutedEventHandler ZipCodeGeometryPresenterMouseLeave
		{
			add { AddHandler(ZipCodeGeometryPresenterMouseLeaveEvent, value); }
			remove { RemoveHandler(ZipCodeGeometryPresenterMouseLeaveEvent, value); }
		}


		public static readonly RoutedEvent ZipCodeGeometryPresenterClickedEvent
			= EM.Register<ZipCodeGeometryPresenter, RoutedEventHandler>(RoutingStrategy.Bubble);

		public event RoutedEventHandler ZipCodeGeometryPresenterClicked
		{
			add { AddHandler(ZipCodeGeometryPresenterClickedEvent, value); }
			remove { RemoveHandler(ZipCodeGeometryPresenterClickedEvent, value); }
		}



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
			MouseUp += OnMouseUp;
			MouseEnter += OnMouseEnter;
			MouseLeave += OnMouseLeave;
			//Loaded += (s, Args) =>
			//{
			//	DeliveryConfigurationCurrentlyPaintingId = this.FindVisualAncestor<MainWindow>()
			//		.DataContext
			//		.As<MainViewModel>()
			//		.BitwiseId;
			//};
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


		private void OnMouseDown(object sender, MouseButtonEventArgs E)
		{
			//CaptureMouse();
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterClickedEvent, this));
			//ReleaseMouseCapture();
		}

		private void OnMouseLeave(object sender, MouseEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterMouseLeaveEvent, this));
		}

		private void OnMouseEnter(object sender, MouseEventArgs E)
		{
			RaiseEvent(new RoutedEventArgs(ZipCodeGeometryPresenterMouseOverEvent, this));
		}



		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_ZipCodeBoundaryPath = GetTemplateChild(nameof(PART_ZipCodeBoundaryPath)).As<Path>();
		}
	}
}
