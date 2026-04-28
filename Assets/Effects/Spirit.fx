Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 PixelMain(float2 texCoord : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    return float4(0.2, 0.8, 1.0, 0.5);
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelMain();
    }
}