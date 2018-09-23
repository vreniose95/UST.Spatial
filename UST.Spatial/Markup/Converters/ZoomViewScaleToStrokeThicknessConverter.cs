using Ccr.Xaml.Markup.Converters.Infrastructure;

namespace UST.Spatial.Markup.Converters
{
  public class ZoomViewScaleToStrokeThicknessConverter
    : XamlConverter<
      double,
      double,
      double,
      ConverterParam<double>,
      double>
  {
    public override double Convert(
      double zoomScale,
      double minZoomScale,
      double maxZoomScale,
      ConverterParam<double> param)
    {
      var normalizedTargetStrokeThickness = param.Value;
      var zoomRange = maxZoomScale - minZoomScale;

      var progressionThroughZoomRange = zoomScale / zoomRange;

      var targetRangeMin = 0.1;
      var targetRangeMax= 1;

      var targetRangeSize = targetRangeMax - targetRangeMin;

      var adjustmentMultiplier = progressionThroughZoomRange * targetRangeSize;
      var adjustmentSubtractiveActual = targetRangeMax * adjustmentMultiplier;
      var finalActualStrokeThickness = normalizedTargetStrokeThickness - adjustmentSubtractiveActual;

      return finalActualStrokeThickness;

    }
  }
}
