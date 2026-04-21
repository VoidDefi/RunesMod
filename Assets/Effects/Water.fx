sampler shadowImage : register(s1);
sampler foamImage : register(s2);

float4 color;
float4 shadowColor;
float4 foamColor;
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
    float4 shadowTextureColor = tex2D(shadowImage, position * 4.0f + float2(-time, time));
    float4 foamTextureColor = tex2D(foamImage, position * 2.0f + float2(time, -time));

    float4 resultColor = lerp(color, shadowTextureColor * shadowColor, shadowTextureColor.r);
    resultColor = canvasTextureColor * lerp(resultColor, foamTextureColor * foamColor, foamTextureColor.r);

    return resultColor;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}