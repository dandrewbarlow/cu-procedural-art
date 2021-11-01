Shader "Unlit/Vertex Offset Me" // to avoid conflict w/ freya's files
{

    // default code which Freya doesn't use/explain is commented out, but left for posterity

    // input data
    Properties 
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("Color A", Color) = (1, 1, 1, 1)
        _ColorB ("Color B", Color) = (1, 1, 1, 1)

        _WaveAmp ("Wave Amplitude", Range(0, 0.2)) = 1
        _ColorStart ("Color Start", Range(0, 1) ) = 1
        _ColorEnd ("Color End", Range(0,1) ) = 0
    }

    SubShader
    {
        // help define when to queue rendering & buidling render pipeline
        Tags { 
            "RenderType"="Opaque" // tell render pipeline what type it is
            "Queue"="Geometry" // change render order
         }

        // LOD 100

        // Shader Code
        Pass
        {
            
            // begin shader code //////////////////////////////////////////////////
            CGPROGRAM

            // tells compiler which function is the vertex shader & which is the fragment shader
            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            // #pragma multi_compile_fog

            // Unity specific helper functions
            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            // UNIFORMS: variables for properies
                // freya finds these boilerplatey
                // freya also puts textures off until later in tutorial
            // sampler2D _MainTex;
            // float4 _MainTex_ST;

            // automatically gets value from properties
            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _WaveAmp;

            // Freya hates the original name of this struct, appdata, and renames it
            // per-vertex mesh data 
            // automatically defined by Unity
            struct MeshData
            {
                // type varName : Thing to be passed into this field

                float4 vertex : POSITION; // vertex position; usually neccessary value

                // UVs are very general, but mostly used for mapping textures to objects
                float2 uv0 : TEXCOORD0; // UV coordinates, UV channel 0

                /* POTENTIAL OTHER FIELDS */

                float3 normals : NORMAL; // vertex normals; direction of normals (perpindicular) to a surface

                /*
                float2 uv1 : TEXCOORD1; // UV channel 1
                float4 color: COLOR; // vertex color
                float4 tangent : TANGENT;
                */
            };

            // used to pass data from vertex -> fragment shader
            // all data passed to frag shader must be defined here
            // default name v2f is vague, freya also changes
            struct Interpolators
            {
                // vertex posiitons
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD0;

                // UV Coordinates
                float2 uv : TEXCOORD1; // what is written to TEXCOORD is up to you. not necessarily a texture

                // UNITY_FOG_COORDS(1)
            };


            // The vertex shader 🎉
            Interpolators vert (MeshData v)
            {
                // what data do you want to interpolate across vertices?

                Interpolators o; //o for output

                float wave = cos ( (v.uv0.y - _Time.y * 0.1) * TAU * 5);
                v.vertex.y = wave * _WaveAmp;

                // converts local space to clip space (MVP Matrix)
                // if you don't do this, it draws directly to screen, 
                // which can be useful for post-processing shaders
                o.vertex = UnityObjectToClipPos(v.vertex); 


                o.normal = UnityObjectToWorldNormal( v.normals ); // just pass through

                o.uv = v.uv0;
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v-a) / (b-a);
            }

            // Fragment Shader
            float4 frag (Interpolators i) : SV_Target // output to frame buffer
            {
                float wave = cos ( (i.uv.y - _Time.y * 0.1) * TAU * 5)*.05 + 0.5;

                wave *= 1-i.uv.y;

                return wave;
            }
            ENDCG
        }
    }
}
