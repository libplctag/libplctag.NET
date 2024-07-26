// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.DataTypes.Simple
{

    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagBool : Tag<BoolPlcMapper, bool> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagBool1D : Tag<BoolPlcMapper, bool[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagBool2D : Tag<BoolPlcMapper, bool[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagBool3D : Tag<BoolPlcMapper, bool[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagDint : Tag<DintPlcMapper, int> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagDint1D : Tag<DintPlcMapper, int[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagDint2D : Tag<DintPlcMapper, int[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagDint3D : Tag<DintPlcMapper, int[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagInt : Tag<IntPlcMapper, short> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagInt1D : Tag<IntPlcMapper, short[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagInt2D : Tag<IntPlcMapper, short[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagInt3D : Tag<IntPlcMapper, short[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLint : Tag<LintPlcMapper, long> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLint1D : Tag<LintPlcMapper, long[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLint2D : Tag<LintPlcMapper, long[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLint3D : Tag<LintPlcMapper, long[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLreal : Tag<LrealPlcMapper, double> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLreal1D : Tag<LrealPlcMapper, double[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLreal2D : Tag<LrealPlcMapper, double[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagLreal3D : Tag<LrealPlcMapper, double[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagReal : Tag<RealPlcMapper, float> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagReal1D : Tag<RealPlcMapper, float[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagReal2D : Tag<RealPlcMapper, float[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagReal3D : Tag<RealPlcMapper, float[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagSint : Tag<SintPlcMapper, sbyte> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagSint1D : Tag<SintPlcMapper, sbyte[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagSint2D : Tag<SintPlcMapper, sbyte[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagSint3D : Tag<SintPlcMapper, sbyte[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagString : Tag<StringPlcMapper, string> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagString1D : Tag<StringPlcMapper, string[]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagString2D : Tag<StringPlcMapper, string[,]> { }
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagString3D : Tag<StringPlcMapper, string[,,]> { }


    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagTagInfo : Tag<TagInfoPlcMapper, TagInfo[]> { }

    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagTimer : Tag<TimerPlcMapper, AbTimer> { }
    
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class TagUdtInfo : Tag<UdtInfoPlcMapper, UdtInfo> { }

}
