Shader "ToonStyles/Levels A/Diffuse"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Levels ("Levels", Range(2, 8)) = 3
	}

	SubShader
	{
		Tags {"RenderType"="Opaque" "Queue"="Geometry"}
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Custom approxview halfasview nodirlightmap
		
		sampler2D _MainTex;
		half _Levels;
		
		struct Input
	  	{
	    	fixed2 uv_MainTex;
	  	};
		
		void surf (Input IN, inout SurfaceOutput o) 
	  	{
	  		fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
	  	}
	  	
	  	fixed4 LightingCustom (SurfaceOutput o, half3 lightDir, half atten)
	  	{
	  		return LevelsLight(o, lightDir, atten, _Levels);
	  	}
		ENDCG
	}
    Fallback "Diffuse"
}

