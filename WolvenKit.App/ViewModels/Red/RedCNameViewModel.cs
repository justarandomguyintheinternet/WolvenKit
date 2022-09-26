using ReactiveUI.Fody.Helpers;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.App.ViewModels.Red;

public class RedCNameViewModel : ChunkViewModel
{
    [Reactive] public string Text { get; set; }

    public RedCNameViewModel(CName cName, ChunkViewModel parent = null, string name = null, bool lazy = false, bool isReadOnly = false) : base(cName, parent, name, lazy, isReadOnly)
    {
    }
}
