namespace engenious.Graphics
{
    /// <summary>
    /// The type of a effect parameter
    /// </summary>
    public enum EffectParameterType
    {
        /// <summary>Original was GL_INT = 0x1404</summary>
        Int = 5124, // 0x00001404
        /// <summary>Original was GL_UNSIGNED_INT = 0x1405</summary>
        UnsignedInt = 5125, // 0x00001405
        /// <summary>Original was GL_FLOAT = 0x1406</summary>
        Float = 5126, // 0x00001406
        /// <summary>Original was GL_DOUBLE = 0x140A</summary>
        Double = 5130, // 0x0000140A
        /// <summary>Original was GL_FLOAT_VEC2 = 0x8B50</summary>
        FloatVec2 = 35664, // 0x00008B50
        /// <summary>Original was GL_FLOAT_VEC3 = 0x8B51</summary>
        FloatVec3 = 35665, // 0x00008B51
        /// <summary>Original was GL_FLOAT_VEC4 = 0x8B52</summary>
        FloatVec4 = 35666, // 0x00008B52
        /// <summary>Original was GL_INT_VEC2 = 0x8B53</summary>
        IntVec2 = 35667, // 0x00008B53
        /// <summary>Original was GL_INT_VEC3 = 0x8B54</summary>
        IntVec3 = 35668, // 0x00008B54
        /// <summary>Original was GL_INT_VEC4 = 0x8B55</summary>
        IntVec4 = 35669, // 0x00008B55
        /// <summary>Original was GL_BOOL = 0x8B56</summary>
        Bool = 35670, // 0x00008B56
        /// <summary>Original was GL_BOOL_VEC2 = 0x8B57</summary>
        BoolVec2 = 35671, // 0x00008B57
        /// <summary>Original was GL_BOOL_VEC3 = 0x8B58</summary>
        BoolVec3 = 35672, // 0x00008B58
        /// <summary>Original was GL_BOOL_VEC4 = 0x8B59</summary>
        BoolVec4 = 35673, // 0x00008B59
        /// <summary>Original was GL_FLOAT_MAT2 = 0x8B5A</summary>
        FloatMat2 = 35674, // 0x00008B5A
        /// <summary>Original was GL_FLOAT_MAT3 = 0x8B5B</summary>
        FloatMat3 = 35675, // 0x00008B5B
        /// <summary>Original was GL_FLOAT_MAT4 = 0x8B5C</summary>
        FloatMat4 = 35676, // 0x00008B5C
        /// <summary>Original was GL_SAMPLER_1D = 0x8B5D</summary>
        Sampler1D = 35677, // 0x00008B5D
        /// <summary>Original was GL_SAMPLER_2D = 0x8B5E</summary>
        Sampler2D = 35678, // 0x00008B5E
        /// <summary>Original was GL_SAMPLER_3D = 0x8B5F</summary>
        Sampler3D = 35679, // 0x00008B5F
        /// <summary>Original was GL_SAMPLER_CUBE = 0x8B60</summary>
        SamplerCube = 35680, // 0x00008B60
        /// <summary>Original was GL_SAMPLER_1D_SHADOW = 0x8B61</summary>
        Sampler1DShadow = 35681, // 0x00008B61
        /// <summary>Original was GL_SAMPLER_2D_SHADOW = 0x8B62</summary>
        Sampler2DShadow = 35682, // 0x00008B62
        /// <summary>Original was GL_SAMPLER_2D_RECT = 0x8B63</summary>
        Sampler2DRect = 35683, // 0x00008B63
        /// <summary>Original was GL_SAMPLER_2D_RECT_SHADOW = 0x8B64</summary>
        Sampler2DRectShadow = 35684, // 0x00008B64
        /// <summary>Original was GL_FLOAT_MAT2x3 = 0x8B65</summary>
        FloatMat2x3 = 35685, // 0x00008B65
        /// <summary>Original was GL_FLOAT_MAT2x4 = 0x8B66</summary>
        FloatMat2x4 = 35686, // 0x00008B66
        /// <summary>Original was GL_FLOAT_MAT3x2 = 0x8B67</summary>
        FloatMat3x2 = 35687, // 0x00008B67
        /// <summary>Original was GL_FLOAT_MAT3x4 = 0x8B68</summary>
        FloatMat3x4 = 35688, // 0x00008B68
        /// <summary>Original was GL_FLOAT_MAT4x2 = 0x8B69</summary>
        FloatMat4x2 = 35689, // 0x00008B69
        /// <summary>Original was GL_FLOAT_MAT4x3 = 0x8B6A</summary>
        FloatMat4x3 = 35690, // 0x00008B6A
        /// <summary>Original was GL_SAMPLER_1D_ARRAY = 0x8DC0</summary>
        Sampler1DArray = 36288, // 0x00008DC0
        /// <summary>Original was GL_SAMPLER_2D_ARRAY = 0x8DC1</summary>
        Sampler2DArray = 36289, // 0x00008DC1
        /// <summary>Original was GL_SAMPLER_BUFFER = 0x8DC2</summary>
        SamplerBuffer = 36290, // 0x00008DC2
        /// <summary>Original was GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3</summary>
        Sampler1DArrayShadow = 36291, // 0x00008DC3
        /// <summary>Original was GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4</summary>
        Sampler2DArrayShadow = 36292, // 0x00008DC4
        /// <summary>Original was GL_SAMPLER_CUBE_SHADOW = 0x8DC5</summary>
        SamplerCubeShadow = 36293, // 0x00008DC5
        /// <summary>Original was GL_UNSIGNED_INT_VEC2 = 0x8DC6</summary>
        UnsignedIntVec2 = 36294, // 0x00008DC6
        /// <summary>Original was GL_UNSIGNED_INT_VEC3 = 0x8DC7</summary>
        UnsignedIntVec3 = 36295, // 0x00008DC7
        /// <summary>Original was GL_UNSIGNED_INT_VEC4 = 0x8DC8</summary>
        UnsignedIntVec4 = 36296, // 0x00008DC8
        /// <summary>Original was GL_INT_SAMPLER_1D = 0x8DC9</summary>
        IntSampler1D = 36297, // 0x00008DC9
        /// <summary>Original was GL_INT_SAMPLER_2D = 0x8DCA</summary>
        IntSampler2D = 36298, // 0x00008DCA
        /// <summary>Original was GL_INT_SAMPLER_3D = 0x8DCB</summary>
        IntSampler3D = 36299, // 0x00008DCB
        /// <summary>Original was GL_INT_SAMPLER_CUBE = 0x8DCC</summary>
        IntSamplerCube = 36300, // 0x00008DCC
        /// <summary>Original was GL_INT_SAMPLER_2D_RECT = 0x8DCD</summary>
        IntSampler2DRect = 36301, // 0x00008DCD
        /// <summary>Original was GL_INT_SAMPLER_1D_ARRAY = 0x8DCE</summary>
        IntSampler1DArray = 36302, // 0x00008DCE
        /// <summary>Original was GL_INT_SAMPLER_2D_ARRAY = 0x8DCF</summary>
        IntSampler2DArray = 36303, // 0x00008DCF
        /// <summary>Original was GL_INT_SAMPLER_BUFFER = 0x8DD0</summary>
        IntSamplerBuffer = 36304, // 0x00008DD0
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1</summary>
        UnsignedIntSampler1D = 36305, // 0x00008DD1
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2</summary>
        UnsignedIntSampler2D = 36306, // 0x00008DD2
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3</summary>
        UnsignedIntSampler3D = 36307, // 0x00008DD3
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4</summary>
        UnsignedIntSamplerCube = 36308, // 0x00008DD4
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5</summary>
        UnsignedIntSampler2DRect = 36309, // 0x00008DD5
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6</summary>
        UnsignedIntSampler1DArray = 36310, // 0x00008DD6
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7</summary>
        UnsignedIntSampler2DArray = 36311, // 0x00008DD7
        /// <summary>Original was GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8</summary>
        UnsignedIntSamplerBuffer = 36312, // 0x00008DD8
        /// <summary>Original was GL_DOUBLE_VEC2 = 0x8FFC</summary>
        DoubleVec2 = 36860, // 0x00008FFC
        /// <summary>Original was GL_DOUBLE_VEC3 = 0x8FFD</summary>
        DoubleVec3 = 36861, // 0x00008FFD
        /// <summary>Original was GL_DOUBLE_VEC4 = 0x8FFE</summary>
        DoubleVec4 = 36862, // 0x00008FFE
        /// <summary>Original was GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C</summary>
        SamplerCubeMapArray = 36876, // 0x0000900C
        /// <summary>Original was GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D</summary>
        SamplerCubeMapArrayShadow = 36877, // 0x0000900D
        /// <summary>Original was GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E</summary>
        IntSamplerCubeMapArray = 36878, // 0x0000900E
        /// <summary>
        /// Original was GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F
        /// </summary>
        UnsignedIntSamplerCubeMapArray = 36879, // 0x0000900F
        /// <summary>Original was GL_IMAGE_1D = 0x904C</summary>
        Image1D = 36940, // 0x0000904C
        /// <summary>Original was GL_IMAGE_2D = 0x904D</summary>
        Image2D = 36941, // 0x0000904D
        /// <summary>Original was GL_IMAGE_3D = 0x904E</summary>
        Image3D = 36942, // 0x0000904E
        /// <summary>Original was GL_IMAGE_2D_RECT = 0x904F</summary>
        Image2DRect = 36943, // 0x0000904F
        /// <summary>Original was GL_IMAGE_CUBE = 0x9050</summary>
        ImageCube = 36944, // 0x00009050
        /// <summary>Original was GL_IMAGE_BUFFER = 0x9051</summary>
        ImageBuffer = 36945, // 0x00009051
        /// <summary>Original was GL_IMAGE_1D_ARRAY = 0x9052</summary>
        Image1DArray = 36946, // 0x00009052
        /// <summary>Original was GL_IMAGE_2D_ARRAY = 0x9053</summary>
        Image2DArray = 36947, // 0x00009053
        /// <summary>Original was GL_IMAGE_CUBE_MAP_ARRAY = 0x9054</summary>
        ImageCubeMapArray = 36948, // 0x00009054
        /// <summary>Original was GL_IMAGE_2D_MULTISAMPLE = 0x9055</summary>
        Image2DMultisample = 36949, // 0x00009055
        /// <summary>Original was GL_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9056</summary>
        Image2DMultisampleArray = 36950, // 0x00009056
        /// <summary>Original was GL_INT_IMAGE_1D = 0x9057</summary>
        IntImage1D = 36951, // 0x00009057
        /// <summary>Original was GL_INT_IMAGE_2D = 0x9058</summary>
        IntImage2D = 36952, // 0x00009058
        /// <summary>Original was GL_INT_IMAGE_3D = 0x9059</summary>
        IntImage3D = 36953, // 0x00009059
        /// <summary>Original was GL_INT_IMAGE_2D_RECT = 0x905A</summary>
        IntImage2DRect = 36954, // 0x0000905A
        /// <summary>Original was GL_INT_IMAGE_CUBE = 0x905B</summary>
        IntImageCube = 36955, // 0x0000905B
        /// <summary>Original was GL_INT_IMAGE_BUFFER = 0x905C</summary>
        IntImageBuffer = 36956, // 0x0000905C
        /// <summary>Original was GL_INT_IMAGE_1D_ARRAY = 0x905D</summary>
        IntImage1DArray = 36957, // 0x0000905D
        /// <summary>Original was GL_INT_IMAGE_2D_ARRAY = 0x905E</summary>
        IntImage2DArray = 36958, // 0x0000905E
        /// <summary>Original was GL_INT_IMAGE_CUBE_MAP_ARRAY = 0x905F</summary>
        IntImageCubeMapArray = 36959, // 0x0000905F
        /// <summary>Original was GL_INT_IMAGE_2D_MULTISAMPLE = 0x9060</summary>
        IntImage2DMultisample = 36960, // 0x00009060
        /// <summary>Original was GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9061</summary>
        IntImage2DMultisampleArray = 36961, // 0x00009061
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_1D = 0x9062</summary>
        UnsignedIntImage1D = 36962, // 0x00009062
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_2D = 0x9063</summary>
        UnsignedIntImage2D = 36963, // 0x00009063
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_3D = 0x9064</summary>
        UnsignedIntImage3D = 36964, // 0x00009064
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_2D_RECT = 0x9065</summary>
        UnsignedIntImage2DRect = 36965, // 0x00009065
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_CUBE = 0x9066</summary>
        UnsignedIntImageCube = 36966, // 0x00009066
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_BUFFER = 0x9067</summary>
        UnsignedIntImageBuffer = 36967, // 0x00009067
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_1D_ARRAY = 0x9068</summary>
        UnsignedIntImage1DArray = 36968, // 0x00009068
        /// <summary>Original was GL_UNSIGNED_INT_IMAGE_2D_ARRAY = 0x9069</summary>
        UnsignedIntImage2DArray = 36969, // 0x00009069
        /// <summary>
        /// Original was GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = 0x906A
        /// </summary>
        UnsignedIntImageCubeMapArray = 36970, // 0x0000906A
        /// <summary>
        /// Original was GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE = 0x906B
        /// </summary>
        UnsignedIntImage2DMultisample = 36971, // 0x0000906B
        /// <summary>
        /// Original was GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x906C
        /// </summary>
        UnsignedIntImage2DMultisampleArray = 36972, // 0x0000906C
        /// <summary>Original was GL_SAMPLER_2D_MULTISAMPLE = 0x9108</summary>
        Sampler2DMultisample = 37128, // 0x00009108
        /// <summary>Original was GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109</summary>
        IntSampler2DMultisample = 37129, // 0x00009109
        /// <summary>
        /// Original was GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A
        /// </summary>
        UnsignedIntSampler2DMultisample = 37130, // 0x0000910A
        /// <summary>Original was GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B</summary>
        Sampler2DMultisampleArray = 37131, // 0x0000910B
        /// <summary>
        /// Original was GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C
        /// </summary>
        IntSampler2DMultisampleArray = 37132, // 0x0000910C
        /// <summary>
        /// Original was GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D
        /// </summary>
        UnsignedIntSampler2DMultisampleArray = 37133, // 0x0000910D
        /// <summary>Original was GL_UNSIGNED_INT_ATOMIC_COUNTER = 0x92DB</summary>
        UnsignedIntAtomicCounter = 37595, // 0x000092DB
    }
}