Shader "ToonStyles/Levels B/Diffuse"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Levels ("Levels", Range(1, 8)) = 3
	}

	SubShader
	{
		Tags {"RenderType"="Opaque" "Queue"="Geometry"}
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview 
		
		sampler2D _MainTex;
		half _Levels;
		
		struct Input
	  	{
	    	fixed2 uv_MainTex;
	  	};
		
		void surf (Input IN, inout SurfaceOutput o) 
	  	{
	     	o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	  	}
	  	
		fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
		{
			return SimpleLambertLight(o, lightDir, atten);
		}
		
	  	void customColor (Input IN, SurfaceOutput o, inout fixed4 color)
	  	{
	  		color.rgb = levelsColor(o, color, _Levels);
	  	}
		ENDCG
	}
    Fallback "Diffuse"
}
