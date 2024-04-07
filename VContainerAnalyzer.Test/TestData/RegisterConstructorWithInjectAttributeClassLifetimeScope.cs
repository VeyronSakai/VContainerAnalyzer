// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class RegisterConstructorWithInjectAttributeClassLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.Register<ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<ConstructorWithInjectAttributeClass>(Lifetime.Singleton).As<IInterface1>();
            builder.Register<IInterface1, ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, ConstructorWithInjectAttributeClass>(Lifetime.Singleton);
            builder.Register<IInterface1, IInterface2, IInterface3, ConstructorWithInjectAttributeClass>(
                Lifetime.Singleton);
        }
    }
}
