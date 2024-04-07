// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;
using VContainer.Unity;

namespace VContainerAnalyzer.Test.TestData
{
    public class AddConstructorWithInjectAttributeClassLifetimeScope
    {
        // ReSharper disable once UnusedMember.Global
        public void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ConstructorWithInjectAttributeClass>();
            });
        }
    }
}
