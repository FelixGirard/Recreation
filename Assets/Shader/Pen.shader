// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Pen Blit"
{
    Properties
    {
        _penColor("Color", Color) = (0,0,0,1)
        _start("Start", Vector) = (0,0,0,0)
        _end("End", Vector) = (0,0,0,0)
    }
        SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _penColor;
            float2 _start;
            float2 _end;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = float2(i.uv.x, i.uv.y);
                float2 ba = _end - _start;
                float2 pa = p - _start;
                float k = saturate(dot(pa, ba) / dot(ba, ba));
                float l = length(pa - ba * k);

                float penSize = 0.01f;

                if (l < penSize) {
                    return _penColor;
                }

                return fixed4(1,1,1,0);
            }
            ENDCG
        }
    }
}