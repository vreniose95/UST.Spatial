using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using Ccr.Core.Extensions;
namespace UST.Spatial.GeoJSON
{
  [DebuggerDisplay("ToString()")]
  public class ZipCodeDeliverySchedule
  {
    private static readonly IDictionary<DayOfWeek, Expression<Func<ZipCodeDeliverySchedule, bool>>> _mappings
      = new Dictionary<DayOfWeek, Expression<Func<ZipCodeDeliverySchedule, bool>>>
      {
        [DayOfWeek.Sunday] = t => t.Sunday,
        [DayOfWeek.Monday] = t => t.Monday,
        [DayOfWeek.Tuesday] = t => t.Tuesday,
        [DayOfWeek.Wednesday] = t => t.Wednesday,
        [DayOfWeek.Thursday] = t => t.Sunday,
        [DayOfWeek.Friday] = t => t.Friday,
        [DayOfWeek.Saturday] = t => t.Saturday
      };

    /// <summary>
    ///   Indicates whether delivery should occur on Sunday.
    /// </summary>
    public bool Sunday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Monday.
    /// </summary>
    public bool Monday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Tuesday.
    /// </summary>
    public bool Tuesday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Wednesday.
    /// </summary>
    public bool Wednesday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Thursday.
    /// </summary>
    public bool Thursday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Friday.
    /// </summary>
    public bool Friday { get; }

    /// <summary>
    ///   Indicates whether delivery should occur on Saturday.
    /// </summary>
    public bool Saturday { get; }


    /// <summary>
    ///   Creates an instance of the <see cref="ZipCodeDeliverySchedule"/> class.
    /// </summary>
    /// <param name="sunday">
    ///   A <see cref="bool"/> value indicating whether or not Sunday delivery is scheduled.
    /// </param>
    /// <param name="monday">
    ///   A <see cref="bool"/> value indicating whether or not Monday delivery is scheduled.
    /// </param>
    /// <param name="tuesday">
    ///   A <see cref="bool"/> value indicating whether or not Tuesday delivery is scheduled.
    /// </param>
    /// <param name="wednesday">
    ///   A <see cref="bool"/> value indicating whether or not Wednesday delivery is scheduled.
    /// </param>
    /// <param name="thursday">
    /// A <see cref="bool"/> value indicating whether or not Thursday delivery is scheduled.
    /// </param>
    /// <param name="friday">
    /// A <see cref="bool"/> value indicating whether or not Friday delivery is scheduled.
    /// </param>
    /// <param name="saturday">
    /// A <see cref="bool"/> value indicating whether or not Saturday delivery is scheduled.
    /// </param>
    public ZipCodeDeliverySchedule(
      bool sunday,
      bool monday,
      bool tuesday,
      bool wednesday,
      bool thursday,
      bool friday,
      bool saturday)
    {
      Sunday = sunday;
      Monday = monday;
      Tuesday = tuesday;
      Wednesday = wednesday;
      Thursday = thursday;
      Friday = friday;
      Saturday = saturday;
    }

    public ZipCodeDeliverySchedule(
      params bool[] deliveryDays)
    {
      if (deliveryDays.Length != _mappings.Count)
        throw new NotSupportedException();

      foreach (var mapping in _mappings)
      {
        var setterExpression = mapping
          .Value
          .ToSetterExpression();

        setterExpression.Compile()(this, true);
      }
    }


    public bool ShouldDeliverOnDay(
      DayOfWeek dayOfWeek)
    {
      if (!_mappings.TryGetValue(dayOfWeek, out var func))
        throw new KeyNotFoundException(
          $"There is no declared \'DayOfWeek\' entry in the dictionary.");

      var result = func.Compile()(this);
      return result;
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

      if (Thursday)
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