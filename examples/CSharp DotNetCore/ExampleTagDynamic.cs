// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;

namespace CSharp_DotNetCore
{
    class ExampleTagDynamic
    {
        public static void Run()
        {

            /*
             * 
             * WARNING: THIS HAS NOT BEEN TESTED
             * 
             */

            var tag = new TagDynamic()
            {
                Name = "MyDintTag",
                // Configure other properties....
            };

            tag.Initialize(); // Detects the tag is a DINT and configures the appropriate getter and setter.
            Console.WriteLine(tag.Value);  // Prints e.g. 1234
            tag.Value = 4321;
            tag.Write();
            tag.Value = "Hello world"; // This will compile but will fail at runtime
        }

        class TagDynamic
        {
            private readonly Tag _tag;
            private Func<Tag, int, dynamic> decoder;
            private Action<Tag, int, dynamic> encoder;

            public TagDynamic()
            {
                _tag = new Tag()
                {
                    PlcType = PlcType.ControlLogix,   // Only certain types of devices support this, so hard-code this
                    Protocol = Protocol.ab_eip,                 // Only certain types of devices support this, so hard-code this
                };

                _tag.ReadCompleted += (s, e) => ReadCompleted?.Invoke(this, e);
                _tag.WriteCompleted += (s, e) => WriteCompleted?.Invoke(this, e);
            }

            /// <inheritdoc cref="Tag.Initialize">
            public void Initialize()
            {
                _tag.Initialize();

                // After we have read the tag metadata, configure the encoder/decoder
                Configure();
            }

            /// <inheritdoc cref="Tag.Write">
            public void Write()
            {
                if (!_tag.IsInitialized)
                {
                    Initialize();
                }

                _tag.Write();
            }

            /// <inheritdoc cref="Tag.Read">
            public void Read()
            {
                if (!_tag.IsInitialized)
                {
                    Initialize();
                    return; // Initialize already does a read, so in this case we don't need to read again and can return early.
                }

                _tag.Read();
            }

            /// <summary>
            /// The value stored in local memory.
            /// </summary> 
            public object Value
            {
                get => decoder(_tag, 0);
                set => encoder(_tag, 0, value);
            }

            /// <inheritdoc cref="Tag.Gateway"/>
            public string Gateway
            {
                get => _tag.Gateway;
                set => _tag.Gateway = value;
            }

            /// <inheritdoc cref="Tag.Path"/>
            public string Path
            {
                get => _tag.Path;
                set => _tag.Path = value;
            }

            /// <inheritdoc cref="Tag.Name"/>
            public string Name
            {
                get => _tag.Name;
                set => _tag.Name = value;
            }

            /// <inheritdoc cref="Tag.Timeout"/>
            public TimeSpan Timeout
            {
                get => _tag.Timeout;
                set => _tag.Timeout = value;
            }

            /// <inheritdoc cref="Tag.AutoSyncReadInterval"/>
            public TimeSpan? AutoSyncReadInterval
            {
                get => _tag.AutoSyncReadInterval;
                set => _tag.AutoSyncReadInterval = value;
            }

            /// <inheritdoc cref="Tag.AutoSyncWriteInterval"/>
            public TimeSpan? AutoSyncWriteInterval
            {
                get => _tag.AutoSyncWriteInterval;
                set => _tag.AutoSyncWriteInterval = value;
            }

            /// <inheritdoc cref="Tag.DebugLevel"/>
            public DebugLevel DebugLevel
            {
                get => _tag.DebugLevel;
                set => _tag.DebugLevel = value;
            }

            public event EventHandler<TagEventArgs> ReadCompleted;
            public event EventHandler<TagEventArgs> WriteCompleted;

            private void Configure()
            {
                var tagTypeBytes = _tag.GetByteArrayAttribute("raw_tag_type_bytes");
                var tagTypeCode = BitConverter.ToUInt16(tagTypeBytes, 0);   // Endianess?

                switch (tagTypeCode)
                {
                    case 0xC1: // BOOL: Boolean value
                        decoder = (tag, offset) => tag.GetBit(offset);
                        encoder = (tag, offset, value) => tag.SetBit(offset, value);
                        break;

                    case 0xC2: // SINT: Signed 8-bit integer value
                        decoder = (tag, offset) => tag.GetInt8(offset);
                        encoder = (tag, offset, value) => tag.SetInt8(offset, value);
                        break;

                    case 0xC3: // INT: Signed 16-bit integer value
                        decoder = (tag, offset) => tag.GetInt16(offset);
                        encoder = (tag, offset, value) => tag.SetInt16(offset, value);
                        break;

                    case 0xC4: // DINT: Signed 32-bit integer value
                        decoder = (tag, offset) => tag.GetInt32(offset);
                        encoder = (tag, offset, value) => tag.SetInt32(offset, value);
                        break;

                    case 0xC5: // LINT: Signed 64-bit integer value
                        decoder = (tag, offset) => tag.GetInt64(offset);
                        encoder = (tag, offset, value) => tag.SetInt64(offset, value);
                        break;

                    case 0xC6: // USINT: Unsigned 8-bit integer value
                        decoder = (tag, offset) => tag.GetUInt8(offset);
                        encoder = (tag, offset, value) => tag.SetUInt8(offset, value);
                        break;

                    case 0xC7: // UINT: Unsigned 16-bit integer value
                        decoder = (tag, offset) => tag.GetUInt16(offset);
                        encoder = (tag, offset, value) => tag.SetUInt16(offset, value);
                        break;

                    case 0xC8: // UDINT: Unsigned 32-bit integer value
                        decoder = (tag, offset) => tag.GetUInt32(offset);
                        encoder = (tag, offset, value) => tag.SetUInt32(offset, value);
                        break;

                    case 0xC9: // ULINT: Unsigned 64-bit integer value
                        decoder = (tag, offset) => tag.GetUInt64(offset);
                        encoder = (tag, offset, value) => tag.SetUInt64(offset, value);
                        break;

                    case 0xCA: // REAL: 32-bit floating point value, IEEE format
                        decoder = (tag, offset) => tag.GetFloat32(offset);
                        encoder = (tag, offset, value) => tag.SetFloat32(offset, value);
                        break;

                    case 0xCB: // LREAL: 64-bit floating point value, IEEE format
                        decoder = (tag, offset) => tag.GetFloat64(offset);
                        encoder = (tag, offset, value) => tag.SetFloat64(offset, value);
                        break;

                    case 0xCC: // Synchronous time value
                    case 0xCD: // Date value
                    case 0xCE: // Time of day value
                    case 0xCF: // Date and time of day value
                    case 0xD0: // Character string, 1 byte per character
                    case 0xD1: // 8-bit bit string
                    case 0xD2: // 16-bit bit string
                    case 0xD3: // 32-bit bit string
                    case 0xD4: // 64-bit bit string
                    case 0xD5: // Wide char character string, 2 bytes per character
                    case 0xD6: // High resolution duration value
                    case 0xD7: // Medium resolution duration value
                    case 0xD8: // Low resolution duration value
                    case 0xD9: // N-byte per char character string
                    case 0xDA: // Counted character sting with 1 byte per character and 1 byte length indicator
                    case 0xDB: // Duration in milliseconds
                    case 0xDC: // CIP path segment(s)
                    case 0xDD: // Engineering units
                    case 0xDE: // International character string (encoding?)
                        throw new NotImplementedException();
                }
            }
        }

    }
}
