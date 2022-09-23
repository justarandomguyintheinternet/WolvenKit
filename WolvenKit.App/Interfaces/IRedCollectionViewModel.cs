using CommunityToolkit.Mvvm.Input;

namespace WolvenKit.App.Interfaces;

public interface IRedCollectionViewModel
{
    public IRelayCommand AddItemToCollectionCommand { get; }
    public IRelayCommand DeleteAllFromCollectionCommand { get; }
}
