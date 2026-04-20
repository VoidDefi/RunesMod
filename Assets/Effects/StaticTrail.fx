sampler uImage0 : register(s0);
float2 uImageSize0;
float2 uImageOffset0;

float2 uSize;
float uZoom;

float uTime;
matrix uWorldViewProjection;

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
    float2 ScreenPos : TEXCOORD1;
};

VertexOutput VertexMain(VertexInput input)
{
    VertexOutput output;
    
    float4 worldPos = mul(float4(input.Position.xy, 0, 1), uWorldViewProjection);
    
    output.Position = worldPos;
    output.Color = input.Color;
    output.TexCoord = input.TexCoord;

    output.ScreenPos = worldPos.xy * uSize;
    
    return output;
}

float4 PixelMain(VertexOutput input) : COLOR0
{
    float4 color = input.Color;
    color *= tex2D(uImage0, (input.ScreenPos.xy + (uImageOffset0 * uTime)));
    
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