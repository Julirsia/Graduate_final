Shader "ToonStyles/Levels B/Bumped"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Levels ("Levels", Range(1, 8)) = 3
	}

	SubShader
	{
		Tags {"Queue"="Geometry" "RenderType"="Opaque"}
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Lambert finalcolor:customColor approxview halfasview dualforward
		
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
