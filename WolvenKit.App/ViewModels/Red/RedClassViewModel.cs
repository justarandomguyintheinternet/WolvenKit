using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Documents;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.App.ViewModels.Red;

public class RedClassViewModel : ChunkViewModel
{
    private RedBaseClass _data;

    public RedClassViewModel(RedBaseClass export, RDTDataViewModel tab) : base(export, tab) {}

    public RedClassViewModel(RedBaseClass cls, ChunkViewModel parent = null, string name = null) : base(cls, parent, name)
    {
        _data = cls;
    }
}
