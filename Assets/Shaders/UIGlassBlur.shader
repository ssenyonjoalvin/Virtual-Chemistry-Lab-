Shader "Custom/UI/GlassBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (0.85, 0.95, 1.0, 0.28)
        _BlurSize ("Blur Size", Range(0, 5)) = 1.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "UIBlur"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.ugui/ShaderLibrary/UI.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float4 _MainTex_ST;
                float _BlurSize;
            CBUFFER_END

            v2f vert(appdata IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                return OUT;
            }

            half4 SampleScene(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv);
            }

            half4 frag(v2f IN) : SV_Target
            {
                float2 sceneUV = IN.screenPos.xy / IN.screenPos.w;
                float2 pixel = _ScreenParams.zw * _BlurSize;

                half4 c0 = SampleScene(sceneUV + float2(-pixel.x, -pixel.y));
                half4 c1 = SampleScene(sceneUV + float2( pixel.x, -pixel.y));
                half4 c2 = SampleScene(sceneUV + float2(-pixel.x,  pixel.y));
                half4 c3 = SampleScene(sceneUV + float2( pixel.x,  pixel.y));
                half4 c4 = SampleScene(sceneUV);
                half4 blurred = (c0 + c1 + c2 + c3 + c4) * 0.2h;

                half4 spriteMask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half alpha = IN.color.a * spriteMask.a;
                half3 finalRgb = lerp(blurred.rgb, IN.color.rgb, 0.35h);
                return half4(finalRgb, alpha);
            }
            ENDHLSL
        }
    }
}
