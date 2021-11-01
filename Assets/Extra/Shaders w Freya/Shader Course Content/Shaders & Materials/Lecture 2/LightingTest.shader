Shader "Unlit/LightingTest" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss ("Gloss", Range(0,1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Gloss;
            float4 _Color;

            Interpolators vert (MeshData v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal( v.normal );
                o.wPos = mul( unity_ObjectToWorld, v.vertex );
                return o;
            }

            float4 frag (Interpolators i) : SV_Target {

                // diffuse lighting
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float3 lambert = saturate( dot( N, L ) );
                float3 diffuseLight = lambert * _LightColor0.xyz * 0;

                // specular lighting
                float3 V = normalize( _WorldSpaceCameraPos - i.wPos );
                float3 H = normalize(L + V);
                //float3 R = reflect( -L, N ); // uses for Phong
                float3 specularLight = saturate(dot(H, N)) * (lambert > 0); // Blinn-Phong
                float specularExponent = exp2( _Gloss * 11 ) + 2;
                specularLight = pow( specularLight, specularExponent ) * _Gloss; // specular exponent
                specularLight *= _LightColor0.xyz;

                return float4( diffuseLight * _Color + specularLight, 1 );
            }
            ENDCG
        }
        
        
    }
}
