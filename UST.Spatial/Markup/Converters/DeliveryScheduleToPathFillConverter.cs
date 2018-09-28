using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Ccr.Core.Extensions;
using Ccr.MaterialDesign;
using Ccr.MaterialDesign.Primitives.Behaviors;
using Ccr.MaterialDesign.Static;
using Ccr.Xaml.Markup.Converters.Infrastructure;
using UST.Spatial.GeoJSON;
using UST.Spatial.Models;

namespace UST.Spatial.Markup.Converters
{
	public class DeliveryScheduleIdToPathFillConverter
		: XamlConverter<
			ZipCodeDeliverySchedule,
			sbyte,
			NullParam,
			Brush>
	{
		public override Brush Convert(
			ZipCodeDeliverySchedule local,
			sbyte parentDeliveryScheduleId,
			NullParam param)
		{
			var parentProfile = ScheduleProfileRepository.I.
				FetchOrGenerateProfile(
					parentDeliveryScheduleId);

			var id = (Day)local.ScheduleConfigurationId;
			var appendId = (Day) parentProfile.DeliverySchedule.ScheduleConfigurationId;
			var newId = id | appendId;

			var combinedProfiles = ScheduleProfileRepository.I.
				FetchOrGenerateProfile(
					(sbyte)newId);

			return combinedProfiles.ProfileBrush;
		}
	}

	public static class EnumerableExtensions
	{
		public static IEnumerable<sbyte> Range(
			sbyte start,
			sbyte end)
		{
			if (start >= end)
				throw new ArgumentOutOfRangeException(nameof(end));

			return RangeIterator(start, end);
		}

		private static IEnumerable<sbyte> RangeIterator(
			sbyte start,
			sbyte end)
		{
			var current = start;
			while (current <= end)
			{
				yield return current;

				if (current != sbyte.MaxValue)
					current++;
			}
		}
	}

	public class ScheduleProfileRepository
	{
		#region Singleton
		protected static ScheduleProfileRepository _i;
		public static ScheduleProfileRepository I
		{
			get => _i ??
						 (_i = new ScheduleProfileRepository());
		}
		#endregion


		private readonly SortedDictionary<sbyte, NamedScheduleConfigurationProfile> _identifierToBrushMappings;
		private readonly Palette _palette;

		private ScheduleProfileRepository()
		{
			//var range = EnumerableExtensions.Range(sbyte.MinValue, sbyte.MaxValue).ToArray();

			//var brushDictionary = range
			//	.ToDictionary(
			//		t => t,
			//		t => (NamedScheduleConfigurationProfile)null);
			var brushDictionary = new Dictionary<sbyte, NamedScheduleConfigurationProfile>(256);

			_identifierToBrushMappings = new SortedDictionary<sbyte, NamedScheduleConfigurationProfile>(
				brushDictionary,
				Comparer<sbyte>.Default);

			_palette = GlobalResources.Palette;
		}


		public void UnregisterProfile(sbyte identifier)
		{
			if (_identifierToBrushMappings.ContainsKey(identifier))
			{
				_identifierToBrushMappings[identifier] = null;
			}
		}

		public NamedScheduleConfigurationProfile FetchOrGenerateProfile(
			sbyte identifier)
		{
			if (_identifierToBrushMappings.ContainsKey(identifier))
			{
				var cachedValue = _identifierToBrushMappings[identifier];

				if (cachedValue != null)
					return cachedValue;

				var newBrush = GenerateBrushFromProfileId(identifier);

				var newProfile = new NamedScheduleConfigurationProfile(
					identifier,
					"Test",
					newBrush);

				_identifierToBrushMappings[identifier] = newProfile;
				return newProfile;
			}
			else
			{
				var newBrush = GenerateBrushFromProfileId(identifier);

				var newProfile = new NamedScheduleConfigurationProfile(
					identifier,
					"Test",
					newBrush);

				_identifierToBrushMappings.Add(identifier, newProfile);
				return newProfile;
			}
			throw new Exception();
		}

		public Brush GenerateBrushFromProfileId(
			sbyte identifier)
		{
			var swatchCount = _palette.Swatches.Count;

			var quotient = ((int)identifier)
				.DivRem(
					swatchCount,
					out var remainder);

			var swatch = _palette.Swatches[remainder];

			var materialBrush = swatch.Materials.ElementAt(quotient);

			return materialBrush.Brush;
		}


	}
}
