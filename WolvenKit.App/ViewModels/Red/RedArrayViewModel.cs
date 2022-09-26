using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using WolvenKit.App.Interfaces;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Dialogs;
using WolvenKit.ViewModels.Shell;
using static WolvenKit.ViewModels.Dialogs.DialogViewModel;

namespace WolvenKit.App.ViewModels.Red;

public partial class RedArrayViewModel : ChunkViewModel, IRedCollectionViewModel
{
    private IRedArray CastedData => (IRedArray)Data;

    // [Reactive] public new ObservableCollectionExtended<ChunkViewModel> DisplayProperties { get; set; } = new();

    public RedArrayViewModel() : base()
    {

    }

    public RedArrayViewModel(IRedArray redArray, ChunkViewModel parent = null, string name = null, bool lazy = false, bool isReadOnly = false) : base(redArray, parent, name, lazy, isReadOnly)
    {
        // RefreshDisplayProperties();
    }

    private void RefreshDisplayProperties()
    {
        var disposable = DisplayProperties.SuspendNotifications();

        DisplayProperties.Clear();
        for (int i = 0; i < CastedData.Count; i++)
        {
            if (CastedData[i] is not IRedType redItem)
            {
                throw new Exception();
            }

            DisplayProperties.Add(CreateChunkViewModel(redItem));
        }

        disposable.Dispose();
    }

    [RelayCommand]
    private void AddItemToCollection()
    {
        if (CastedData == null)
        {
            // TODO: Need info for CStatic, ...
            return;
        }

        var innerType = CastedData.InnerType;
        DialogHandlerDelegate handler = HandleChunk;
        if (innerType.IsAssignableTo(typeof(IRedBaseHandle)))
        {
            handler = HandleChunkPointer;
            innerType = innerType.GenericTypeArguments[0];
        }
        else if (innerType.IsGenericType) // handles CResoruceReference<>, etc
        {
            innerType = innerType.GetGenericTypeDefinition();
        }
        var existing = new ObservableCollection<string>(AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => innerType.IsAssignableFrom(p) && p.IsClass)
            .Select(x => x.Name));

        // no inheritable
        if (existing.Count == 1)
        {
            var type = CastedData.InnerType;
            if (type == typeof(CKeyValuePair))
            {
                var app = Locator.Current.GetService<AppViewModel>();
                app.SetActiveDialog(new SelectRedTypeDialogViewModel
                {
                    DialogHandler = HandleCKeyValuePair
                });

                return;
            }

            var newItem = RedTypeManager.CreateRedType(type);
            if (newItem is IRedBaseHandle handle)
            {
                var pointee = RedTypeManager.CreateRedType(handle.InnerType);
                handle.SetValue((RedBaseClass)pointee);
            }
            InsertChild(-1, newItem);
        }
        else
        {
            var app = Locator.Current.GetService<AppViewModel>();
            app.SetActiveDialog(new CreateClassDialogViewModel(existing, true)
            {
                DialogHandler = handler
            });
        }

        // RefreshDisplayProperties();
    }

    [RelayCommand]
    private void DeleteAllFromCollection()
    {

    }
}
