using System.Runtime.CompilerServices;
using Ccr.PresentationCore.Collections;

namespace UST.Spatial.Controls
{
  public sealed class ZoomStage 
    : ValueEnum<double>
  {

    public static ZoomStage Stage1 = new ZoomStage(1d);

    public static ZoomStage Stage2 = new ZoomStage(2d);

    public static ZoomStage Stage3 = new ZoomStage(3d);


    private ZoomStage(
      double value,
      [CallerMemberName] string fieldName = "", 
      [CallerLineNumber] int line = 0) 
        : base(
          value, 
          fieldName,
          line)
    {
    }

    public override bool Equals(object obj)
    {
      if (obj is ZoomStage zoomStage)
      {
        return Name == zoomStage.Name;
      }

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}