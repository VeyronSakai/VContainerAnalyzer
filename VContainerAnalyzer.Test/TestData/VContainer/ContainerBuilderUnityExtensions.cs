// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;

// ReSharper disable once CheckNamespace
namespace VContainer.Unity
{
    public readonly struct EntryPointsBuilder
    {
        private readonly IContainerBuilder _containerBuilder;
        private readonly Lifetime _lifetime;

        // ReSharper disable once ConvertToPrimaryConstructor
        public EntryPointsBuilder(IContainerBuilder containerBuilder, Lifetime lifetime)
        {
            this._containerBuilder = containerBuilder;
            this._lifetime = lifetime;
        }

        public RegistrationBuilder Add<T>() => _containerBuilder.Register<T>(_lifetime).AsImplementedInterfaces();
    }

    public static class ContainerBuilderUnityExtensions
    {
        public static RegistrationBuilder RegisterEntryPoint<T>(this IContainerBuilder builder,
            Lifetime lifetime = Lifetime.Singleton)
        {
            return new RegistrationBuilder();
        }

        public static void UseEntryPoints(
            this IContainerBuilder builder,
            Lifetime lifetime,
            Action<EntryPointsBuilder> configuration)
        {
            configuration(new EntryPointsBuilder(builder, lifetime));
        }
    }
}
