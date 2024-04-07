// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer.Unity;
using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    // ReSharper disable once UnusedType.Global
    public class RegisterEntryPointConstructorWithoutInjectAttributeClassLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>();
            builder.RegisterEntryPoint<ConstructorWithoutInjectAttributeClass>(Lifetime.Singleton);
        }
    }
}
