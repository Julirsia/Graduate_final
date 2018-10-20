Shader "ToonStyles/Threshold/SimpleFluid"
{
	Properties
	{
      	_Threshold ("Threshold", Range(0, 1)) = 0.5
	  	_LightBalance ("Light / Texture Balance", Range(0, 1)) = 0.5
      	_Color ("Color 1", Color) = (1,1,1,1)
      	_Color1 ("Color 2", Color) = (0,0,0,1)
      	
      	_BumpMap ("Bump", 2D) = "bump" {}
		_WaveSpeedX ("Wave Speed X", Float) = 0.2
		_WaveSpeedY ("Wave Speed Y", Float) = 0.2
		_WaveScale("Wave Scale", Float) = 1
		_ColorControl ("Reflective color (RGB) fresnel (A) ", 2D) = "" { }
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
	  	CGPROGRAM
		#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap 
		
		sampler2D _BumpMap;
		sampler2D _ColorControl;
	    fixed _Threshold;
	    fixed _LightBalance;
	    fixed4 _Color;
	    fixed4 _Color1;
	    half _WaveSpeedX;
	    half _WaveSpeedY;
	    half _WaveScale;
	    	
	  	struct Input
	  	{
	    	fixed2 uv_BumpMap;
	    	half3 viewDir;
	  	};
   
	  	void surf (Input IN, inout SurfaceOutput o) 
	  	{
			fluidSurf(o, _BumpMap, _ColorControl, IN.uv_BumpMap, _WaveScale, half2(_WaveSpeedX, _WaveSpeedY), IN.viewDir);
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

