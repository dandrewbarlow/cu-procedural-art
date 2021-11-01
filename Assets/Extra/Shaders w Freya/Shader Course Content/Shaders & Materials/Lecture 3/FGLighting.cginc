#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

#define USE_LIGHTING
#define TAU 6.28318530718

struct MeshData {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT; // xyz = tangent direction, w = tangent sign
    float2 uv : TEXCOORD0;
};

struct Interpolators {
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : TEXCOORD1;
    float3 tangent : TEXCOORD2;
    float3 bitangent : TEXCOORD3;
    float3 wPos : TEXCOORD4;
    LIGHTING_COORDS(5,6)
    float3 localPos : TEXCOORD7;
};

sampler2D _RockAlbedo;
float4 _RockAlbedo_ST;
sampler2D _RockNormals;
sampler2D _RockHeight;
sampler2D _DiffuseIBL;
sampler2D _SpecularIBL;
float _Gloss;
float4 _Color;
float4 _AmbientLight;
float _NormalIntensity;
float _DispStrength;
float _SpecIBLIntensity;

float2 Rotate( float2 v, float angRad ) {
    float ca = cos( angRad );
    float sa = sin( angRad );
    return float2( ca * v.x - sa * v.y, sa * v.x + ca * v.y );
}

float2 DirToRectilinear( float3 dir ) {
    float x = atan2( dir.z, dir.x ) / TAU + 0.5; // 0-1
    float y = dir.y * 0.5 + 0.5; // 0-1
    return float2(x,y);
}

Interpolators vert (MeshData v) {
    Interpolators o;
    o.uv = TRANSFORM_TEX(v.uv, _RockAlbedo);

    float height = tex2Dlod( _RockHeight, float4(o.uv, 0, 0 ) ).x * 2 - 1;

    v.vertex.xyz += v.normal * (height * _DispStrength);

    o.localPos = v.vertex.xyz;
    o.vertex = UnityObjectToClipPos(v.vertex);
    
    o.normal = UnityObjectToWorldNormal( v.normal );
    o.tangent = UnityObjectToWorldDir( v.tangent.xyz );
    o.bitangent = cross( o.normal, o.tangent );
    o.bitangent *= v.tangent.w * unity_WorldTransformParams.w; // correctly handle flipping/mirroring
    
    o.wPos = mul( unity_ObjectToWorld, v.vertex );
    TRANSFER_VERTEX_TO_FRAGMENT(o); // lighting, actually
    return o;
}

float4 frag (Interpolators i) : SV_Target {


    float3 V = normalize( _WorldSpaceCameraPos - i.wPos );

    float3 rock = tex2D( _RockAlbedo, i.uv ).rgb;
    float3 surfaceColor = rock * _Color.rgb;
    
    float3 tangentSpaceNormal = UnpackNormal( tex2D( _RockNormals, i.uv ) );
    tangentSpaceNormal = normalize( lerp( float3(0,0,1), tangentSpaceNormal, _NormalIntensity ) );
    
    float3x3 mtxTangToWorld = {
        i.tangent.x, i.bitangent.x, i.normal.x,
        i.tangent.y, i.bitangent.y, i.normal.y,
        i.tangent.z, i.bitangent.z, i.normal.z
    };

    float3 N = mul( mtxTangToWorld, tangentSpaceNormal );

    #ifdef USE_LIGHTING
        // diffuse lighting
        //float3 N = normalize(i.normal);
 
        float3 L = normalize( UnityWorldSpaceLightDir( i.wPos ) );
        float attenuation = LIGHT_ATTENUATION(i);
        float3 lambert = saturate( dot( N, L ) );
        float3 diffuseLight = (lambert * attenuation) * _LightColor0.xyz;

        #ifdef IS_IN_BASE_PASS
            float3 diffuseIBL = tex2Dlod( _DiffuseIBL, float4(DirToRectilinear( N ),0,0) ).xyz;
            diffuseLight += diffuseIBL; // adds the indirect diffuse lighting
        #endif

        // specular lighting
        
        float3 H = normalize(L + V);
        //float3 R = reflect( -L, N ); // uses for Phong
        float3 specularLight = saturate(dot(H, N)) * (lambert > 0); // Blinn-Phong

        float specularExponent = exp2( _Gloss * 11 ) + 2;
        specularLight = pow( specularLight, specularExponent ) * _Gloss * attenuation; // specular exponent
        specularLight *= _LightColor0.xyz;
    
        #ifdef IS_IN_BASE_PASS
            float fresnel = pow(1-saturate(dot(V,N)),5);
            float3 viewRefl = reflect( -V, N );
            float mip = (1-_Gloss)*6;
            float3 specularIBL = tex2Dlod( _SpecularIBL, float4(DirToRectilinear( viewRefl ),mip,mip) ).xyz;
            specularLight += specularIBL * _SpecIBLIntensity * fresnel;
        #endif
    

        return float4( diffuseLight * surfaceColor + specularLight, 1 );
    #else
        #ifdef IS_IN_BASE_PASS
            return surfaceColor;
        #else
            return 0;
        #endif
    #endif
    
}