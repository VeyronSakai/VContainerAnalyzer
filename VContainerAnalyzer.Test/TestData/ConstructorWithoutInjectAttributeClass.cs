// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;

namespace VContainerAnalyzer.Test.TestData
{
    public class ConstructorWithoutInjectAttributeClass : IInterface1, IInterface2, IInterface3
    {
        public ConstructorWithoutInjectAttributeClass(int x, int y)
        {
            Console.WriteLine("");
        }
    }
}
