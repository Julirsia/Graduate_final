Shader "ToonStyles/Contrast/Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Brightness ("Brightness", Float) = -0.75
		_Contrast ("Contrast", Float) = 4
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Lambert finalcolor:customColor approxview halfasview nodirlightmap
		
		sampler2D _MainTex;
		half _Brightness;
		half _Contrast;

		struct Input
		{
			fixed2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
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
