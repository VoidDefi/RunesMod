float4 uColor;
float4 uHotColor;
float uHotFactor;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 PixelMain(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(SpriteTextureSampler, texCoord);
    float brightness = texColor.r;

    brightness = pow(brightness, uHotFactor);

    float4 color = lerp(uColor * texColor, uHotColor, brightness) * brightness;
    
    return color;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}