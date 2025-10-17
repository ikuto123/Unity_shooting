Shader "Custom/RustyMetalLit_URP6"
{
    Properties
    {
        _BaseMap ("Base Color (RGB)", 2D) = "white" {}
        _MetallicMap ("Metallic Mask (R)", 2D) = "white" {}
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _BaseColor ("Base Tint", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0,1)) = 1.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _RustColor ("Rust Color Tint", Color) = (0.6, 0.3, 0.1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile _ _FORWARD_PLUS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            // ==== Texture & Sampler (URP 2025対応構文) ====
            TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
            TEXTURE2D(_MetallicMap);    SAMPLER(sampler_MetallicMap);
            TEXTURE2D(_RoughnessMap);   SAMPLER(sampler_RoughnessMap);
            TEXTURE2D(_NormalMap);      SAMPLER(sampler_NormalMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _Metallic;
                float _Smoothness;
                float4 _RustColor;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 bitangentWS = cross(OUT.normalWS, tangentWS) * IN.tangentOS.w;
                OUT.tangentWS = tangentWS;
                OUT.bitangentWS = bitangentWS;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // === Sample textures (URP6構文) ===
                half4 baseTex = _BaseMap.Sample(sampler_BaseMap, IN.uv);
                half metallicMask = _MetallicMap.Sample(sampler_MetallicMap, IN.uv).r;
                half roughness = _RoughnessMap.Sample(sampler_RoughnessMap, IN.uv).r;
                half3 normalTS = UnpackNormal(_NormalMap.Sample(sampler_NormalMap, IN.uv));

                // === Tangent → World Normal ===
                half3x3 TBN = half3x3(normalize(IN.tangentWS), normalize(IN.bitangentWS), normalize(IN.normalWS));
                half3 normalWS = normalize(mul(normalTS, TBN));

                // === Rust blending ===
                half rustFactor = 1 - metallicMask; // 白＝金属、黒＝錆
                half3 albedo = lerp(baseTex.rgb * _BaseColor.rgb, _RustColor.rgb, rustFactor);
                half metallic = lerp(_Metallic, 0.0, rustFactor);
                half smoothness = lerp(_Smoothness, 1 - roughness, rustFactor);

                // === PBR lighting input ===
                InputData inputData = (InputData)0;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = GetWorldSpaceViewDir(IN.positionWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedo;
                surfaceData.metallic = metallic;
                surfaceData.smoothness = smoothness;
                surfaceData.occlusion = 1.0;
                surfaceData.emission = 0;
                surfaceData.alpha = 1.0;
                surfaceData.normalTS = normalTS;

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
