Shader "ToonStyles/Lookup/DetailsBumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Detail ("Detail", 2D) = "white" {}
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
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap dualforward 

	    sampler2D _MainTex;
	    sampler2D _BumpMap;
	    sampler2D _Detail;
	    sampler2D _Lookup;
	    fixed _Offset;
	    fixed _Scale;
	    	
	  	struct Input
	  	{
	    	fixed2 uv_MainTex;
	    	fixed2 uv_Detail;
	    	fixed2 uv_BumpMap;
	  	};

	  	void surf (Input IN, inout SurfaceOutputDetail o) 
	  	{
	     	o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	  		fixed4 detail = tex2D (_Detail, IN.uv_Detail);
	     	o.Alpha = detail.a;
	     	o.Detail = detail.rgb;
	     	o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
	  	}
	  	
	  	fixed4 LightingCustom (inout SurfaceOutputDetail o, half3 lightDir, half atten)
 	  	{
 	  		return LookupDetailsLight(o, lightDir, atten);
		}

	  	void customColor (Input IN, SurfaceOutputDetail o, inout fixed4 color)
	  	{
	  		color.rgb = lookupColorDetails(color, _Lookup, _Scale, _Offset, o);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}

