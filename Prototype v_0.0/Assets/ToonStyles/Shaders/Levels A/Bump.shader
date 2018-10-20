Shader "ToonStyles/Levels A/Bumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Levels ("Levels", Range(2, 8)) = 3
	}

	SubShader
	{
		Tags {"Queue"="Geometry" "RenderType"="Opaque"}
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Custom dualforward approxview halfasview 
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		half _Levels;
		
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
	  		return LevelsLight(o, lightDir, atten, _Levels);
	  	}
		ENDCG
	}
    Fallback "Diffuse"
}
