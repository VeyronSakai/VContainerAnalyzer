// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

// ReSharper disable once CheckNamespace

namespace VContainer.Unity
{
    public static class ContainerBuilderUnityExtensions
    {
        public static RegistrationBuilder RegisterEntryPoint<T>(this IContainerBuilder builder,
            Lifetime lifetime = Lifetime.Singleton)
        {
            return new RegistrationBuilder();
        }
    }
}
