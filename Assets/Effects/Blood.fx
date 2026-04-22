sampler shadowImage : register(s1);
sampler lightImage : register(s2);

float4 color;
float4 shadowColor;
float4 lightColor;
float time;
float2 size;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 PixelMain(float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 position = texCoord;
    position *= size;
    position /= min(size.x, size.y);
    
    float4 canvasTextureColor = tex2D(SpriteTextureSampler, texCoord);
    float4 shadowTextureColor = tex2D(shadowImage, position * 5.0f + float2(-time, sin(time)) / 4.0);
    float4 lightTextureColor = tex2D(lightImage, position * 10.0f + float2(sin(time * 2.0) / 2.0, time));

    float4 resultColor = lerp(color, shadowTextureColor * shadowColor, shadowTextureColor.r);
    resultColor = canvasTextureColor * lerp(resultColor, lightTextureColor * lightColor, lightTextureColor.r);

    if (resultColor.a > 0)
        resultColor.a = 1.0f;
    
    return resultColor;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}