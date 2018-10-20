Shader "ToonStyles/Levels A/TransparentDoubleSided"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Levels ("Levels", Range(2, 8)) = 3
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
		half _Levels;
		
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
	  		return LevelsLight(o, lightDir, atten, _Levels);
	  	}
		ENDCG
	}
    Fallback "Diffuse"
}

