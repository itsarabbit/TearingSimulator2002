uniform Texture2D noise;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = float4(1.0, 0.0, 1.0, 1.0);
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}