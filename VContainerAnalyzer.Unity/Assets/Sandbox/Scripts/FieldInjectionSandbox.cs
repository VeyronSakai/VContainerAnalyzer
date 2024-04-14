// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace Sandbox
{
    public class FieldInjectionSandbox
    {
        [Inject] private EmptyClassStub _field1;
        private EmptyClassStub _field2;
    }
}
