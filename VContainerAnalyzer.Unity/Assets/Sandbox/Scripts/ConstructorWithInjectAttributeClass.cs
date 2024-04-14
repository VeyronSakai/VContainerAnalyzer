// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace Sandbox
{
    public class ConstructorWithInjectAttributeClass : IInterface1, IInterface2, IInterface3
    {
        [Inject]
        public ConstructorWithInjectAttributeClass(EmptyClassStub stub1, EmptyClassStub stub2)
        {
        }
    }
}
