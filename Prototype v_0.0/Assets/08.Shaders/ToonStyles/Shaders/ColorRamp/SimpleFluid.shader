Shader "ToonStyles/ColorRamp/SimpleFluid"
{
	Properties
	{
		_Ramp ("Coloring Ramp", 2D) = "white" {}
		_Highlight ("Shine", Range(0, 64)) = 40
		
      	_BumpMap ("Bump", 2D) = "bump" {}
		_WaveSpeedX ("Wave Speed X", Float) = 0.2
		_WaveSpeedY ("Wave Speed Y", Float) = 0.2
		_WaveScale("Wave Scale", Float) = 1
		_ColorControl ("Reflective color (RGB) fresnel (A) ", 2D) = "" { }
		_Horizon ("Tint Color", Color) = (1,1,1,1)
	}
	SubShader
	{
	    Tags { "RenderType"="Opaque" }
		
	  	CGPROGRAM
		#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom approxview halfasview nodirlightmap
		
		sampler2D _Ramp;
		half _Highlight;
		
		sampler2D _BumpMap;
		sampler2D _ColorControl;
		fixed4 _Horizon;
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
			fluidSurf(o, _BumpMap, _ColorControl, IN.uv_BumpMap, _WaveScale, half2(_WaveSpeedX, _WaveSpeedY), IN.viewDir, _Horizon);
	  	}
	  	
	  	fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half3 viewDir, half atten)
		{
			return ColorRampLight (o, lightDir, viewDir, atten, _Highlight, _Ramp, 0.25 * (64 - _Highlight));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
