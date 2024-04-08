// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class RegisterConstructorWithoutInjectAttributeClassLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton).As<IInterface1>();
            builder.Register<IInterface1, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, IInterface3, ConstructorWithoutInjectAttributeClass>(
                Lifetime.Singleton);
        }
    }
}
