using Avalonia.Controls;
using Avalonia.Input;
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
        if (double.TryParse(textBoxCelsius.Text, out var celsius))
        {
            double f = celsius * (9d/5d) + 32;
            textBoxFahrenheit.Text = f.ToString("0.00");
        }
        else
        {
            Console.WriteLine("Error in data submission");
            textBoxCelsius.Text = "0";
            textBoxFahrenheit.Text = "0";
        }
    }

    public void InputEntered(object source, TextInputEventArgs args)
    {
        Console.WriteLine("Entered");
        if (double.TryParse(textBoxCelsius.Text, out var celsius))
        {
            double f = celsius * (9d/5d) + 32;
            textBoxFahrenheit.Text = f.ToString("0.00");
        }
        else
        {
            Console.WriteLine("Error in data submission");
            textBoxCelsius.Text = "0";
            textBoxFahrenheit.Text = "0";
        } 
    }
}