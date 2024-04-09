using VContainer;
using VContainer.Unity;

namespace Sandbox
{
    public class SandboxLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>();
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>(Lifetime.Scoped);
            builder.RegisterEntryPoint<ConstructorWithInjectAttributeClass>();
            builder.RegisterEntryPoint<ConstructorWithInjectAttributeClass>(Lifetime.Scoped);

            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ConstructorWithoutInjectAttributeClass>();
                entryPoints.Add<ConstructorWithInjectAttributeClass>();
            });

            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton).As<IInterface1>();
            builder.Register<IInterface1, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, IInterface3, ConstructorWithoutInjectAttributeClass>(
                Lifetime.Singleton);

            builder.Register<ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<ConstructorWithInjectAttributeClass>(Lifetime.Singleton).As<IInterface1>();
            builder.Register<IInterface1, ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, IInterface3, ConstructorWithInjectAttributeClass>(
                Lifetime.Singleton);

            builder.RegisterEntryPoint<DefaultConstructorClass>();
            builder.Register<DefaultConstructorClass>(Lifetime.Singleton);

            builder.RegisterEntryPoint<NoConstructorClass>();
            builder.Register<IInterface1, NoConstructorClass>(Lifetime.Scoped);
        }
    }
}
