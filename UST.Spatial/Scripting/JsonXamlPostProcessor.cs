using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using UST.Spatial.GeoJSON;

namespace UST.Spatial.Scripting
{
  public class JsonXamlPostProcessor
  {
    public static void ProcessFile(
      FileInfo xamlFileInfo,
      FileInfo jsonFileInfo)
    {
      if (xamlFileInfo.Extension == ".xaml")
        throw new NotSupportedException();

      if (jsonFileInfo.Extension == ".json")
        throw new NotSupportedException();

      using (var reader = xamlFileInfo.OpenRead())
      {

        using (var xmlReader = XmlReader.Create(reader, new XmlReaderSettings()))
        {
          while (xmlReader.ReadToFollowing("Geometry"))
          {
            var pathMarkup = xmlReader.ReadContentAsString();
          }

        }
      }
    }

    public static Regex _jsonZipCodeRegex = new Regex(
      @"""ZCTA5CE10""[\s]*:[\s]*""(?<ZipCode>[0-9]{5})""");



    public static void ProcessSVGToXaml(
      FileInfo inputSvgFile,
      State state)
    {
      if (!inputSvgFile.Exists || inputSvgFile.Extension != ".svg")
        throw new NotSupportedException();

      var targetOutputPath = inputSvgFile.Directory.Parent.FullName + @"\XAML\"
                             + $@"\{state.Value.Abbreviation.ToLower()}.xaml";

      var targetOutputFileInfo = new FileInfo(targetOutputPath);

      if (targetOutputFileInfo.Exists || targetOutputFileInfo.Extension != ".xaml")
        throw new NotSupportedException();

      var expectedJsonFileName =
        $"{state.Value.Abbreviation}_{state.Value.StateName.Replace(" ", "_")}_zip_codes_geo.min"
          .ToLower();

      var jsonZipSource = new FileInfo(
        inputSvgFile.Directory.Parent.FullName + $@"\JSON\{expectedJsonFileName}.json");

      var extractedZipCodesInState = ExtractJsonZipCodes(
          jsonZipSource)
        .ToArray();

      using (var streamWriter = targetOutputFileInfo.CreateText())
      {
        streamWriter.WriteLine(
          "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");

        streamWriter.WriteLine(
          "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");

        streamWriter.WriteLine(
          "xmlns:geo=\"clr-namespace:UST.Spatial.GeoJSON\">");

        //streamWriter.WriteLine(
        //  "xmlns:po=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/options\">");

        streamWriter.WriteLine(
          $"\t<geo:StateGeometry x:Key=\"Maps.{state.Value.Abbreviation.ToUpper()}\">");

        var index = 0;

        using (var fileReader = inputSvgFile.OpenRead())
        {
          using (var xmlReader = XmlReader.Create(fileReader, new XmlReaderSettings()))
          {
            xmlReader.ReadToFollowing("path");
            while (xmlReader.Read())
            {
              if (xmlReader.IsStartElement())
              {
                var path = xmlReader["d"];

                int pairedZip;

                if (index > extractedZipCodesInState.Length - 1)
                {
                  pairedZip = -1;
                }
                else
                {
                  pairedZip = extractedZipCodesInState[index];
                }

                streamWriter.WriteLine(
                  $"\t\t<geo:ZipCodeGeometry ZipCode=\"{pairedZip}\" Geometry=\"{path}\"/>");

                index++;
              }
            }
          }
        }

        streamWriter.WriteLine(
          $"\t</geo:StateGeometry>");

        streamWriter.WriteLine(
          $"</ResourceDictionary>");
      }
    }


    public static void ProcessSVGToXalFreezable(
      FileInfo inputSvgFile,
      State state)
    {
      if (!inputSvgFile.Exists || inputSvgFile.Extension != ".svg")
        throw new NotSupportedException();

      var targetOutputPath = inputSvgFile.Directory.CreateSubdirectory("SVG_2").FullName
                             + $@"\{state.Value.Abbreviation.ToLower()}.xaml";

      var targetOutputFileInfo = new FileInfo(targetOutputPath);

      if (targetOutputFileInfo.Exists || targetOutputFileInfo.Extension != ".xaml")
        throw new NotSupportedException();

      var jsonZipSource = new FileInfo(
        inputSvgFile.Directory.FullName + $@"\{state.Value.Abbreviation.ToLower()}.json");

      var extractedZipCodesInState = ExtractJsonZipCodes(
          jsonZipSource)
        .ToArray();

      using (var streamWriter = targetOutputFileInfo.CreateText())
      {
        streamWriter.WriteLine(
          "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");

        streamWriter.WriteLine(
          "\t\t\txmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");

        streamWriter.WriteLine(
          "\t\t\txmlns:geo=\"clr-namespace:UST.Spatial.GeoJSON\"");

        streamWriter.WriteLine(
          "\t\t\txmlns:po=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/options\">");

        streamWriter.WriteLine(
          $"\t<GeometryGroup x:Key=\"Maps.{state.Value.Abbreviation.ToUpper()}\" po:Freeze=\"True\">");

        var index = 0;

        using (var fileReader = inputSvgFile.OpenRead())
        {
          using (var xmlReader = XmlReader.Create(fileReader, new XmlReaderSettings()))
          {
            xmlReader.ReadToFollowing("path");
            while (xmlReader.Read())
            {
              if (xmlReader.IsStartElement())
              {
                var path = xmlReader["d"];

                int pairedZip;

                if (index > extractedZipCodesInState.Length - 1)
                {
                  pairedZip = -1;
                }
                else
                {
                  pairedZip = extractedZipCodesInState[index];
                }

                streamWriter.WriteLine(
                  $"\t\t<Geometry po:Freeze=\"True\">{path}</Geometry>");

                index++;
              }
            }
          }
        }

        streamWriter.WriteLine(
          $"\t</GeometryGroup>");

        streamWriter.WriteLine(
          $"</ResourceDictionary>");
      }
    }


    public static IEnumerable<int> ExtractJsonZipCodes(
      FileInfo inputJsonFile)
    {
      if (!inputJsonFile.Exists || inputJsonFile.Extension != ".json")
        throw new NotSupportedException();

      using (var fileReader = inputJsonFile.OpenRead())
      {
        using (var streamReader = new StreamReader(fileReader))
        {
          var line = streamReader.ReadLine();

          while (line != null)
          {
            if (_jsonZipCodeRegex.IsMatch(line))
            {
              var matches = _jsonZipCodeRegex.Matches(line);
              foreach (Match match in matches)
              {
                if (matches.Count > 1)
                {

                }
                var zipCodeStr = match.Groups["ZipCode"].ToString();

                yield return int.Parse(zipCodeStr);

              }
            }
            line = streamReader.ReadLine();
          }
        }
      }
    }

  }
}