Shader "Custom/Glisten"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlistenColor ("Glisten Color", Color) = (1,1,1,1)
        _GlistenStrength ("Glisten Strength", Range(0,1)) = 0.4
        _GlistenSpeed ("Glisten Speed", Float) = 1.5
        _GlistenScale ("Glisten Scale", Float) = 8.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Sprite"
            "CanUseSpriteAtlas"="True"
        }

        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            float4 _GlistenColor;
            float _GlistenStrength;
            float _GlistenSpeed;
            float _GlistenScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv) * i.color;

                if (baseCol.a <= 0.001)
                    discard;
                
                float t = _Time.y * _GlistenSpeed;
                float shimmer =
                    sin((i.uv.x + i.uv.y) * _GlistenScale + t) * 0.5 + 0.5;

                shimmer *= baseCol.a;

                fixed3 glisten =
                    _GlistenColor.rgb * shimmer * _GlistenStrength;

                baseCol.rgb += glisten;

                return baseCol;
            }
            ENDCG
        }
    }
}
