using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Documents;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.App.ViewModels.Red;

public class RedClassViewModel : ChunkViewModel
{
    public RedClassViewModel(RedBaseClass export, RDTDataViewModel tab) : base(export, tab) {}

    public RedClassViewModel(RedBaseClass redArray, ChunkViewModel parent = null, string name = null, bool lazy = false, bool isReadOnly = false) : base(redArray, parent, name, lazy, isReadOnly)
    {
    }
}
