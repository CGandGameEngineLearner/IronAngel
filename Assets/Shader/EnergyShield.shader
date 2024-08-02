Shader "Cus/EnergyShield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _RotateSpeed("Rotate Speed", float) = 1.0
        [HDR]_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [IntRange]_Intensity("Intensity", range(1.0, 10.0)) = 1.0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "RenderQueue"="Transparent"
            "RenderPipeLine"="UniversalRenderPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        HLSLINCLUDE 
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        ENDHLSL
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial) 
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            float _RotateSpeed;
            float4 _Color;
            float _Intensity;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS);              

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 UV = i.uv * 2 - 1;
;
                float2 offset = i.uv - 0.5;
                float angle = atan2(offset.x, offset.y);
                angle += _Time.y * _RotateSpeed;
                float2 rotatedUV = float2(cos(angle), sin(angle)) * length(offset) + 0.5;


                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, rotatedUV);
                half noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, rotatedUV).r;
                col *= _Color;
                float mask = length(UV);
                col.a = pow(mask, _Intensity) * step(mask, 1) * noise;
                return col;
            }
            ENDHLSL
        }
    }
}
