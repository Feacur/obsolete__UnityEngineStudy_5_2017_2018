Shader "Custom/MeshDraw/Color"
{
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "DisableBatching"="true" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma shader_feature NORMALS_ON
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "MeshDraw_ColorBase.cginc"
            ENDCG
        }
    }
}