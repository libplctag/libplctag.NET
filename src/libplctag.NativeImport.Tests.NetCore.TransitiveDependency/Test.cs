// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.NativeImport.Tests.NetCore.TransitiveDependency
{
    public class Test
    {
        [Fact]
        public void Can_execute_native_methods()
        {
            // The test is succesful if this does not throw
            var output = LibPlcTag.IsRequiredVersion(0, 0, 0);

            // The below is redundant but is used to prove to the reader that the test works
            Assert.True(true);
        }
    }
}