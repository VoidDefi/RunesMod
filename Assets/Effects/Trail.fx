sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
matrix uWorldViewProjection;
float4 uShaderSpecificData;

bool flipped;

struct VertexInput
{
    float2 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

VertexOutput VertexMain(VertexInput input)
{
    VertexOutput output;
    
    output.Position = mul(float4(input.Position.xy, 0, 1), uWorldViewProjection);
    output.Color = input.Color;
    output.TexCoord = input.TexCoord;

    return output;
}

float4 PixelMain(VertexOutput input) : COLOR0
{
    float4 color = input.Color;
    color *= tex2D(uImage0, input.TexCoord.xy).r * 2;
    color *= tex2D(uImage1, float2(input.TexCoord.x - uTime, input.TexCoord.y)).r;
    
    return color;
}

technique Technique1
{
    pass ShaderPass
    {
        VertexShader = compile vs_2_0 VertexMain();
        PixelShader = compile ps_2_0 PixelMain();
    }
}