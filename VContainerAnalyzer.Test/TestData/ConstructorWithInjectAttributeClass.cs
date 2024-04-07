// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;
using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class ConstructorWithInjectAttributeClass
    {
        [Inject]
        public ConstructorWithInjectAttributeClass(EmptyClassStub stub1, EmptyClassStub stub2)
        {
            Console.WriteLine("");
        }
    }
}
