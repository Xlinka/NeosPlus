// Made by LeCloutPanda
// Archived as HologramV2 fixes bugs with this one

Shader "Panda/Fragment Holo Shader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTexture ("Main Texture", 2D) = "white" {}
        _RampTexture ("Ramp", 2D) = "white" {}
        _Scale ("Texture Scale", Float) = 1.0
        _ScrollSpeed ("Scroll Speed", Float) = 1.0

        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelStrength ("Fresnel Intensity", Float) = 1.0
        [PowerSlider(4)] _FresnelPower ("Fresnel Power", Range(0.25, 10)) = 1
    }
    SubShader
    {
        Tags { 
            "Queue" = "AlphaTest+200"
            "RenderType" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

        Pass
        {   
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

            sampler2D _MainTexture, _RampTexture;
            float4 _MainTexture_ST;
            fixed4 _MainColor, _RampColor;
            float _Scale, _ScrollSpeed;
            
            fixed4 _FresnelColor;
            float _FresnelPower, _FresnelStrength;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);

                float3 worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.worldPosA = mul (unity_ObjectToWorld, v.vertex);
                o.worldPosB = normalize(worldNormal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 UV;

                UV = i.worldPosA.xy;
                fixed2 scrolledUV = tex2D(_RampTexture, i.worldPosA);

                fixed xScrollValue = 0;
                fixed yScrollValue = _ScrollSpeed * _Time;

                scrolledUV += UV* _Scale + fixed2(xScrollValue, yScrollValue);

                fixed4 rampTexture = tex2D(_RampTexture, scrolledUV);

                // Convert the ramp texture to black and white to then use it as alpha transition
                fixed4 alphaFromRamp = rampTexture;
                alphaFromRamp.rgb = dot(rampTexture.rgb, float3(0.3, 0.59, 0.11));

                fixed4 mainTexture = tex2D(_MainTexture, i.uv) * _MainColor;

                // Setting the alpha based on the ramp
                mainTexture.a = alphaFromRamp;
                
                float fresnelInfluence = dot(i.worldPosB, i.viewDir);
                float saturatedFresnel = saturate(1 - fresnelInfluence);

                // Multiply the main texture with the colored ramp texture  + 
                return pow(saturatedFresnel, _FresnelPower) * (_FresnelColor * _FresnelStrength) + mainTexture * rampTexture ;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
