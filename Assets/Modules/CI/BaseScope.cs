using IsoRush;
using IsoRush.Player;
using IsoRush.State;
using IsoRush.Utils;
using VContainer;
using VContainer.Unity;

public class BaseScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.RegisterComponentInHierarchy<PlayerController>().AsImplementedInterfaces().AsSelf();
    }
}
