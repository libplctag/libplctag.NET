// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace libplctag.Tests
{
    public class DisposeTests
    {

        [Fact]
        public void Destroy_is_called_if_initialized_and_disposed()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);

            // Act
            tag.Initialize();
            tag.Dispose();

            // Assert
            nativeTag.Verify(m => m.plc_tag_destroy(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GithubIssue418()
        {
            // Arrange
            const int tagId = 11;

            NativeImport.plctag.callback_func_ex callback = null;
            Status? status = null;

            var nativeTag = new Mock<INative>();

            nativeTag
                .Setup(m => m.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), 0))
                .Callback<string, NativeImport.plctag.callback_func_ex, IntPtr, int>(async (attributeString, callbackFunc, userData, timeout) =>
                {
                    status = Status.Pending;
                    callback = callbackFunc;
                    status = Status.ErrorNotFound;
                    callback?.Invoke(tagId, (int)NativeImport.EVENT.PLCTAG_EVENT_CREATED, (int)NativeImport.STATUS.PLCTAG_ERR_NOT_FOUND, IntPtr.Zero);
                });

            // the status was being tracked, so return it if asked
            nativeTag
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns(() => (int)status.Value);

            // Act
            using(var tag = new Tag(nativeTag.Object))
            {
                try
                {
                    await tag.InitializeAsync();
                }
                catch (Exception e) when (e.Message == "ErrorNotFound") // we are expecting this exception
                {
                }
            }

            // Assert
            LibPlcTag.Shutdown();

        }

        [Fact]
        public void Can_not_use_if_already_disposed()
        {
            // Arrange
            var nativeTag = new Mock<INative>();
            var tag = new Tag(nativeTag.Object);

            // Act
            tag.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => tag.GetStatus());
        }

    }
}
