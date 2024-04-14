// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace Sandbox
{
    public class PropertyInjectionSandbox
    {
        [Inject] public EmptyClassStub Property1 { get; set; }
        public EmptyClassStub Property2 { get; set; }
    }
}
