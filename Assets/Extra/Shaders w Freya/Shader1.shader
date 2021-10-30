Shader "Unlit/Shader1"
{

    // default code which Freya doesn't use/explain is commented out, but left for posterity

    // input data
    Properties 
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("Color A", Color) = (1, 1, 1, 1)
        _ColorB ("Color B", Color) = (1, 1, 1, 1)

        _ColorStart ("Color Start", Range(0, 1) ) = 1
        _ColorEnd ("Color End", Range(0,1) ) = 0
    }

    SubShader
    {
        // help define when to queue rendering & buidling render pipeline
        Tags { "RenderType"="Opaque" }

        // LOD 100

        // Shader Code
        Pass
        {
            // begin shader code
            CGPROGRAM

            // tells compiler which function is the vertex shader & which is the fragment shader
            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            // #pragma multi_compile_fog

            // Unity specific helper functions
            #include "UnityCG.cginc"

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

            // shader data type breakdown
            // bool 0 1
            // int
            // float (32 bit float)
            // half (16 bit float)
            // fixed (lower precision) only useful from -1:1

            // float4 -> half4 -> fixed4 (vectors)
            // float4x4 -> half4x4 -> fixed4x4 (matrices)

            // Fragment Shader
            float4 frag (Interpolators i) : SV_Target // output to frame buffer
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);

                // use inverseLerp to create new value based on start & end locations & x value
                float t = InverseLerp( _ColorStart, _ColorEnd, i.uv.x );

                // used to help figure out if values are going over 1
                // frac = v - floor(v)
                // t = frac(t);

                // saturate is bad function name; in this context it just clips to 0 if t<0 || 1 if t>1
                t = saturate(t);

                // lerp colors for gradient blend based on t
                float4 outColor = lerp( _ColorA, _ColorB, t);


                // returns a color 🌈
                return outColor;
            }
            ENDCG
        }
    }
}
