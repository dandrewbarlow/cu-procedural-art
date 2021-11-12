Shader "Unlit/hw7Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // float4 col = float4(i.uv, 0, 1);
                // float offset = i.uv.x / 2;
                float4 c = float4(
                    abs(sin(i.uv.x + _Time.y * 0.1) ),
                    abs(sin(i.uv.y + _Time.y * 0.1) ),
                    // abs(sin(i.uv.x + _Time.y)),
                    1,
                    1
                );

                return c;
            }
            ENDCG
        }
    }
}
