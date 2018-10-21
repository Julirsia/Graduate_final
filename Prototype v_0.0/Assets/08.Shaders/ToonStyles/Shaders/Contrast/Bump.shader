Shader "ToonStyles/Contrast/Bumped" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Brightness ("Brightness", Float) = -0.75
		_Contrast ("Contrast", Float) = 4
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor dualforward

		sampler2D _MainTex;
		sampler2D _BumpMap;
		half _Brightness;
		half _Contrast;

		struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv_BumpMap;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		}
		
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return SimpleLambertLight(o, lightDir, atten);
		}
		
		void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			color.rgb = contrastcolor(color, _Brightness, _Contrast);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

