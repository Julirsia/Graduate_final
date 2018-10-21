Shader "ToonStyles/Lookup/Bumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	  	_Offset ("Lookup Offset", Float) = 0
	  	_Scale ("Lookup Scale", Float) = 1
      	_Lookup ("Lookup Texture", 2D) = "white" {}
	}
	SubShader
	{
	    Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }  
	  	CGPROGRAM
	  	#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview dualforward 
		
	    sampler2D _MainTex;
	    sampler2D _Lookup;
	    sampler2D _BumpMap;
	    fixed _Offset;
	    fixed _Scale;
	    	
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
			return SimpleLambertLight(o, lightDir, atten);
		}
		
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color.rgb = lookupColor(color, _Lookup, _Scale, _Offset);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}
