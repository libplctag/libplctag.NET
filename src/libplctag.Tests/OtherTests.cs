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
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);

            // Act

            // Assert
            var status = tag.GetStatus();
            Assert.Equal(Status.Ok, status);
        }

        [Fact]
        public void Attribute_string_formatted_correctly()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object)
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
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);

            // Act
            tag.Initialize();


            // Assert
            var expectedAttributeString = "";
            nativeTag.Verify(m => m.plc_tag_create_ex(expectedAttributeString, It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);

        }

        [Theory]
        [InlineData(null, true, "str_is_zero_terminated=1")]
        [InlineData(null, false, "str_is_zero_terminated=0")]
        [InlineData(true, null, "str_is_fixed_length=1")]
        [InlineData(false, null, "str_is_fixed_length=0")]
        [InlineData(true, false, "str_is_fixed_length=1&str_is_zero_terminated=0")]
        [InlineData(false, true, "str_is_fixed_length=0&str_is_zero_terminated=1")]
        [InlineData(null, null, "")]
        public void Attribute_string_formats_string_flags_independently(bool? fixedLength, bool? zeroTerminated, string expectedAttributeString)
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object)
            {
                StringIsFixedLength = fixedLength,
                StringIsZeroTerminated = zeroTerminated,
            };

            // Act
            tag.Initialize();

            // Assert
            nativeTag.Verify(m => m.plc_tag_create_ex(expectedAttributeString, It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Attribute_string_contains_connection_group_id_when_set()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object)
            {
                ConnectionGroupId = 7,
            };

            // Act
            tag.Initialize();

            // Assert
            var expectedAttributeString = "connection_group_id=7";
            nativeTag.Verify(m => m.plc_tag_create_ex(expectedAttributeString, It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Connection_inactivity_timeout_reports_value_in_effect_after_initialization()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            nativeTag
                .Setup(m => m.plc_tag_get_int_attribute(It.IsAny<int>(), "connection_inactivity_timeout_ms", It.IsAny<int>()))
                .Returns(31000);
            var tag = new Tag(nativeTag.Object)
            {
                ConnectionInactivityTimeout = TimeSpan.FromMilliseconds(50000),
            };

            // Act
            tag.Initialize();

            // Assert
            nativeTag.Verify(m => m.plc_tag_create_ex("connection_inactivity_timeout_ms=50000", It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(TimeSpan.FromMilliseconds(31000), tag.ConnectionInactivityTimeout);
        }

        [Fact]
        public void Connection_inactivity_timeout_set_after_initialization_is_written_to_native_tag()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);
            tag.Initialize();

            // Act
            tag.ConnectionInactivityTimeout = TimeSpan.FromMilliseconds(5000);

            // Assert
            nativeTag.Verify(m => m.plc_tag_set_int_attribute(It.IsAny<int>(), "connection_inactivity_timeout_ms", 5000), Times.Once);
        }

        [Fact]
        public void Read_cache_set_to_null_after_initialization_disables_cache()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object) { ReadCacheMillisecondDuration = 100 };
            tag.Initialize();

            // Act
            tag.ReadCacheMillisecondDuration = null;

            // Assert
            nativeTag.Verify(m => m.plc_tag_set_int_attribute(It.IsAny<int>(), "read_cache_ms", 0), Times.Once);
            Assert.Null(tag.ReadCacheMillisecondDuration);
        }

        [Fact]
        public void Connection_inactivity_timeout_set_to_null_after_initialization_is_passed_to_core_as_max_int()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);
            tag.Initialize();

            // Act
            tag.ConnectionInactivityTimeout = null;

            // Assert
            nativeTag.Verify(m => m.plc_tag_set_int_attribute(It.IsAny<int>(), "connection_inactivity_timeout_ms", int.MaxValue), Times.Once);
        }
    }
}
