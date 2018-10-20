Shader "ToonStyles/Levels B/Transparent"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Levels ("Levels", Range(1, 8)) = 3
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" } 
	    Blend SrcAlpha OneMinusSrcAlpha 
	    Cull Back
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "../toonstyles.cginc"
		#pragma surface surf Custom finalcolor:customColor approxview halfasview nodirlightmap
		
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

