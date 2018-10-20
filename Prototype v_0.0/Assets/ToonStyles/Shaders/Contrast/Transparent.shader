Shader "ToonStyles/Contrast/Transparent"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
      	_Brightness ("Brightness", Float) = -0.75
		_Contrast ("Contrast", Float) = 4
	}
	SubShader
	{
	    Tags { "RenderType" = "Transparent" "Queue" = "Transparent" } 
	    Blend SrcAlpha OneMinusSrcAlpha 
	    Cull Back
	    
	  	CGPROGRAM
	  	#include "UnityCG.cginc"
	  	#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview
	
		sampler2D _MainTex;
	    half _Brightness;
		fixed _Contrast;
	    	
	  	struct Input
	  	{
	    	fixed2 uv_MainTex;
	  	};
	  	
	  	void surf (Input IN, inout SurfaceOutput o) 
	  	{
	  		half4 color = tex2D (_MainTex, IN.uv_MainTex);
	    	o.Albedo = color.rgb;
	   	  	o.Alpha = color.a;
	  	}
 	  	
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return SimpleLambertLight(o, lightDir, atten);
		}
		
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color.rgb = contrastcolor(color, _Brightness, _Contrast);
	  	}
		ENDCG
	} 
	FallBack "Diffuse"
}

