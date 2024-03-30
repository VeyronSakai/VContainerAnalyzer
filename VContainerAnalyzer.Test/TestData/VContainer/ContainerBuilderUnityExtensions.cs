// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

// ReSharper disable once CheckNamespace
namespace VContainer.Unity
{
    public static class ContainerBuilderUnityExtensions
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

        public static RegistrationBuilder RegisterEntryPoint<T>(this IContainerBuilder builder,
            Lifetime lifetime = Lifetime.Singleton)
        {
            return new RegistrationBuilder();
        }
    }
}
