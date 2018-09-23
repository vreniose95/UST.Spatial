using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Ccr.PresentationCore.Collections;
using UST.Spatial.GeoJSON;
using UST.Spatial.Scripting;

namespace UST.Spatial
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      Loaded += OnLoaded;
    }

    private static readonly string folderRoot = @"C:\Users\Eric\Desktop\Maps\";

    private void OnLoaded(object Sender, RoutedEventArgs E)
    {
      var rootDirectoryInfo = new DirectoryInfo(folderRoot);

      var svgDirectoryInfo = new DirectoryInfo(folderRoot + @"SVG\");
      var jsonDirectoryInfo = new DirectoryInfo(folderRoot + @"JSON\");
      var xamlDirectoryInfo = new DirectoryInfo(folderRoot + @"XAML\");

      foreach (var svgFile in svgDirectoryInfo.GetFiles())
      {
        var stateAbbreviation = svgFile.Name.Substring(0, 2);
        var state = ValueEnum.ParseSelect<State, string>(
          t => t.Value.Abbreviation,
          stateAbbreviation,
          true);

        JsonXamlPostProcessor.ProcessSVGToXaml(
          svgFile,
          state);

      }


    }
  }
}
