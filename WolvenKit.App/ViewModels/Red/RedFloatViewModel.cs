using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.App.ViewModels.Red;

public class RedFloatViewModel : ChunkViewModel
{
    public RedFloatViewModel(CFloat cFloat, ChunkViewModel parent = null, string name = null, bool lazy = false, bool isReadOnly = false) : base(cFloat, parent, name, lazy, isReadOnly)
    {
    }
}
