Shader "Custom/LocalVerticalGradientURP_Transparent"
{
    Properties
    {
        _LightYellow ("Light Yellow (Top)", Color) = (1, 0.94, 0.58, 1)
        _DarkYellow ("Dark Yellow (Bottom)", Color) = (1, 0.7, 0, 1)
        _GradientBottomY ("Gradient Bottom Y (Local)", Float) = -0.5
        _GradientTopY ("Gradient Top Y (Local)", Float) = 0.5
        
        [Header(Transparency)]
        _Alpha ("Alpha", Range(0.0, 1.0)) = 0.5
        
        [HideInInspector] _BaseMap ("Base Map", 2D) = "white" {}
        [HideInInspector] _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _LightYellow;
                float4 _DarkYellow;
                float _GradientBottomY;
                float _GradientTopY;
                float _Alpha;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionOS   : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionOS = IN.positionOS.xyz;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            // --- ↓↓↓ ここを修正しました ↓↓↓ ---
            half4 frag(Varyings IN) : SV_Target
            {
                // 1. ローカルY座標
                float localY = IN.positionOS.y;
                
                // 2. グラデーションの範囲を計算
                float range = _GradientTopY - _GradientBottomY;
                
                // 3. 係数tを計算 (デフォルトは0 = Bottom色)
                float t = 0.0; 
                
                // 4. ゼロ除算を回避 (rangeが 0.0001 より大きい場合のみ計算)
                if (abs(range) > 0.0001)
                {
                    // rcp() の代わりに安全な除算を使用
                    t = (localY - _GradientBottomY) / range;
                }
                
                // 5. 係数tを 0～1 の範囲にクランプ
                t = saturate(t);

                // 6. グラデーション色を計算
                half4 finalColor = lerp(_DarkYellow, _LightYellow, t);
                
                // 7. アルファ値を適用
                finalColor.a = _Alpha;

                return finalColor;
            }
            // --- ↑↑↑ 修正完了 ↑↑↑ ---
            
            ENDHLSL
        }
    }
}