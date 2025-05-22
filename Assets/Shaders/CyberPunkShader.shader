Shader "Custom/CyberpunkFont" {
    Properties {
        _MainTex ("Font Atlas", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "gray" {}
        _RGBOffset ("RGB Offset", Range(-1, 1)) = 0.02
        _ScanlineDensity ("Scanline Density", Range(10, 100)) = 50
        _GlitchSpeed ("Glitch Speed", Range(0, 10)) = 1
        _GlitchIntensity ("Glitch Intensity", Range(0, 1)) = 0.1
        _DisplacementAmount("Displacement", Range(0,0.1)) = 0.02
        _GlowIntensity("Glow Power", Range(0,2)) = 0.5
        _FlickerSpeed("Flicker Speed", Range(0,10)) = 2.0
        _FlickerAmount("Flicker Amount", Range(0,1)) = 0.1
        
        // Add to Properties block
        _Color("Main Color", Color) = (1,1,1,1)




    }

    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
            "LightMode" = "UniversalForward" 
            "RenderType" = "Overlay"
        
        }
        Blend SrcAlpha OneMinusSrcAlpha

        // Main Pass
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_NoiseTex);
            
            CBUFFER_START(UnityPerMaterial)
                float _RGBOffset;
                float _ScanlineDensity;
                float _GlitchSpeed;
                float _GlitchIntensity;
                float _DisplacementAmount;
                float _FlickerSpeed;
                float _FlickerAmount;
                float _GlowIntensity;
                 half4 _Color;
            CBUFFER_END

            Varyings vert (Attributes IN) {
                Varyings OUT;
                
                // Vertex displacement with noise
                float2 noiseUV = IN.uv * 5 + _Time.y * 2;
                half noise = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, noiseUV, 0).r;
                float3 positionOS = IN.positionOS.xyz;
                positionOS.xy += noise * _DisplacementAmount;
                // 改用局部坐标系位移
                float3 positionWS = TransformObjectToWorld(positionOS);
                positionWS.xy += _DisplacementAmount;
                
                // Glitch effect
                float glitchWave = sin(_Time.y * _GlitchSpeed * 10) * cos(_Time.y * _GlitchSpeed * 13);
                float glitch = saturate(glitchWave * 10) * _GlitchIntensity;
                positionOS.x += glitch * (frac(_Time.y * 20) - 0.5);
                positionOS.y += glitch * (frac(_Time.y * 20 + 0.5) - 0.5);
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                // OUT.positionHCS = TransformObjectToHClip(positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target {

                // RGB Split
                float2 offsetR = float2(_RGBOffset * sin(_Time.y * 5), 0);
                float2 offsetB = float2(-_RGBOffset * cos(_Time.y * 5), 0);
                
                half r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offsetR).r;
                half g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).g;
                half b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offsetB).b;

                // Scanlines
                float scanline = sin(IN.uv.y * _ScanlineDensity + _Time.y * 50) * 0.25 + 0.75;
                
                // Glitch color
                float glitchColor = step(0.9, frac(_Time.y * _GlitchSpeed));
                half3 color = half3(r, g * scanline, b);
                color += glitchColor * half3(0.8, 0.2, 0.8);

                // Flicker
                float flicker = 1.0 - (frac(_Time.y * _FlickerSpeed) * _FlickerAmount);
                color.rgb *= flicker;

                // Alpha handling
                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;
                half3 finalColor = color * _Color.rgb;
                half finalAlpha = alpha * _Color.a;
                finalColor *= _GlowIntensity * 2.0; // 示例增强亮度
                return half4(finalColor * 5.0, finalAlpha); // 乘以系数扩展动态范围
              
                
            }
            ENDHLSL
        }

        // Glow Pass
        Pass {
    Name "Glow"
    Blend One One
    ZTest Always
    ZWrite Off

    HLSLPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct Attributes {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct Varyings {
        float4 positionHCS : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    
    CBUFFER_START(UnityPerMaterial)
        float _GlowIntensity;
    CBUFFER_END

    Varyings vert (Attributes IN) {
        Varyings OUT;
        OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
        OUT.uv = IN.uv;
        return OUT;
    }

    half4 frag (Varyings IN) : SV_Target {
        half4 sum = half4(0,0,0,0);
        
        // 偏移量与权重定义
        const float2 offsets[9] = {
            float2(-1,-1), float2(0,-1), float2(1,-1),
            float2(-1,0),  float2(0,0),  float2(1,0),
            float2(-1,1),  float2(0,1),  float2(1,1)
        };
        
        static const float weights[9] = {
            0.05, 0.1, 0.05,
            0.1,  0.4, 0.1,
            0.05, 0.1, 0.05
        };

        // 加权采样
        for(int i=0; i<9; i++) {
            sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offsets[i]*0.005) * weights[i];
        }
        
        return sum * _GlowIntensity * 0.1; // 调整最终强度系数
    }
    ENDHLSL
}
    }
}