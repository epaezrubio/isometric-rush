using IsoRush;
using IsoRush.State;
using VContainer;
using VContainer.Unity;

public class BaseScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameState>(Lifetime.Singleton);
    }
}
