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
/// Interaktionslogik für RedFloatEditorView.xaml
/// </summary>
public partial class RedFloatEditorView
{
    public RedFloatEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Name, v => v.RowIndexTextBlock.Text)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.XPath, v => v.RowIndexTextBlock.ToolTip)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Data, v => v.ContentTextBox.Text)
                .DisposeWith(disposables);
        });
    }
}
