#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ToonStyles/Outline B"
{
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Float) = .1
	}
	SubShader
	{
		// The outline pass that is used also by other shaders.
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			float _Outline;
			float4 _OutlineColor;
			
			struct appdata 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f 
			{
				float4 pos : POSITION;
				float4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				norm.x *= UNITY_MATRIX_P[0][0];
				norm.y *= UNITY_MATRIX_P[1][1];
				o.pos.xy += norm.xy * o.pos.z * _Outline / 1.0;
				o.color = _OutlineColor;
				return o;
			}
			
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
