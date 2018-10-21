Shader "ToonStyles/Lookup/SimpleFluidTransparent"
{
	Properties
	{
		_Offset ("Lookup Offset", Float) = 0
	  	_Scale ("Lookup Scale", Float) = 1
      	_Lookup ("Lookup Texture", 2D) = "white" {}
      	
      	_BumpMap ("Bump", 2D) = "bump" {}
		_WaveSpeedX ("Wave Speed X", Float) = 0.2
		_WaveSpeedY ("Wave Speed Y", Float) = 0.2
		_WaveScale("Wave Scale", Float) = 1
		_Transparency ("Transparency", Range(0, 1)) = 0.8
	}
	SubShader
	{
	    Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="true"}
		Blend SrcAlpha OneMinusSrcAlpha
		
	  	CGPROGRAM
		#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap 
			   
		sampler2D _BumpMap;
	    half _WaveSpeedX;
	    half _WaveSpeedY;
	    half _WaveScale;
	    fixed _Transparency;
	    	
	    sampler2D _Lookup;
	    fixed _Offset;
	    fixed _Scale;
	    					
	  	struct Input
	  	{
	  		half2 uv_BumpMap;
	  		half3 viewDir;
	  	};

	  	void surf (Input IN, inout SurfaceOutput o) 
	  	{
	  		fluidAngledSurf(o, _BumpMap, IN.uv_BumpMap, _WaveScale, half2(_WaveSpeedX, _WaveSpeedY), IN.viewDir);
	  	}
	  	
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return SimpleLambertLight(o, lightDir, atten);
		}
		
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color.rgb = lookupColor(color, _Lookup, _Scale, _Offset);
	  		color.a = color.a * _Transparency;
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}
