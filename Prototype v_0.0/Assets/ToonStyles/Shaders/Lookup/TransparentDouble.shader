Shader "ToonStyles/Lookup/TransparentDoubleSided"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Offset ("Lookup Offset", Float) = 0
	  	_Scale ("Lookup Scale", Float) = 1
      	_Lookup ("Lookup Texture", 2D) = "white" {}
	}
	SubShader
	{
	    Tags { "RenderType" = "Transparent" "Queue" = "AlphaTest" } 
	    Blend SrcAlpha OneMinusSrcAlpha 
	    Cull off
	    AlphaTest Greater 0.1
	    
	  	CGPROGRAM
	  	#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap 
		
	    sampler2D _MainTex;
	    sampler2D _Lookup;
	    fixed _Offset;
	    fixed _Scale;
	    	
	  	struct Input
	  	{
	    	fixed2 uv_MainTex;
	  	};
   
	  	void surf (Input IN, inout SurfaceOutput o) 
	  	{
	     	fixed4 color = tex2D (_MainTex, IN.uv_MainTex);
	     	o.Albedo = color.rgb;
	     	o.Alpha = color.a;
	  	}
 	  	
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return SimpleLambertLight(o, lightDir, atten);
		}
		
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color.rgb = lookupRGBAColor(color, _Lookup, _Scale, _Offset);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}

