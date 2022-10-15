// Made by LeCloutPanda#9456

Shader "Panda/Hologram"
{
    Properties
    {
        _MainTexture ("Main Texture", 2D) = "white" {}
        [HDR] _MainColor ("Main Color", Color) = (0,0,0,1)
        _EmissionTexture ("Emission Texture", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _RampTexture ("Ramp", 2D) = "white" {}
        _Scale ("Texture Scale", Float) = 1.0
        _ScrollSpeed ("Scroll Speed", Float) = 1.0

        [HDR] _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        [PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 100)) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
 
        Pass {
            ZWrite On
            ColorMask 0
        }

        Pass
        {   
            Tags { "LightMode"="ForwardBase" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPosA : TEXCOORD1;
                float3 worldPosB : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD3;
            };

            sampler2D _MainTexture, _EmissionTexture, _RampTexture;
            fixed4 _MainTexture_ST, _MainColor, _EmissionColor, _RampColor;
            float _Scale, _ScrollSpeed;
            
            fixed4 _FresnelColor;
            float _FresnelExponent;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);

                float3 worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.worldPosA = mul (unity_ObjectToWorld, v.vertex) / 100;
                o.worldPosB = normalize(worldNormal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // Main
                float2 UV = i.worldPosA.xy;
                fixed2 scrolledUV = tex2D(_RampTexture, i.worldPosA);
                fixed xScrollValue = 0;
                fixed yScrollValue = _ScrollSpeed * _Time;
                scrolledUV += UV* (_Scale * 100) + fixed2(xScrollValue, yScrollValue);
                fixed4 rampTexture = tex2D(_RampTexture, scrolledUV);
                fixed4 alphaFromRamp = rampTexture;
                alphaFromRamp.rgb = dot(rampTexture.rgb, float3(0.3, 0.59, 0.11));
                rampTexture.a = alphaFromRamp;

                fixed4 mainTexture;
                fixed4 emissionTexture;
                
                mainTexture = tex2D(_MainTexture, i.uv) * _MainColor;
                mainTexture.a = alphaFromRamp;
                emissionTexture = tex2D(_EmissionTexture, i.uv) * _EmissionColor;
                mainTexture.rgb += emissionTexture.rgb;

                // Fresnel
                float fresnelInfluence = dot(i.worldPosB, i.viewDir);
                float saturatedFresnel = saturate(1 - fresnelInfluence);

                return pow(saturatedFresnel, _FresnelExponent) * (_FresnelColor) + (mainTexture) * rampTexture ;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
