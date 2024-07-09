// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using Xunit;
using Moq;

namespace libplctag.Tests
{
    public class OtherTests
    {

        [Fact]
        public void Status_ok_when_first_created()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object);

            // Act

            // Assert
            var status = tag.GetStatus();
            Assert.Equal(Status.Ok, status);
        }

        [Fact]
        public void Attribute_string_formatted_correctly()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                ElementSize = 4,
                ElementCount = 10,
                PlcType = PlcType.Slc500,
                Name = "TagName",
            };


            // Act
            tag.Initialize();


            // Assert
            var expectedAttributeString = "plc=slc500&elem_size=4&elem_count=10&name=TagName";
            nativeTag.Verify(m => m.plc_tag_create_ex(expectedAttributeString, It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public void Attribute_string_does_not_contain_unset_properties()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object);

            // Act
            tag.Initialize();


            // Assert
            var expectedAttributeString = "";
            nativeTag.Verify(m => m.plc_tag_create_ex(expectedAttributeString, It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);

        }
    }
}
