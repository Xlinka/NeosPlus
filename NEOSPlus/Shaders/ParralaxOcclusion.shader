Shader "Custom/ParallaxOcclusion" {
	Properties{
		_Color("Color", Color) = (1,0,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_TextureScale("Texture Scale", Float) = 1
		_BumpMap("Normal map (RGB)", 2D) = "bump" {}
		_BumpScale("Bump scale", Range(0,1)) = 1
		_ParallaxMap("Height map (R)", 2D) = "white" {}
		_Parallax("Height scale", Range(0,1)) = 0.05
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_ParallaxMinSamples("Parallax min samples", Range(2,100)) = 4
		_ParallaxMaxSamples("Parallax max samples", Range(2,100)) = 20
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _ParallaxMap;

		struct Input {
			float2 texcoord;
			float3 eye;
			float sampleRatio;
		};

		half _Glossiness;
		half _Metallic;
		half _BumpScale;
		half _Parallax;
		fixed4 _Color;
		uint _ParallaxMinSamples;
		uint _ParallaxMaxSamples;
		float _TextureScale;

		void parallax_vert(
			float4 vertex,
			float3 normal,
			float4 tangent,
			out float3 eye,
			out float sampleRatio
		) {
			float4x4 mW = unity_ObjectToWorld;
			float3 binormal = cross( normal, tangent.xyz ) * tangent.w;
			float3 EyePosition = _WorldSpaceCameraPos;
			
			// Need to do it this way for W-normalisation and.. stuff.
			float4 localCameraPos = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
			float3 eyeLocal = vertex - localCameraPos;
			float4 eyeGlobal = mul( float4(eyeLocal, 1), mW  );
			float3 E = eyeGlobal.xyz;
			
			float3x3 tangentToWorldSpace;

			tangentToWorldSpace[0] = mul( normalize( tangent ), mW );
			tangentToWorldSpace[1] = mul( normalize( binormal ), mW );
			tangentToWorldSpace[2] = mul( normalize( normal ), mW );
			
			float3x3 worldToTangentSpace = transpose(tangentToWorldSpace);
			
			eye	= mul( E, worldToTangentSpace );
			sampleRatio = 1-dot( normalize(E), -normal );
		}

		float2 parallax_offset (
			float fHeightMapScale,
			float3 eye,
			float sampleRatio,
			float2 texcoord,
			sampler2D heightMap,
			int nMinSamples,
			int nMaxSamples
		) {

			float fParallaxLimit = -length( eye.xy ) / eye.z;
			fParallaxLimit *= fHeightMapScale;
			
			float2 vOffsetDir = normalize( eye.xy );
			float2 vMaxOffset = vOffsetDir * fParallaxLimit;
			
			int nNumSamples = (int)lerp( nMinSamples, nMaxSamples, saturate(sampleRatio) );
			
			float fStepSize = 1.0 / (float)nNumSamples;
			
			float2 dx = ddx( texcoord );
			float2 dy = ddy( texcoord );
			
			float fCurrRayHeight = 1.0;
			float2 vCurrOffset = float2( 0, 0 );
			float2 vLastOffset = float2( 0, 0 );

			float fLastSampledHeight = 1;
			float fCurrSampledHeight = 1;

			int nCurrSample = 0;
			
			while ( nCurrSample < nNumSamples )
			{
			fCurrSampledHeight = tex2Dgrad(heightMap, texcoord + vCurrOffset, dx, dy ).r;
			if ( fCurrSampledHeight > fCurrRayHeight )
			{
				float delta1 = fCurrSampledHeight - fCurrRayHeight;
				float delta2 = ( fCurrRayHeight + fStepSize ) - fLastSampledHeight;

				float ratio = delta1/(delta1+delta2);

				vCurrOffset = (ratio) * vLastOffset + (1.0-ratio) * vCurrOffset;

				nCurrSample = nNumSamples + 1;
			}
			else
			{
				nCurrSample++;

				fCurrRayHeight -= fStepSize;

				vLastOffset = vCurrOffset;
				vCurrOffset += fStepSize * vMaxOffset;

				fLastSampledHeight = fCurrSampledHeight;
			}
			}
			
			return vCurrOffset;
		}

		void vert(inout appdata_full IN, out Input OUT) {
			parallax_vert(IN.vertex, IN.normal, IN.tangent, OUT.eye, OUT.sampleRatio);
			OUT.texcoord = IN.texcoord;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float2 offset = parallax_offset(_Parallax, IN.eye, IN.sampleRatio, IN.texcoord * _TextureScale,
			_ParallaxMap, _ParallaxMinSamples, _ParallaxMaxSamples);
			float2 uv = IN.texcoord * _TextureScale + offset;
			fixed4 c = tex2D(_MainTex, uv) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, uv), _BumpScale);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
