// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;

// ReSharper disable once CheckNamespace
namespace VContainer
{
    public static class ContainerBuilderExtensions
    {
        public static RegistrationBuilder Register<T>(this IContainerBuilder builder, Lifetime lifetime)
        {
            return new RegistrationBuilder();
        }

        public static RegistrationBuilder Register<TInterface, TImplement>(
            this IContainerBuilder builder,
            Lifetime lifetime)
            where TImplement : TInterface
        {
            return new RegistrationBuilder();
        }

        public static RegistrationBuilder Register<TInterface1, TInterface2, TImplement>(this IContainerBuilder builder,
            Lifetime lifetime) where TImplement : TInterface1, TInterface2
        {
            return new RegistrationBuilder();
        }

        public static RegistrationBuilder Register<TInterface1, TInterface2, TInterface3, TImplement>(
            this IContainerBuilder builder,
            Lifetime lifetime) where TImplement : TInterface1, TInterface2, TInterface3
        {
            return new RegistrationBuilder();
        }

        public static RegistrationBuilder Register<TInterface>(
            this IContainerBuilder builder,
            Func<IObjectResolver, TInterface> implementationConfiguration,
            Lifetime lifetime)
        {
            return new RegistrationBuilder();
        }
    }
}
