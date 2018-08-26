using System;
using System.Windows;
using Ccr.PresentationCore.Helpers.DependencyHelpers;

namespace UST.Spatial.GeoJSON
{
  public static class ZipSpatialInfo
  {
    private static readonly Type _type = typeof(ZipSpatialInfo);


    public static readonly DependencyProperty ZipCodeProperty = DP.Attach(
      _type, new MetaBase<int>());


    public static int GetZipCode(DependencyObject @this)
    {
      return @this.Get<int>(ZipCodeProperty);
    }
    public static void SetZipCode(DependencyObject @this, int value)
    {
      @this.Set(ZipCodeProperty, value);
    }


    public static readonly DependencyProperty GEOID10Property = DP.Attach(
      _type, new MetaBase<int>());


    public static int GetGEOID10(DependencyObject @this)
    {
      return @this.Get<int>(GEOID10Property);
    }
    public static void SetGEOID10(DependencyObject @this, int value)
    {
      @this.Set(GEOID10Property, value);
    }


    public static readonly DependencyProperty CLASSFP10Property = DP.Attach(
      _type, new MetaBase<string>());


    public static string GetCLASSFP10(DependencyObject @this)
    {
      return @this.Get<string>(CLASSFP10Property);
    }
    public static void SetCLASSFP10(DependencyObject @this, string value)
    {
      @this.Set(CLASSFP10Property, value);
    }


    public static readonly DependencyProperty INTPTLAT10Property = DP.Attach(
      _type, new MetaBase<double>());


    public static double GetINTPTLAT10(DependencyObject @this)
    {
      return @this.Get<double>(INTPTLAT10Property);
    }
    public static void SetINTPTLAT10(DependencyObject @this, double value)
    {
      @this.Set(INTPTLAT10Property, value);
    }


    public static readonly DependencyProperty INTPTLON10Property = DP.Attach(
      _type, new MetaBase<double>());


    public static double GetINTPTLON10(DependencyObject @this)
    {
      return @this.Get<double>(INTPTLON10Property);
    }
    public static void SetINTPTLON10(DependencyObject @this, double value)
    {
      @this.Set(INTPTLON10Property, value);
    }
  }
}
