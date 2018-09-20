Shader "Custom/Water" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_BumpMap2("Bumpmap 2", 2D) = "bump" {}
		_BumpAnimation("Bump Animation", Range(0, 0.1)) = 0.05
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_FoamFade("Foam Fade", Range(0,5)) = 2
		_Frequency("Foam animation frequency", Range(0, 20)) = 2
		_Reflection("Reflection intensity", Range(0, 1)) = 0.5
		[HideInInspector] _ReflectionTex("", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model
		#pragma surface surf Standard alpha noshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _BumpMap;
		sampler2D _BumpMap2;

		struct Input {
			float2 uv_BumpMap;
			float2 uv_BumpMap2;
			float4 screenPos;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform sampler2D _CameraDepthTexture;
		fixed4 _FoamColor;
		float _FoamFade;
		float _Frequency;
		float _BumpAnimation;
		float _Reflection;
		sampler2D _ReflectionTex;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 r = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(IN.screenPos));
			o.Albedo = _Color.rgb + r.rgb * _Reflection;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap - _Time * _BumpAnimation));
			o.Normal += UnpackNormal(tex2D(_BumpMap2, IN.uv_BumpMap2 + _Time  * _BumpAnimation));
			o.Alpha = _Color.a;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			// Foam
			float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r);
			float partZ = IN.screenPos.z;

			float animation = 0.5*(1 + sin(2 * 3.1415 * _Frequency * _Time)) + 20 - _FoamFade * 4;
			float diff = (abs(sceneZ - partZ)) /_FoamFade * animation + o.Normal;
			if (diff <= 1)
			{
				o.Albedo = lerp(_FoamColor, _Color + r * _Reflection, float4(diff, diff, diff, diff));
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
