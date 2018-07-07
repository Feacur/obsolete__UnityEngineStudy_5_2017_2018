#ifndef CUSTOM_MESH_DRAW_COLOR
#define CUSTOM_MESH_DRAW_COLOR

#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
#ifdef NORMALS_ON
    float3 normal : NORMAL;
#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex : SV_POSITION;
#ifdef NORMALS_ON
    float3 normal : NORMAL;
#endif
    // fixed4 color : TEXCOORD0;
};

float4 _Color;

v2f vert(appdata_t i)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    o.vertex = UnityObjectToClipPos(i.vertex);
#ifdef NORMALS_ON
    o.normal = UnityObjectToWorldNormal(i.normal);
#endif
    // o.color = (i.vertex + 1) / 2;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed4 color = _Color; // i.color;
#ifdef NORMALS_ON
    float light = dot(i.normal, normalize(float3(1, 1, -1)));
    color.rgb *= 0.3 + 0.7 * (light + 1) * 0.5;
#endif
    return color;
}

#endif