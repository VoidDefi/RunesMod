float3 uHighlightColor;
float3 uShadowColor;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

//HSV Converter by tayloia: https://www.shadertoy.com/view/4dKcWK
/*
const float Epsilon = 1e-10;

float3 HUEtoRGB(in float hue)
{
    float3 rgb = abs(hue * 6. - float3(3, 2, 4)) * float3(1, -1, -1) + float3(-1, 2, 2);
    return clamp(rgb, 0., 1.);
}

float3 RGBtoHCV(in float3 rgb)
{
    float4 p = (rgb.g < rgb.b) ? float4(rgb.bg, -1., 2. / 3.) : float4(rgb.gb, 0., -1. / 3.);
    float4 q = (rgb.r < p.x) ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);
    float c = q.x - min(q.w, q.y);
    float h = abs((q.w - q.y) / (6. * c + Epsilon) + q.z);
    return float3(h, c, q.x);
}

float3 HSVtoRGB(in float3 hsv)
{
    float3 rgb = HUEtoRGB(hsv.x);
    return ((rgb - 1.) * hsv.y + 1.) * hsv.z;
}

float3 RGBtoHSV(in float3 rgb)
{
    float3 hcv = RGBtoHCV(rgb);
    float s = hcv.y / (hcv.z + Epsilon);
    return float3(hcv.x, s, hcv.z);
}*/

float4 PixelMain(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(SpriteTextureSampler, texCoord);
    float brightness = saturate(texColor);

    float3 color = lerp(uShadowColor, uHighlightColor, brightness);
    
    return float4(color * texColor.a, texColor.a);
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}