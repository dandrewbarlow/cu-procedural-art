Shader "Unlit/FGSky"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 viewDir : TEXCOORD0;
            };

            struct v2f
            {
                float3 viewDir : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            #define TAU 6.28318530718

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewDir = v.viewDir;
                return o;
            }

            float2 DirToRectilinear( float3 dir ) {
                float x = atan2( dir.z, dir.x ) / TAU + 0.5; // 0-1
                float y = dir.y * 0.5 + 0.5; // 0-1
                return float2(x,y);
            }

            float3 frag (v2f i) : SV_Target {
                // sample the texture
                float3 col = tex2Dlod(_MainTex, float4(DirToRectilinear(i.viewDir),0,0) );
                return col;
            }
            ENDCG
        }
    }
}
