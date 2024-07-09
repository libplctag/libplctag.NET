// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace libplctag
{

    /// <summary>
    /// An exception thrown by the underlying libplctag library
    /// </summary>
    public class LibPlcTagException : Exception
    {
        public LibPlcTagException()
        {
        }

        public LibPlcTagException(string message)
            : base(message)
        {
        }

        public LibPlcTagException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public LibPlcTagException(Status status)
            : base(status.ToString())
        {
        }
    }
}
