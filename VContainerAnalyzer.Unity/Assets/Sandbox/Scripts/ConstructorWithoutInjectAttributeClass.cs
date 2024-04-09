// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

namespace Sandbox
{
    public class ConstructorWithoutInjectAttributeClass : IInterface1, IInterface2, IInterface3
    {
        public ConstructorWithoutInjectAttributeClass(EmptyClassStub stub1, EmptyClassStub stub2)
        {
        }
    }
}
