using System.Reactive.Disposables;
using ReactiveUI;
using WolvenKit.App.ViewModels.Red;
using WolvenKit.Helpers;

namespace WolvenKit.Views.Types;
/// <summary>
/// Interaktionslogik für RedCHandleEditorView.xaml
/// </summary>
public partial class RedCHandleEditorView
{
    public RedCHandleEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, viewModel => viewModel.DisplayProperties, view => view.PropertiesListBox.ItemsSource)
                .DisposeWith(disposables);
        });
    }
}
