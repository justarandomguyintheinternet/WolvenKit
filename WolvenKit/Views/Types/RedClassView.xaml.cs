using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;

namespace WolvenKit.Views.Types;
/// <summary>
/// Interaktionslogik für RedClassView.xaml
/// </summary>
public partial class RedClassView
{
    public RedClassView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, viewModel => viewModel.DisplayProperties, view => view.PropertiesListBox.ItemsSource)
                .DisposeWith(disposables);
        });
    }
}
