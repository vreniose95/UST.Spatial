using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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
     // Loaded += OnLoaded;
    }

    private static readonly string folderRoot = @"C:\Users\Eric\Desktop\Maps\SVG\";

    private void OnLoaded(object Sender, RoutedEventArgs E)
    {
      JsonXamlPostProcessor.ProcessSVGToXaml(
        new FileInfo($"{folderRoot}tx.svg"),
        State.Texas);
    }
  }
}
