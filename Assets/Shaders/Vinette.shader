Shader "Custom/VignetteOverlay"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        [MainColor] _BaseColor ("Color Tint", Color) = (1, 1, 1, 1)
        // Default color is Black
        _VignetteColor ("Vignette Color", Color) = (0, 0, 0, 1)
        
        // Power: How large the clear circle is (0.1 = small circle, 0.8 = large circle)
        _VignettePower ("Circle Size", Range(0.0, 1.0)) = 0.1
        
        // Smoothness: How soft the edge is
        _VignetteSmoothness ("Smoothness", Range(0.0, 1.0)) = 0.2
        
        _VignetteCenter ("Center Point", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off 

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _VignetteColor;
                float _VignettePower;
                float _VignetteSmoothness;
                float2 _VignetteCenter;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv; // Use raw UVs
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 1. Calculate distance from center
                float dist = distance(IN.uv, _VignetteCenter);

                // 2. Create the mask
                // smoothstep returns 0.0 if we are inside the circle, and blends to 1.0 at the edge
                // We tweak the logic here: 
                // We want 0 alpha at the center (distance 0) and high alpha at the edge.
                
                half alphaMask = smoothstep(_VignettePower, _VignettePower + _VignetteSmoothness, dist);

                // 3. Apply the mask to the Color's Alpha
                half4 finalColor = _VignetteColor;
                finalColor.a *= alphaMask;

                return finalColor;
            }
            ENDHLSL
        }
    }
}