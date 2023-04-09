// Cleaned-up version of Neitri's Wireframe Overlay Shader by xLinka
// Original by Neitri, free of charge, free to redistribute
// Source: https://github.com/netri/Neitri-Unity-Shaders

Shader "Neitri/Wireframe Overlay V2"
{
    Properties
    {
        _WireframeColor("Wireframe Color", Color) = (1, 1, 1, 1)
        _BackgroundColor("Background Color", Color) = (0, 0, 0, 1)
    }
        SubShader
    {
        Tags
        {
            "Queue" = "Transparent+1000"
            "RenderType" = "Transparent"
        }

        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex :POSITION;
            };
            struct v2f
            {
                float4 clipPos : SV_POSITION;
                float4 modelPos : TEXCOORD0;
            };

            sampler2D _CameraDepthTexture;

            v2f vert(appdata v)
            {
                v2f o;
                o.clipPos = UnityObjectToClipPos(v.vertex);
                o.modelPos = v.vertex;
                return o;
            }

            float4x4 inverse(float4x4 input)
            {
                #define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
                float4x4 cofactors = float4x4(
                    minor(_22_23_24, _32_33_34, _42_43_44),
                    -minor(_21_23_24, _31_33_34, _41_43_44),
                    minor(_21_22_24, _31_32_34, _41_42_44),
                    -minor(_21_22_23, _31_32_33, _41_42_43),

                    -minor(_12_13_14, _32_33_34, _42_43_44),
                    minor(_11_13_14, _31_33_34, _41_43_44),
                    -minor(_11_12_14, _31_32_34, _41_42_44),
                    minor(_11_12_13, _31_32_33, _41_42_43),

                    minor(_12_13_14, _22_23_24, _42_43_44),
                    -minor(_11_13_14, _21_23_24, _41_43_44),
                    minor(_11_12_14, _21_22_24, _41_42_44),
                    -minor(_11_12_13, _21_22_23, _41_42_43),

                    -minor(_12_13_14, _22_23_24, _32_33_34),
                    minor(_11_13_14, _21_23_24, _31_33_34),
                    -minor(_11_12_14, _21_22_24, _31_32_34),
                    minor(_11_12_13, _21_22_23, _31_32_33)
                );
                #undef minor
                return transpose(cofactors) / determinant(input);
            }

            float3 calculateWorldSpace(float4 vertex,float2 screenOffset)
            {
            float4 worldPos = mul(unity_ObjectToWorld, float4(vertex.xyz, 1));
            float4 screenPos = mul(UNITY_MATRIX_VP, worldPos);
            screenPos.xy += screenOffset * screenPos.w;
            worldPos = mul(inverse(UNITY_MATRIX_VP), screenPos);
            float3 worldDir = worldPos.xyz - _WorldSpaceCameraPos;
            float2 screenUV = screenPos.xy / screenPos.w;
            screenUV.y *= _ProjectionParams.x;
            screenUV = screenUV * 0.5f + 0.5f;
            screenUV = UnityStereoTransformScreenSpaceTex(screenUV);
            float depth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, screenUV))) / screenPos.w;
            float3 worldSpacePos = worldDir * depth + _WorldSpaceCameraPos;
            return worldSpacePos;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float2 offset = 1.2 / _ScreenParams.xy;

                float3 worldPos1 = calculateWorldSpace(i.modelPos, float2(0, 0));
                float3 worldPos2 = calculateWorldSpace(i.modelPos, float2(0, offset.y));
                float3 worldPos3 = calculateWorldSpace(i.modelPos, float2(-offset.x, 0));

                float3 worldNormal = normalize(cross(worldPos2 - worldPos1, worldPos3 - worldPos1));

                return float4(worldNormal, 1.0f);
            }

                ENDCG
    }

        GrabPass
            {
                "_WorldSpaceNormal"
            }

                Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct v2f
                {
                    float4 grabPos : TEXCOORD0;
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.grabPos = ComputeGrabScreenPos(o.pos);
                    return o;
                }

                sampler2D _WorldSpaceNormal;

                fixed4 _BackgroundColor;
                fixed4 _WireframeColor;

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 grabPos = i.grabPos.xy / i.grabPos.w;

                    float2 offset = 1.0 / _ScreenParams.xy;

                    float3 pos00 = tex2D(_WorldSpaceNormal, grabPos).rgb;
                    float3 pos01 = tex2D(_WorldSpaceNormal, grabPos + float2(0, offset.y)).rgb;
                    float3 pos10 = tex2D(_WorldSpaceNormal, grabPos - float2(offset.x, 0)).rgb;

                    float3 one = float3(1, 1, 1);
                    float w = dot(one, abs(pos10 - pos00)) + dot(one, abs(pos01 - pos00));

                    return lerp(_BackgroundColor, _WireframeColor, w);
                }
                ENDCG
            }
    }
}