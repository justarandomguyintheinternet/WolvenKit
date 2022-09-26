using System;
using ReactiveUI;

namespace WolvenKit.Helpers;

public class RedViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null)
    {
        var defaultViewLocator = ViewLocator.Current;
        if (defaultViewLocator == null)
        {
            throw new Exception();
        }

        var view = defaultViewLocator.ResolveView(viewModel, contract);
        if (view != null)
        {
            return view;
        }

        if (contract != null)
        {
            return defaultViewLocator.ResolveView(viewModel);
        }

        return null;
    }
}
