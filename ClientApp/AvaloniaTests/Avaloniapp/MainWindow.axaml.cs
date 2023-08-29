using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System;

namespace Avaloniapp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ButtonClicked (object source, RoutedEventArgs args)
    {
        Console.WriteLine("Click");
    }
}