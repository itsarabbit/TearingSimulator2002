sampler TextureSampler : register(s0);
extern float amplification;


float4 main(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float tex = tex2D(TextureSampler, texCoord);
	float dist = length(float2(texCoord.x - 0.5, texCoord.y - 0.5));
	dist *= 2.0;
	dist = 1.0 - dist;
	tex += 0.5;
	
	float check = dist * (amplification + 0.2) + tex * tex * tex * tex * tex * dist * amplification * amplification;
	
	if(check > 0.5)
	{
		color = float4(0.0, 0.0, 0.0, 1.0 * check / 1.01);	
	}
	else
	{
		color = float4(0.0, 0.0, 0.0, 0.0);
	}
    return color;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_5_0 main();
    }
}