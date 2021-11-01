Shader "Unlit/Shader1" {
    Properties { // input data
        _ColorA ("Color A", Color ) = (1,1,1,1)
        _ColorB ("Color B", Color ) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1) ) = 0
        _ColorEnd ("Color End", Range(0,1) ) = 1
    }
    SubShader {
        // subshader tags
        Tags {
            "RenderType"="Transparent" // tag to inform the render pipeline of what type this is
            "Queue"="Transparent" // changes the render order
        }
        Pass {
            // pass tags
            
            Cull Off
            ZWrite Off
            Blend One One // additive
            
            
            
            //Blend DstColor Zero // multiply
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;            

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

            Interpolators vert( MeshData v ){
                Interpolators o;
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


                //return i.uv.y;

                float xOffset = cos( i.uv.x * TAU * 8 ) * 0.01;                
                float t = cos( (i.uv.y + xOffset - _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
                t *= 1-i.uv.y;

                float topBottomRemover = (abs(i.normal.y) < 0.999);
                float waves = t * topBottomRemover;
                float4 gradient = lerp( _ColorA, _ColorB, i.uv.y );

                return gradient * waves;
                
                //return outColor;
            }
            
            ENDCG
        }
    }
}
