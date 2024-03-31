// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer.Unity;
using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    // ReSharper disable once UnusedType.Global
    public class FooLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>();
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, IInterface3, ConstructorWithoutInjectAttributeClass>(
                Lifetime.Singleton);
            builder.Register<ConstructorWithoutInjectAttributeClass>(_ => new ConstructorWithoutInjectAttributeClass(),
                Lifetime.Singleton);
            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton).As<IInterface1>();
            builder.RegisterInstance<ConstructorWithoutInjectAttributeClass>(
                new ConstructorWithoutInjectAttributeClass());
            builder.RegisterInstance(new ConstructorWithoutInjectAttributeClass());

            var instance = new ConstructorWithoutInjectAttributeClass();
            builder.RegisterInstance(instance);
        }
    }
}
