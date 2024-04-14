// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using VContainer;

namespace VContainerAnalyzer.Test.TestData
{
    public class FieldInjectionClass
    {
        [Inject] private EmptyClassStub _field;
    }
}
