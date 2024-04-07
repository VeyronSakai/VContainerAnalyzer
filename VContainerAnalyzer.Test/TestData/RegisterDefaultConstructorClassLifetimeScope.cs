// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class RegisterDefaultConstructorClassLifetimeScope
    {
        public void Configure(IContainerBuilder builder)
        {
            builder.Register<DefaultConstructorClass>(Lifetime.Singleton);
        }
    }
}
