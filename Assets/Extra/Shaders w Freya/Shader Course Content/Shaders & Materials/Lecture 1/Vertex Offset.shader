Shader "Unlit/Vertex Offset" {
    Properties { // input data
        _ColorA ("Color A", Color ) = (1,1,1,1)
        _ColorB ("Color B", Color ) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1) ) = 0
        _ColorEnd ("Color End", Range(0,1) ) = 1
        _WaveAmp ("Wave Amplitude", Range(0,0.2) ) = 0.1
    }
    SubShader {
        // subshader tags
        Tags {
            "RenderType"="Opaque" // tag to inform the render pipeline of what type this is
        }
        Pass {
            // pass tags
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;            
            float _WaveAmp;            

            // automatically filled out by Unity
            struct MeshData { // per-vertex mesh data
                float4 vertex : POSITION; // local space vertex position
                float3 normals : NORMAL; // local space normal direction
                // float4 tangent : TANGENT; // tangent direction (xyz) tangent sign (w)
                // float4 color : COLOR; // vertex colors
                float4 uv0 : TEXCOORD0; // uv0 diffuse/normal map textures
                //float4 uv1 : TEXCOORD1; // uv1 coordinates lightmap coordinates
                //float4 uv2 : TEXCOORD2; // uv2 coordinates lightmap coordinates
                //float4 uv3 : TEXCOORD3; // uv3 coordinates lightmap coordinates
            };

            // data passed from the vertex shader to the fragment shader
            // this will interpolate/blend across the triangle!
            struct Interpolators {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            float GetWave( float2 uv ) {
                float2 uvsCentered = uv * 2 - 1; 
                float radialDistance = length( uvsCentered );
                float wave = cos( (radialDistance - _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
                wave *= 1-radialDistance;
                return wave;
            }

            Interpolators vert( MeshData v ){
                Interpolators o;

               // float wave = cos( (v.uv0.y - _Time.y * 0.1) * TAU * 5);

               // v.vertex.y = wave * _WaveAmp;

                v.vertex.y = GetWave( v.uv0 ) * _WaveAmp;
                
                o.vertex = UnityObjectToClipPos( v.vertex ); // local space to clip space
                o.normal = UnityObjectToWorldNormal( v.normals );
                o.uv = v.uv0; //(v.uv0 + _Offset) * _Scale; // passthrough
                return o;
            }

            float InverseLerp( float a, float b, float v ) {
                return (v-a)/(b-a);
            }



            float4 frag( Interpolators i ) : SV_Target {
                // blend between two colors based on the X UV coordinate
                // float t = saturate( InverseLerp( _ColorStart, _ColorEnd, i.uv.x ) );
                // float t = abs(frac(i.uv.x * 5) * 2 - 1)
                // return float4( i.normal, 0 );

                return GetWave( i.uv );
                
            }
            
            ENDCG
        }
    }
}
