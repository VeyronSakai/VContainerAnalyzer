// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class RegisterNoConstructorClassLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.Register<IInterface1, NoConstructorClass>(Lifetime.Scoped);
        }
    }
}
