// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag.DataTypes;
using libplctag;
using System;

namespace CSharp_DotNetCore
{
    internal class ExampleNotifyChanged
    {
        public static void Run()
        {
            var myTag = new NotifyValueChangedTag<DintPlcMapper, int>()
            {
                Name = "MyTag",
                Gateway = "127.0.0.1",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                AutoSyncReadInterval = TimeSpan.FromMilliseconds(500)
            };

            myTag.ValueChanged += (s, e) => Console.WriteLine($"{myTag.Name} changed at {DateTime.Now} to {myTag.Value}");

            var myUdtTag = new NotifyValueChangedTag<MyUdtMapper, MyUdt>()
            {
                Name = "MyUdtTag",
                Gateway = "127.0.0.1",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                AutoSyncReadInterval = TimeSpan.FromMilliseconds(500)
            };

            myUdtTag.ValueChanged += (s, e) => Console.WriteLine($"{myUdtTag.Name} changed at {DateTime.Now} to {myUdtTag.Value.MyDint}|{myUdtTag.Value.MyFloat}");
        }

        class NotifyValueChangedTag<M, T> : Tag<M, T>
            where M : IPlcMapper<T>, new()
        {
            public event EventHandler<EventArgs> ValueChanged;

            private int previousHash;

            public NotifyValueChangedTag()
            {
                ReadCompleted += NotifyValueChangedTag_ReadCompleted;
            }

            private void NotifyValueChangedTag_ReadCompleted(object sender, TagEventArgs e)
            {
                var currentHash = this.Value.GetHashCode();
                if (currentHash != previousHash)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
                previousHash = currentHash;
            }
        }

        class MyUdt
        {
            public int MyDint { get; set; }
            public float MyFloat { get; set; }

            public override int GetHashCode()
            {
                // Use the tuple shortcut to get an okay hash
                return (MyDint, MyFloat).GetHashCode();
            }

        }

        class MyUdtMapper : PlcMapperBase<MyUdt>
        {
            public override int? ElementSize => 8;

            public override MyUdt Decode(Tag tag, int offset)
            {
                return new MyUdt()
                {
                    MyDint = tag.GetInt32(offset),
                    MyFloat = tag.GetFloat32(offset + 4)
                };
            }

            public override void Encode(Tag tag, int offset, MyUdt value)
            {
                tag.SetInt32(offset, value.MyDint);
                tag.SetFloat32(offset + 4, value.MyFloat);
            }
        }
    }
}