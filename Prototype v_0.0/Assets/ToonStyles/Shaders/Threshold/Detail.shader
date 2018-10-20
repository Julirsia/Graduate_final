Shader "ToonStyles/Threshold/Details"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Detail ("Detail", 2D) = "white" {}
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
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap 
	
		sampler2D _MainTex;
		sampler2D _Detail;
	   	fixed _Threshold;
	   	fixed _LightBalance;
	   	fixed4 _Color;
	   	fixed4 _Color1;
	    	
	  	struct Input
	  	{
		   	fixed2 uv_MainTex;
		   	fixed2 uv_Detail;
	  	};
   
	  	void surf (Input IN, inout SurfaceOutputDetail o) 
	  	{
	  		o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	  		fixed4 detail = tex2D (_Detail, IN.uv_Detail);
	     	o.Alpha = detail.a;
	     	o.Detail = detail.rgb;
	  	}
 	  	
 	  	fixed4 LightingCustom (SurfaceOutputDetail o, half3 lightDir, half atten)
		{
			return BalancedDetailLight(o, lightDir, atten, _LightBalance);
		}
 	  	
	  	void customColor (Input IN, SurfaceOutputDetail o, inout fixed4 color)
	  	{
	  		color = thresholdColorDetail(color, _Threshold, _Color, _Color1, fixed4(o.Detail.r, o.Detail.g, o.Detail.b, 1), o.Albedo);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}

