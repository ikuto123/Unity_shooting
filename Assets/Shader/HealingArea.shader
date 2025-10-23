Shader "HealingArea"
{
    Properties
    {
        [MainColor]_BaseColor("Base Color", Color) = (0.2,0.9,0.6,0.7)
        [HDR]_GlowColor("Glow (Emission) Color", Color) = (0.2,1,0.7,1)

        [MainTexture]_MainTex("Main Texture", 2D) = "white" {}
        _Radius("Radius (UV)", Range(0.05,1.5)) = 0.9
        _EdgeSoftness("Edge Softness", Range(0.001,1)) = 0.25
        _InnerAlpha("Inner Alpha", Range(0,1)) = 0.85
        _OuterAlpha("Outer Alpha", Range(0,1)) = 0.0

        _PulseSpeed("Pulse Speed", Range(0,8)) = 2.0
        _RingWidth("Ring Width", Range(0.001,0.5)) = 0.08
        _RingIntensity("Ring Intensity", Range(0,3)) = 1.0
    }

    SubShader
    {
        Tags{ "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _GlowColor;
                float _Radius;
                float _EdgeSoftness;
                float _InnerAlpha;
                float _OuterAlpha;
                float _PulseSpeed;
                float _RingWidth;
                float _RingIntensity;
                float4 _MainTex_ST; // tiling/offset
            CBUFFER_END

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            Varyings vert(Attributes IN){
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                float3 ws = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(ws);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // texture (RGBA)
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // radial mask
                float2 cuv = IN.uv - 0.5;
                float r = length(cuv)/max(1e-5,_Radius);
                float edge = 1.0 - smoothstep(1.0 - _EdgeSoftness, 1.0, r);
                float baseAlpha = lerp(_InnerAlpha, _OuterAlpha, saturate(r));

                // pulse ring
                float s = frac(_Time.y * _PulseSpeed);
                float ring = 1.0 - smoothstep(_RingWidth, _RingWidth+0.03, abs(r - s));
                ring *= _RingIntensity;

                float glowTerm = saturate(edge + ring);

                float3 col = tex.rgb * _BaseColor.rgb * 0.7 + _GlowColor.rgb * glowTerm;
                float alpha = tex.a * _BaseColor.a * baseAlpha * edge;

                return half4(col, saturate(alpha));
            }
            ENDHLSL
        }
    }
    FallBack Off
}
