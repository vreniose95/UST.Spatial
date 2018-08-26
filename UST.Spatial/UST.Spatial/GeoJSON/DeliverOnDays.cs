using System.Diagnostics;
using System.Text;

namespace UST.Spatial.GeoJSON
{
  [DebuggerDisplay("ToString()")]
  public class DeliverOnDays
  {
    public bool Sunday { get; }

    public bool Monday { get; }

    public bool Tuesday { get; }

    public bool Wednesday { get; }

    public bool Thurday { get; }

    public bool Friday { get; }

    public bool Saturday { get; }


    public DeliverOnDays(
      bool sunday,
      bool monday,
      bool tuesday,
      bool wednesday,
      bool thurday,
      bool friday,
      bool saturday)
    {
      Sunday = sunday;
      Monday = monday;
      Tuesday = tuesday;
      Wednesday = wednesday;
      Thurday = thurday;
      Friday = friday;
      Saturday = saturday;
    }


    public override string ToString()
    {
      var sb = new StringBuilder();

      if (Sunday)
        sb.Append("Sunday, ");

      if (Monday)
        sb.Append("Monday, ");

      if (Tuesday)
        sb.Append("Tuesday, ");

      if (Wednesday)
        sb.Append("Wednesday, ");

      if (Thurday)
        sb.Append("Thurday, ");

      if (Friday)
        sb.Append("Friday, ");

      if (Saturday)
        sb.Append("Saturday, ");

      var str = sb.ToString();

      if (str.EndsWith(", "))
        str = str.Substring(str.Length - 2);

      return str;
    }
  }
}