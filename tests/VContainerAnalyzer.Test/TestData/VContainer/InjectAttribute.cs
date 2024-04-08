// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;

// ReSharper disable once CheckNamespace
namespace VContainer
{
    public class PreserveAttribute : Attribute
    {
    }

    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : PreserveAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = false, Inherited = true)]
    public class InjectIgnoreAttribute : Attribute
    {
    }
}
