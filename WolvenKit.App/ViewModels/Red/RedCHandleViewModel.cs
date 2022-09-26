using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.App.ViewModels.Red;

public class RedCHandleViewModel : ChunkViewModel
{
    public RedCHandleViewModel(IRedBaseHandle redHandle, ChunkViewModel parent = null, string name = null, bool lazy = false, bool isReadOnly = false) : base(redHandle, parent, name, lazy, isReadOnly)
    {
    }
}
