Shader "Unlit/Healthbar" {
    Properties {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Health ("Health", Range(0,1)) = 1
        _BorderSize ("Border Size", Range(0,0.5)) = 0.1
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass {
            ZWrite Off
            // src * srcAlpha + dst * (1-srcAlpha)
            Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
            
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

            sampler2D _MainTex;
            float _Health;
            float _BorderSize;

            Interpolators vert (MeshData v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float InverseLerp( float a, float b, float v ) {
                return (v-a)/(b-a);
            }

            float4 frag (Interpolators i) : SV_Target {

                // rounded corner clipping
                float2 coords = i.uv;
                coords.x *= 8;
                float2 pointOnLineSeg = float2( clamp( coords.x, 0.5, 7.5 ), 0.5);
                float sdf = distance(coords, pointOnLineSeg) * 2 - 1;
                clip(-sdf);

                float borderSdf = sdf + _BorderSize;
                float pd = fwidth(borderSdf); // screen space partial derivative
                float borderMask = 1-saturate(borderSdf / pd);

                //return float4(borderMask.xxx,1);
                
                float healthbarMask = _Health > i.uv.x;
                float3 healthbarColor = tex2D( _MainTex, float2( _Health, i.uv.y) );

                if( _Health < 0.2 ) {
                    float flash = cos( _Time.y * 4 ) * 0.4 + 1;
                    healthbarColor *= flash;
                }

                return float4( healthbarColor * healthbarMask * borderMask, 1 );
            }
            ENDCG
        }
    }
}
