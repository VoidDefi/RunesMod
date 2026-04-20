Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state { Texture = <SpriteTexture>; };

Texture2D uNoise;
sampler2D uNoiseSampler = sampler_state { Texture = <uNoise>; };

float uNoiseSize;
float2 uNoiseMoveDirection;
float uNoiseFactor;

float3 uColor;
float3 uHotColor;
float uHotFactor;

float3 uCircleColor;
float uCircleFactor;

float2 SpherizeUV(float2 uv, float strength)
{
    float2 p = uv * 2. - 1.;
    float r = length(p);

    if (r > 1.) return uv;

    p /= (sqrt(1. - r * r) + strength);

    return p * 0.5 + 0.5;
}

float4 PixelMain(float2 texCoord : TEXCOORD0) : COLOR0
{
    //texCoord = float2(int2(texCoord * 32)) / 32;
    
    float2 spherePos = SpherizeUV(texCoord, 0.5f);
    
    float circleAlpha = 0;
    float noiseAlpha = 0;
    
    if (length(texCoord * 2. - 1.) <= 1)
    {
        circleAlpha = tex2D(SpriteTextureSampler, texCoord).r * uCircleFactor;
        noiseAlpha = tex2D(uNoiseSampler, spherePos + uNoiseMoveDirection).r * uNoiseFactor;
    }
    
    float4 color = (clamp(noiseAlpha + circleAlpha, 0., 1.)).xxxx;

    float brightness = color.r;
    brightness = pow(brightness, uHotFactor);

    return lerp(float4(uColor, 1.) * color, float4(uHotColor, 1.), brightness.xxxx) * brightness;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}