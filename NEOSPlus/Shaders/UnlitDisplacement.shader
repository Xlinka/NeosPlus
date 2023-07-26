Shader "Unlit/UnlitDisplacement" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _DisplacementTex("Displacement Texture", 2D) = "gray" {}
        _Displacement("Displacement", Range(0, 1)) = 0.2
        _Tess("Tessellation", Range(1, 32)) = 4
        _Scale("Texture Scale", Float) = 1.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma surface surf Lambert addshadow fullforwardshadows vertex:vert tessellate:tessFixed nolightmap nodynlightmap
            #pragma target 4.6

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Input {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            float _Displacement;
            sampler2D _DisplacementTex;
            float _Tess;
            float _Scale;

            float4 tessFixed() {
                return _Tess;
            }

            void vert(inout appdata v) {
                float d = tex2Dlod(_DisplacementTex, float4(v.texcoord.xy * _Scale, 0, 0)).r * _Displacement;
                v.vertex.xyz += v.normal * d;
            }

            void surf(Input IN, inout SurfaceOutput o) {
                float2 UV = IN.uv_MainTex * _Scale;
                half4 c = tex2D(_MainTex, UV);
                o.Emission = c.rgb;
                o.Albedo = float3(0, 0, 0);
                o.Specular = 0;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
