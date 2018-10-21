Shader "ToonStyles/Unlit-Background"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Background"}
		ZWrite off
		Pass
		{
			Lighting Off
        	SetTexture [_MainTex] {}
		}
	}
}

