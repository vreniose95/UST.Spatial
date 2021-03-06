﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Ccr.Core.Extensions;
using Ccr.PresentationCore.Helpers.DependencyHelpers;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Controls
{
  public class MapViewer
    : Control
  {


    public static readonly DependencyProperty GeometryProperty = DP.Register(
      new Meta<MapViewer, PathGeometry>());
    public PathGeometry Geometry
    {
      get { return (PathGeometry) GetValue(GeometryProperty); }
      set { SetValue(GeometryProperty, value); }
    }

    public static readonly DependencyProperty ZoomCenterPointProperty = DP.Register(
      new Meta<MapViewer, Point>());
    public Point ZoomCenterPoint
    {
      get { return (Point) GetValue(ZoomCenterPointProperty); }
      set { SetValue(ZoomCenterPointProperty, value); }
    }

    public static readonly DependencyProperty ZoomPercentageProperty = DP.Register(
      new Meta<MapViewer, double>(1));
    public double ZoomPercentage
    {
      get { return (double) GetValue(ZoomPercentageProperty); }
      set { SetValue(ZoomPercentageProperty, value); }
    }

    //private static readonly DependencyPropertyKey GeometryPropertyKey = DP.RegisterReadOnly(
    //  new Meta<MapViewer, PathGeometry>());

    //public static readonly DependencyProperty GeometryProperty
    //  = GeometryPropertyKey.DependencyProperty;


    public static readonly DependencyProperty StateProperty = DP.Register(
      new Meta<MapViewer, State>(null, onStatePropertyChanged));


    public static readonly DependencyProperty ZoomStageProperty = DP.Register(
      new Meta<MapViewer, ZoomStage>(ZoomStage.Stage1));
    public ZoomStage ZoomStage
    {
      get { return (ZoomStage) GetValue(ZoomStageProperty); }
      set { SetValue(ZoomStageProperty, value); }
    }
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

    public State State
    {
      get => (State)GetValue(StateProperty);
      set => SetValue(StateProperty, value);
    }

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



    static MapViewer()
    {
      ControlExtensions.OverrideStyleKey<MapViewer>();
    }


    private static void onStatePropertyChanged(
      MapViewer @this,
      DPChangedEventArgs<State> args)
    {
      var geometry = GeoSVGReader.Read(args.NewValue);
      var pathGeometry = PathGeometry.CreateFromGeometry(geometry);

      @this.Geometry = pathGeometry;
    }
    

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);

      var position = e.GetPosition(this);

      var xProgression = position.X / ActualWidth;
      var yProgression = position.Y / ActualHeight;

      ZoomCenterPoint = new Point(xProgression, yProgression);
      
      ZoomStage nextZoomStage;

      if (ZoomStage.Equals(ZoomStage.Stage1))
      {
        nextZoomStage = ZoomStage.Stage2;
      }
      else if (ZoomStage.Equals(ZoomStage.Stage2))
      {
        nextZoomStage = ZoomStage.Stage3;
      }
      else if (ZoomStage.Equals(ZoomStage.Stage3))
      {
        nextZoomStage = ZoomStage.Stage1;
      }
      else
      {
        throw new Exception();
      }

      ZoomStage = nextZoomStage;

      BeginAnimation(
        ZoomPercentageProperty,
        new DoubleAnimation(
          nextZoomStage.Value,
          new Duration(TimeSpan.FromMilliseconds(600)))
        {
          EasingFunction = new CubicEase
          {
            EasingMode = EasingMode.EaseInOut
          }
        });
    }
  }
}
