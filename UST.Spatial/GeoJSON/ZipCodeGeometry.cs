using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Ccr.PresentationCore.Helpers.DependencyHelpers;

namespace UST.Spatial.GeoJSON
{
  public class StateGeometry
    : Collection<ZipCodeGeometry>
  {
    private GeometryGroup _geometryGroup;
    public GeometryGroup GeometryGroup
    {
      get => _geometryGroup
             ?? (_geometryGroup = _toGeometryGroup());
    }

    private GeometryGroup _toGeometryGroup()
    {
      return new GeometryGroup
      {
        Children = new GeometryCollection(
          this.Select(t => t.Geometry))
      };
    }
  }

  [ContentProperty("Geometry")]
  public class ZipCodeGeometry
    : DependencyObject
  {
    public static readonly DependencyProperty GeometryProperty = DP.Register(
      new Meta<ZipCodeGeometry, Geometry>());

    public static readonly DependencyProperty ZipCodeProperty = DP.Register(
      new Meta<ZipCodeGeometry, int>());



    public Geometry Geometry
    {
      get => (Geometry) GetValue(GeometryProperty);
      set => SetValue(GeometryProperty, value);
    }
    public int ZipCode
    {
      get => (int) GetValue(ZipCodeProperty);
      set => SetValue(ZipCodeProperty, value);
    }

  }
}
