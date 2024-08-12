Shader "Cus/FlowingSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", float) = 1.0
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
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
            float _Speed;
            float4 _Color;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);


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
                half4 col = (1, 1, 1, 1);
                float2 newUV = i.uv;
                newUV.x = frac(newUV.x + _Time.y * _Speed);
                col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, newUV);
                col *= _Color;
                return col;
            }
            ENDHLSL
        }
    }
}
