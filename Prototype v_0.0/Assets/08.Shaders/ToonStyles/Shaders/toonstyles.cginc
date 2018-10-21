// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

#ifndef TOONSTYLES_INCLUDED
#define TOONSTYLES_INCLUDED

#include "UnityCG.cginc"

// ---------------
// Structs
// ---------------
struct SurfaceOutputDetail
{
    half3 Albedo;
    half3 Normal;
    half3 Emission;
    half Specular;
    half Gloss;
    half Alpha;
    
    fixed3 Detail;
};

// ---------------
// Test functions
// ---------------
fixed3 red()
{
	return fixed3(1, 0, 0);
}

// ---------------
// Surface functions
// ---------------
void fluidSurf (inout SurfaceOutput o, sampler2D bump, sampler2D fresnelTex, fixed2 uv, half waveScale, half2 waveSpeed, half3 vDir, fixed4 horizon)
{
	half2 uv_b = uv / (1.0 * waveScale);
	half2 uv_b2 = uv_b;
	fixed xPos = _Time.x * waveSpeed.x;
	fixed yPos = _Time.x * waveSpeed.y;
	uv_b.x = uv_b.x + xPos;
	uv_b.y = uv_b.y + yPos;
	uv_b2.x = uv_b2.x - xPos;
	uv_b2.y = uv_b2.y - yPos;
	
	o.Normal = (UnpackNormal ((tex2D (bump, uv_b) + tex2D (bump, uv_b2)) * 0.5));

	half fresnel = dot(vDir, o.Normal * 0.5);
	fixed4 water = tex2D(fresnelTex, half2(fresnel, fresnel));
	o.Albedo = lerp( horizon.rgb, fixed3(clamp(water.r, 0, 1), water.g, water.b), fresnel );
	o.Alpha = water.a;
}

void fluidSurf (inout SurfaceOutput o, sampler2D bump, sampler2D fresnelTex, fixed2 uv, half waveScale, half2 waveSpeed, half3 vDir)
{
	half2 uv_b = uv / (1.0 * waveScale);
	half2 uv_b2 = uv_b;
	fixed xPos = _Time.x * waveSpeed.x;
	fixed yPos = _Time.x * waveSpeed.y;
	uv_b.x = uv_b.x + xPos;
	uv_b.y = uv_b.y + yPos;
	uv_b2.x = uv_b2.x - xPos;
	uv_b2.y = uv_b2.y - yPos;
	
	o.Normal = (UnpackNormal ((tex2D (bump, uv_b) + tex2D (bump, uv_b2)) * 0.5));

	half fresnel = dot(vDir, o.Normal * 0.5);
	fixed4 water = tex2D(fresnelTex, half2(fresnel, fresnel));
	o.Albedo = water.rgb;
	o.Alpha = water.a;
}

void fluidAngledSurf (inout SurfaceOutput o, sampler2D bump, fixed2 uv, half waveScale, half2 waveSpeed, half3 vDir)
{
	half2 uv_b = uv / (1.0 * waveScale);
	half2 uv_b2 = uv_b;
	fixed xPos = _Time.x * waveSpeed.x;
	fixed yPos = _Time.x * waveSpeed.y;
	uv_b.x = uv_b.x + xPos;
	uv_b.y = uv_b.y + yPos;
	uv_b2.x = uv_b2.x - xPos;
	uv_b2.y = uv_b2.y - yPos;
	
	o.Normal = UnpackNormal ((tex2D (bump, uv_b) + tex2D (bump, uv_b2)) * 0.5);
	
	o.Albedo = dot(vDir, o.Normal);
	o.Alpha = 1;
}
			

// ---------------
// Lighting functions
// ---------------
fixed4 SimpleLambertLight (SurfaceOutput o, half3 lightDir, half atten) 
{
	half NdotL = dot (o.Normal, lightDir);
	fixed4 c;
	c.rgb = o.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
	c.a = o.Alpha;
	return c;
}
	  
fixed4 BalancedLight (SurfaceOutput o, half3 lightDir, half atten, fixed balance)
{
	half NdotL = dot (o.Normal, lightDir);
	fixed4 c;
	c.rgb = balance * o.Albedo + (1 - balance) * _LightColor0.rgb * (NdotL * atten * 2);
	c.a = o.Alpha;
	return c;
}
	
fixed4 BalancedDetailLight (SurfaceOutputDetail o, half3 lightDir, half atten, fixed balance)
{
	half NdotL = dot (o.Normal, lightDir);
	fixed3 light = _LightColor0.rgb * (NdotL * atten * 2);
	fixed4 c;
	c.rgb = balance * o.Albedo + (1 - balance) * light;
	o.Detail = o.Detail * light;
	c.a = o.Alpha;
	return c;
}

fixed4 ColorRampLight (SurfaceOutput o, half3 lightDir, half3 viewDir, half atten, half highlight, sampler2D rampTex, half brightness)
{
	fixed NdotL = dot (o.Normal, lightDir);
	fixed diff = NdotL * 0.5 + 0.5;
	half spec = pow (max(0, NdotL), highlight);
	fixed4 ramp = tex2D (rampTex, fixed2(diff, diff));
	fixed4 c;
	c.rgb = (o.Albedo + spec * brightness) * (atten * 2) * ramp.rgb * _LightColor0.rgb;
	c.a = o.Alpha * ramp.a;
	return c;
}

fixed4 ColorRampLightNoSpec (SurfaceOutput o, half3 lightDir, half3 viewDir, half atten, sampler2D rampTex)
{
	fixed NdotL = dot (o.Normal, lightDir);
	fixed diff = NdotL * 0.5 + 0.5;
	fixed3 ramp = tex2D (rampTex, fixed2(diff, diff)).rgb;
	fixed4 c;
	c.rgb = o.Albedo * (atten * 2) * ramp * _LightColor0.rgb;
	c.a = o.Alpha;
	return c;
}

fixed4 LevelsLight (SurfaceOutput o, half3 lightDir, half atten, half levels)
{
	fixed4 c;
	half step = 1 / ceil(levels);
	fixed NdotL = dot(o.Normal, lightDir);
	half currentLevel = ceil(NdotL / step);
	c.rgb = o.Albedo * _LightColor0.rgb * atten * 2 * currentLevel * step;
	c.a = o.Alpha;
	return c;
}

fixed4 LookupDetailsLight (inout SurfaceOutputDetail o, half3 lightDir, half atten)
{
	half NdotL = dot (o.Normal, lightDir);
	fixed3 light = _LightColor0.rgb * (NdotL * atten * 2);
	half4 c;
	c.rgb = o.Albedo * light;
	o.Detail = o.Detail * light;
	c.a = o.Alpha;
	return c;
}
		
// ---------------
// Final color functions
// ---------------
fixed4 thresholdColor (fixed4 col, fixed threshold, fixed4 color1, fixed4 color2)
{
    if (Luminance(col.rgb) < threshold)
	{
		return color2;
    }
    else
    {
    	return color1;
    }
}

fixed4 thresholdColorDetail (fixed4 col, fixed threshold, fixed4 color1, fixed4 color2, fixed4 detail)
{
    if (Luminance(col.rgb) < threshold)
	{
		return (1 - col.a) * color2 + col.a * detail;
    }
    else
    {
    	return (1 - col.a) * color1 + col.a * detail;
    }
}

fixed4 thresholdColorDetail (fixed4 col, fixed threshold, fixed4 color1, fixed4 color2, fixed4 detail, fixed3 albedo)
{	
	fixed diff = (-albedo.r - albedo.g - albedo.b + col.r + col.g + col.b) * 0.5 + 0.5;
	
    if (Luminance(col.rgb) < threshold)
	{
		return (1 - col.a) * color2 + detail * col.a * diff;
    }
    else
    {
    	return (1 - col.a) * color1 + detail * col.a * diff;
    }
}

fixed3 lookupColor (fixed4 color, sampler2D lookup, fixed scale, fixed offsetVar)
{
	fixed lum = Luminance(color.rgb) * scale;
	return tex2D(lookup, fixed2(lum, lum) + offsetVar).rgb;
}

fixed4 lookupRGBAColor (fixed4 color, sampler2D lookup, fixed scale, fixed offsetVar)
{
	fixed lum = Luminance(color.rgb) * scale;
	fixed4 c = tex2D(lookup, fixed2(lum, lum) + offsetVar);
	c.a = c.a * color.a;
	return c;
}

fixed3 contrastcolor (fixed4 color, half brightness, half contrast)
{
	return color.rgb * (1 + contrast) + fixed3(brightness, brightness, brightness);
}

fixed3 lookupColorDetails (fixed4 color, sampler2D lookup, fixed scale, fixed offsetVar, SurfaceOutputDetail o)
{
	fixed lum = Luminance(color.rgb) * scale;
	return (1 - color.a) * tex2D (lookup, fixed2(lum, 0.5) + offsetVar).rgb + color.a * o.Detail;
}

fixed3 levelsColor(SurfaceOutput o, fixed4 color, half levels)
{
	half step = 1 / ceil(levels * 2);
	half currentLevel = 3 * step * floor(Luminance(color.rgb) / step);
	return color.rgb * currentLevel;
}

#endif
