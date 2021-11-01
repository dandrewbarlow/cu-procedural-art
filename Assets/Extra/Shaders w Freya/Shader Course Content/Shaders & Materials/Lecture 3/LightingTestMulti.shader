Shader "Unlit/LightingTestMulti" {
    Properties {
        _RockAlbedo ("Rock Albedo", 2D) = "white" {}
        [NoScaleOffset] _RockNormals ("Rock Normals", 2D) = "bump" {}
        [NoScaleOffset] _RockHeight ("Rock Height", 2D) = "gray" {}
        [NoScaleOffset] _DiffuseIBL ("Diffuse IBL", 2D) = "black" {}
        [NoScaleOffset] _SpecularIBL ("Specular IBL", 2D) = "black" {}
        _Gloss ("Gloss", Range(0,1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _AmbientLight ("Ambient Light", Color) = (0,0,0,0)
        _SpecIBLIntensity ("Spec IBL Intensity", Range(0,1)) = 1
        _NormalIntensity ("Normal Intensity", Range(0,1)) = 1
        _DispStrength ("Displacement Strength", Range(0,0.2)) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        // Base pass
        Pass {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define IS_IN_BASE_PASS
            #include "FGLighting.cginc"
            ENDCG
        }
        
        // Add pass
        Pass {
            Tags { "LightMode" = "ForwardAdd" }
            Blend One One // src*1 + dst*1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd
            #include "FGLighting.cginc"
            ENDCG
        }
        
        
    }
}
