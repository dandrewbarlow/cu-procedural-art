Shader "Unlit/hw6Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            // blending

            // Cull Front
            // Blend One One
            Blend DstColor Zero // Multiplicitave Blending
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolator i) : SV_Target
            {
                float4 c = _Color;

                c *= (sin( length(i.uv-float2(0.5, 0.5)) * 20+sin(i.uv.x) ) + 1);
                // c.xy *= sin(i.uv);

                return c;
            }
            ENDCG
        }
    }
}
