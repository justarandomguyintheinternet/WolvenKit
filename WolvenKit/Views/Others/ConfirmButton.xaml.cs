using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WolvenKit.Functionality.Commands;

namespace WolvenKit.Views.Others;
/// <summary>
/// Interaktionslogik für ConfirmButton.xaml
/// </summary>
public partial class ConfirmButton : UserControl
{
    public ConfirmButton()
    {
        InitializeComponent();

        ToggleButton.Click += OnClick;
    }

    /// <summary>
    /// Get or set the Command property
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Identifies the <see cref="Text"/> dependency property.</summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(ConfirmButton),
            new FrameworkPropertyMetadata("Delete"));

    /// <summary>
    /// Get or set the Command property
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Identifies the <see cref="Command"/> dependency property.</summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ConfirmButton),
            new FrameworkPropertyMetadata((ICommand)null));

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton { IsChecked: true })
        {
            ToggleButtonTextBlock.SetCurrentValue(TextBlock.TextProperty, "Confirm?");
        }
        else
        {
            Command?.SafeExecute();

            ToggleButtonTextBlock.SetCurrentValue(TextBlock.TextProperty, Text);
        }
    }
}
