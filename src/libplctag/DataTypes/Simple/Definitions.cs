using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.DataTypes.Simple
{

    public class TagBool : Tag<BoolPlcMapper, bool> { }
    public class TagBool1D : Tag<BoolPlcMapper, bool[]> { }
    public class TagBool2D : Tag<BoolPlcMapper, bool[,]> { }
    public class TagBool3D : Tag<BoolPlcMapper, bool[,,]> { }


    public class TagDint : Tag<DintPlcMapper, int> { }
    public class TagDint1D : Tag<DintPlcMapper, int[]> { }
    public class TagDint2D : Tag<DintPlcMapper, int[,]> { }
    public class TagDint3D : Tag<DintPlcMapper, int[,,]> { }


    public class TagInt : Tag<IntPlcMapper, short> { }
    public class TagInt1D : Tag<IntPlcMapper, short[]> { }
    public class TagInt2D : Tag<IntPlcMapper, short[,]> { }
    public class TagInt3D : Tag<IntPlcMapper, short[,,]> { }


    public class TagLint : Tag<LintPlcMapper, long> { }
    public class TagLint1D : Tag<LintPlcMapper, long[]> { }
    public class TagLint2D : Tag<LintPlcMapper, long[,]> { }
    public class TagLint3D : Tag<LintPlcMapper, long[,,]> { }


    public class TagLreal : Tag<LrealPlcMapper, double> { }
    public class TagLreal1D : Tag<LrealPlcMapper, double[]> { }
    public class TagLreal2D : Tag<LrealPlcMapper, double[,]> { }
    public class TagLreal3D : Tag<LrealPlcMapper, double[,,]> { }


    public class TagReal : Tag<RealPlcMapper, float> { }
    public class TagReal1D : Tag<RealPlcMapper, float[]> { }
    public class TagReal2D : Tag<RealPlcMapper, float[,]> { }
    public class TagReal3D : Tag<RealPlcMapper, float[,,]> { }


    public class TagSint : Tag<SintPlcMapper, sbyte> { }
    public class TagSint1D : Tag<SintPlcMapper, sbyte[]> { }
    public class TagSint2D : Tag<SintPlcMapper, sbyte[,]> { }
    public class TagSint3D : Tag<SintPlcMapper, sbyte[,,]> { }


    public class TagString : Tag<StringPlcMapper, string> { }
    public class TagString1D : Tag<StringPlcMapper, string[]> { }
    public class TagString2D : Tag<StringPlcMapper, string[,]> { }
    public class TagString3D : Tag<StringPlcMapper, string[,,]> { }


    public class TagTagInfo : Tag<TagInfoPlcMapper, TagInfo[]> { }

    public class TagTimer : Tag<TimerPlcMapper, AbTimer> { }
    
    public class TagUdtInfo : Tag<UdtInfoPlcMapper, UdtInfo> { }

}
