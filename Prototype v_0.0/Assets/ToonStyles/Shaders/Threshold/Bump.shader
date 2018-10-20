Shader "ToonStyles/Threshold/Bumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	    _Threshold ("Threshold", Range(0, 1)) = 0.5
	    _LightBalance ("Light / Texture Balance", Range(0, 1)) = 0.5
	    _Color ("Color 1", Color) = (1,1,1,1)
	    _Color1 ("Color 2", Color) = (0,0,0,1)
	}
	SubShader
	{
	    Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }  
	  	CGPROGRAM
	  	#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview dualforward
	
		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed _Threshold;
		fixed _LightBalance;
		fixed4 _Color;
		fixed4 _Color1;
	    	
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
 	  	
 	  	fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return BalancedLight(o, lightDir, atten, _LightBalance);
		}
 	  	
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color = thresholdColor(color, _Threshold, _Color, _Color1);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}
