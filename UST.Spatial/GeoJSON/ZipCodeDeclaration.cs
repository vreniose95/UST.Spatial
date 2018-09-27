using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Ccr.PresentationCore.Helpers.DependencyHelpers;

namespace UST.Spatial.GeoJSON
{
	[ContentProperty("Geometry")]
  public class ZipCodeDeclaration
    : DependencyObject
  {
    public static readonly DependencyProperty GeometryProperty = DP.Register(
      new Meta<ZipCodeDeclaration, Geometry>());

    public static readonly DependencyProperty ZipCodeProperty = DP.Register(
      new Meta<ZipCodeDeclaration, int>());

		
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
