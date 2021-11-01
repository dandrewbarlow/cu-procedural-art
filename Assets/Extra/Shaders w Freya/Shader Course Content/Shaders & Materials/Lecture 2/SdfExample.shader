Shader "Unlit/SdfExample" {
    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Interpolators vert (MeshData v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target {

                float dist = length(i.uv) - 0.3;
                
                //return step(0, dist); // a <= b
                
                return float4(dist.xxx,0);
            }
            ENDCG
        }
    }
}
