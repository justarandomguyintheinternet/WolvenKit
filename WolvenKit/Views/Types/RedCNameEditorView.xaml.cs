using System;
using System.Collections.Generic;
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
using WolvenKit.App.Interfaces;
using WolvenKit.App.ViewModels.Red;
using WolvenKit.Helpers;
using WolvenKit.RED4.Types;

namespace WolvenKit.Views.Types;
/// <summary>
/// Interaktionslogik für RedCNameEditorView.xaml
/// </summary>
public partial class RedCNameEditorView
{
    public RedCNameEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            if (ViewModel is { Parent: IRedCollectionViewModel })
            {

            }
        });
    }
}
