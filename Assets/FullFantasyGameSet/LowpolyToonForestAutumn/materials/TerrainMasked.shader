Shader "Custom/Diffuse + Mask"
{
Properties {
    _Tex2 ("Base (RGB)", 2D) = "white" {}
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _AlphaMap ("Additional Alpha Map (Greyscale)", 2D) = "white" {}
}
 
SubShader {
Lighting On
    Tags { "RenderType" = "Opaque" }
	
CGPROGRAM
#pragma surface surf Lambert


sampler2D _MainTex;
sampler2D _AlphaMap;
sampler2D _Tex2;
 
struct Input {
    float2 uv_MainTex;
	float2 uv_Tex2;
	float2 uv_AlphaMap;
};
 
void surf (Input IN, inout SurfaceOutput o) {
    half4 c = tex2D(_MainTex, IN.uv_MainTex) * tex2D(_AlphaMap, IN.uv_AlphaMap);
	half4 d = tex2D(_Tex2, IN.uv_Tex2) * (1-tex2D(_AlphaMap, IN.uv_AlphaMap));
    o.Albedo = c.rgb + d.rgb;
    o.Alpha = 1;
}
ENDCG
}
 
Fallback "VertexLit"

}
