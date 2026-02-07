Shader "Custom/Background"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.1, 0.5, 1)
        _Variation ("Color Variation", Range(0, 1)) = 0.2
        _PixelSize ("Pixel Density", Float) = 64
        _RippleSpeed ("Speed", Float) = 5.0
        _RippleDensity ("Density", Float) = 20.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _BaseColor;
            float _Variation;
            float _PixelSize;
            float _RippleSpeed;
            float _RippleDensity;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = floor(i.uv * _PixelSize) / _PixelSize;
                float dist = distance(uv, float2(0.5, 0.5));
                float wave = sin(dist * _RippleDensity - _Time.y * _RippleSpeed);
                fixed4 finalColor = _BaseColor + (wave * _Variation);
                return finalColor;
            }
            ENDCG
        }
    }
}