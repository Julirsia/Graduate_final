Shader "ToonStyles/ColorRamp/TransparentDoubleSided"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Coloring Ramp", 2D) = "white" {}
      	_Highlight ("Shine", Range(0, 64)) = 40
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
	  	#pragma surface surf Custom approxview halfasview nodirlightmap
		sampler2D _MainTex;
		sampler2D _Ramp;
		half _Highlight;
		
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
	  	
	  	fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half3 viewDir, half atten)
		{
			return ColorRampLight(o, lightDir, viewDir, atten, _Highlight, _Ramp, 0.25 * (64 - _Highlight));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

