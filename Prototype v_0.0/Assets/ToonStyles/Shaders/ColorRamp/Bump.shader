Shader "ToonStyles/ColorRamp/Bumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Ramp ("Coloring Ramp", 2D) = "white" {}
		_Highlight ("Shine", Range(1, 64)) = 40
	}
	
	SubShader
	{
	    Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }  
	  	
	  	CGPROGRAM
		#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom approxview halfasview dualforward 		
	  	sampler2D _MainTex;
	    sampler2D _BumpMap;
	    sampler2D _Ramp;
	    uniform half _Highlight;
	  	
	  	struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv_BumpMap;
		};	
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		}
		
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half3 viewDir, half atten)
		{
			return ColorRampLight(o, lightDir, viewDir, atten, _Highlight, _Ramp, 0.25 * (64 - _Highlight));
		}		
		
		ENDCG
	} 
	FallBack "Diffuse"
}

