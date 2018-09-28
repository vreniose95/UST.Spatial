using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ccr.Core.Extensions;
using Ccr.PresentationCore.Helpers;
using Ccr.PresentationCore.Helpers.DependencyHelpers;
using Ccr.PresentationCore.Helpers.EventHelpers;
using UST.Spatial.GeoJSON;
using Xceed.Wpf.AvalonDock.Controls;

namespace UST.Spatial.Controls
{
	public class MapViewer
		: Control
	{
		private Canvas PART_Canvas;


		public static readonly DependencyProperty StateProperty = DP.Register(
			new Meta<MapViewer, State>(null, onStatePropertyChanged));

		private static readonly DependencyPropertyKey StateDeclarationPropertyKey = DP.RegisterReadOnly(
			new Meta<MapViewer, StateDeclaration>());

		public static readonly DependencyProperty StateDeclarationProperty
			= StateDeclarationPropertyKey.DependencyProperty;


		public static readonly DependencyProperty ZoomCenterPointProperty = DP.Register(
			new Meta<MapViewer, Point>());

		public static readonly DependencyProperty ZoomPercentageProperty = DP.Register(
			new Meta<MapViewer, double>(1));


		public static readonly DependencyProperty ZipCodeBoundaryStrokeProperty = DP.Register(
			new Meta<MapViewer, SolidColorBrush>(Brushes.Chartreuse));

		public static readonly DependencyProperty ZipCodeBoundaryStrokeThicknessProperty = DP.Register(
			new Meta<MapViewer, double>(0.25d));


		public static readonly DependencyProperty CurrentSelectionProfileIdProperty = DP.Register(
			new Meta<MapViewer, sbyte>());

		public sbyte CurrentSelectionProfileId
		{
			get => (sbyte) GetValue(CurrentSelectionProfileIdProperty);
			set => SetValue(CurrentSelectionProfileIdProperty, value);
		}

		public State State
		{
			get => (State)GetValue(StateProperty);
			set => SetValue(StateProperty, value);
		}

		public StateDeclaration StateDeclaration
		{
			get => (StateDeclaration)GetValue(StateDeclarationProperty);
			protected set => SetValue(StateDeclarationPropertyKey, value);
		}

		public Point ZoomCenterPoint
		{
			get => (Point)GetValue(ZoomCenterPointProperty);
			set => SetValue(ZoomCenterPointProperty, value);
		}

		public double ZoomPercentage
		{
			get => (double)GetValue(ZoomPercentageProperty);
			set => SetValue(ZoomPercentageProperty, value);
		}

		public SolidColorBrush ZipCodeBoundaryStroke
		{
			get => (SolidColorBrush)GetValue(ZipCodeBoundaryStrokeProperty);
			set => SetValue(ZipCodeBoundaryStrokeProperty, value);
		}

		public double ZipCodeBoundaryStrokeThickness
		{
			get => (double)GetValue(ZipCodeBoundaryStrokeThicknessProperty);
			set => SetValue(ZipCodeBoundaryStrokeThicknessProperty, value);
		}


		static MapViewer()
		{
			ControlExtensions.OverrideStyleKey<MapViewer>();

			EM.RegisterClassHandler<MapViewer, RoutedEventHandler>(
				ZipCodeGeometryPresenter.ZipCodeGeometryPresenterMouseOverEvent,
				onZipCodeGeometryPresenterMouseOver);

			EM.RegisterClassHandler<MapViewer, RoutedEventHandler>(
				ZipCodeGeometryPresenter.ZipCodeGeometryPresenterMouseLeaveEvent,
				onZipCodeGeometryPresenterMouseLeave);

			EM.RegisterClassHandler<MapViewer, RoutedEventHandler>(
				ZipCodeGeometryPresenter.ZipCodeGeometryPresenterClickedEvent,
				onZipCodeGeometryPresenterClick);

		}

		public MapViewer()
		{
			//AddHandler(MouseEnterEvent, new MouseEventHandler(onVisualDescendentMouseEnter));
			//AddHandler(MouseLeaveEvent, new MouseEventHandler(onVisualDescendentMouseLeave));
		}



		private static void onZipCodeGeometryPresenterMouseOver(object sender, RoutedEventArgs args)
		{
			if (args.OriginalSource is ZipCodeGeometryPresenter presenter)
			{
				var contentPresenter = presenter.FindVisualAncestor<ContentPresenter>();
				Panel.SetZIndex(contentPresenter, 200);
				args.Handled = true;
			}
		}

		private static void onZipCodeGeometryPresenterMouseLeave(object sender, RoutedEventArgs args)
		{
			if (args.OriginalSource is ZipCodeGeometryPresenter presenter)
			{
				var contentPresenter = presenter.FindVisualAncestor<ContentPresenter>();
				Panel.SetZIndex(contentPresenter, 100);
				args.Handled = true;
			}
		}

		private static int index = 0;

		private static List<Day[]> dayConfigs = new List<Day[]>
		{
			new []{Day.Monday, Day.Wednesday, Day.Friday},
			new []{Day.Tuesday, Day.Thursday},
			new []{Day.Saturday, Day.Sunday},
			new []{Day.Monday, Day.Tuesday, Day.Wednesday},
			new []{Day.Wednesday, Day.Thursday, Day.Friday},
			new []{Day.Friday, Day.Wednesday, Day.Monday},
			new []{Day.Monday}
		};

		private static void onZipCodeGeometryPresenterClick(object sender, RoutedEventArgs args)
		{
			if (args.OriginalSource is ZipCodeGeometryPresenter presenter)
			{
				var dayConfig = dayConfigs[index];

				presenter.DeliverySchedule.SetShouldDeliverOnDays(true, dayConfig);

				index++;
				if (index >= dayConfigs.Count)
					index = 0;
			}
		}




		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_Canvas = (Canvas)GetTemplateChild(nameof(PART_Canvas));
		}


		private static void onStatePropertyChanged(
			MapViewer @this,
			DPChangedEventArgs<State> args)
		{
			var stateDeclaration = GeoSVGReader.ReadStateDeclaration(args.NewValue);
			@this.StateDeclaration = stateDeclaration;
		}


		//protected override void OnMouseDown(MouseButtonEventArgs e)
		//{
		//  base.OnMouseDown(e);

		//  var mousePositionPx = e.GetPosition(this);

		//  var mousePositionPercentage = new Point(
		//    mousePositionPx.X / ActualWidth,
		//    mousePositionPx.Y / ActualHeight);

		//  var actualMapRenderSize = new Size(
		//    ActualWidth,
		//    ActualHeight);

		//  var theoreticalMapRenderSize = new Size(
		//    ActualWidth * ZoomPercentage,
		//    ActualHeight * ZoomPercentage);

		//  var lastZoomCenterPoint = ZoomCenterPoint;

		//  var leftClipping = lastZoomCenterPoint.X


		//  var clippedXSizePx = theoreticalMapRenderSize.Width - actualMapRenderSize.Width;
		//  var clippedYSizePx = theoreticalMapRenderSize.Height - actualMapRenderSize.Height;

		//  var clippedXLeftSizePx = clippedXSizePx * (1 - xProgression);
		//  var clippedYTopSizePx = clippedYSizePx * (1 - yProgression);

		//  var xTransformationOffsetProgression = clippedXLeftSizePx / ActualWidth;
		//  var yTransformationOffsetProgression = clippedYTopSizePx / ActualHeight;

		//  ZoomCenterPoint = new Point(
		//      xProgression,
		//      yProgression)
		//    .Push(
		//      xTransformationOffsetProgression,
		//      yTransformationOffsetProgression);

		//  ZoomStage nextZoomStage;

		//  if (ZoomStage.Equals(ZoomStage.Stage1))
		//  {
		//    nextZoomStage = ZoomStage.Stage2;
		//  }
		//  else if (ZoomStage.Equals(ZoomStage.Stage2))
		//  {
		//    nextZoomStage = ZoomStage.Stage3;
		//  }
		//  else if (ZoomStage.Equals(ZoomStage.Stage3))
		//  {
		//    nextZoomStage = ZoomStage.Stage1;
		//  }
		//  else
		//  {
		//    throw new Exception();
		//  }

		//  ZoomStage = nextZoomStage;

		//  BeginAnimation(
		//    ZoomPercentageProperty,
		//    new DoubleAnimation(
		//      nextZoomStage.Value,
		//      new Duration(TimeSpan.FromMilliseconds(600)))
		//    {
		//      EasingFunction = new CubicEase
		//      {
		//        EasingMode = EasingMode.EaseInOut
		//      }
		//    });
		//}

		//protected override void OnMouseDown(MouseButtonEventArgs e)
		//{
		//  base.OnMouseDown(e);

		//  var position = e.GetPosition(this);

		//  var xProgression = position.X / ActualWidth;
		//  var yProgression = position.Y / ActualHeight;

		//  ZoomCenterPoint = new Point(xProgression, yProgression);

		//  ZoomStage nextZoomStage;

		//  if (ZoomStage.Equals(ZoomStage.Stage1))
		//  {
		//    nextZoomStage = ZoomStage.Stage2;
		//  }
		//  else if (ZoomStage.Equals(ZoomStage.Stage2))
		//  {
		//    nextZoomStage = ZoomStage.Stage3;
		//  }
		//  else if (ZoomStage.Equals(ZoomStage.Stage3))
		//  {
		//    nextZoomStage = ZoomStage.Stage1;
		//  }
		//  else
		//  {
		//    throw new Exception();
		//  }

		//  ZoomStage = nextZoomStage;

		//  BeginAnimation(
		//    ZoomPercentageProperty,
		//    new DoubleAnimation(
		//      nextZoomStage.Value,
		//      new Duration(TimeSpan.FromMilliseconds(600)))
		//    {
		//      EasingFunction = new CubicEase
		//      {
		//        EasingMode = EasingMode.EaseInOut
		//      }
		//    });
		//}
	}
}

//private void onVisualDescendentMouseEnter(object sender, MouseEventArgs args)
//{
//if (args.OriginalSource is ZipCodeGeometryPresenter presenter)
//{
//var itemsContentPresenter = presenter.FindVisualAncestor<ContentPresenter>();
//Panel.SetZIndex(itemsContentPresenter, 200);
//args.Handled = true;
//}
//}

//private void onVisualDescendentMouseLeave(object sender, MouseEventArgs args)
//{
//if (args.OriginalSource is ZipCodeGeometryPresenter presenter)
//{
//var itemsContentPresenter = presenter.FindVisualAncestor<ContentPresenter>();
//Panel.SetZIndex(itemsContentPresenter, 100);
//args.Handled = true;
//}
//}
#region old


//private static readonly DependencyPropertyKey GeometryPropertyKey = DP.RegisterReadOnly(
//  new Meta<MapViewer, PathGeometry>());

//public static readonly DependencyProperty GeometryProperty
//  = GeometryPropertyKey.DependencyProperty;

//private static readonly DependencyPropertyKey ZoomCenterPointPropertyKey = DP.RegisterReadOnly(
//  new Meta<MapViewer, Point>());

//public static readonly DependencyProperty ZoomCenterPointProperty
//  = ZoomCenterPointPropertyKey.DependencyProperty;

//private static readonly DependencyPropertyKey ZoomPercentagePropertyKey = DP.RegisterReadOnly(
//  new Meta<MapViewer, double>());

//public static readonly DependencyProperty ZoomPercentageProperty
//  = ZoomPercentagePropertyKey.DependencyProperty;




//public PathGeometry Geometry
//{
//  get => (PathGeometry) GetValue(GeometryProperty);
//  set => SetValue(GeometryPropertyKey, value);
//}
//public Point ZoomCenterPoint
//{
//  get => (Point)GetValue(ZoomCenterPointProperty);
//  set => SetValue(ZoomCenterPointPropertyKey, value);
//}

//public double ZoomPercentage
//{
//  get => (double)GetValue(ZoomPercentageProperty);
//  set => SetValue(ZoomPercentagePropertyKey, value);
//}


#endregion
