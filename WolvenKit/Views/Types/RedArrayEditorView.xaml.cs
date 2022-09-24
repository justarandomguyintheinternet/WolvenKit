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
/// Interaktionslogik für RedArrayEditorView.xaml
/// </summary>
public partial class RedArrayEditorView
{
    public RedArrayEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, viewModel => viewModel.AddItemToCollectionCommand, view => view.AddItemToCollectionButton)
                .DisposeWith(disposables);

            // this.BindCommand(ViewModel, viewModel => viewModel.DeleteAllFromCollectionCommand, view => view.DeleteAllItemsConfirmButton)
            //     .DisposeWith(disposables);

            this.OneWayBind(ViewModel, viewModel => viewModel.DisplayProperties, view => view.ItemsListView.ItemsSource)
                .DisposeWith(disposables);
        });
    }
}
