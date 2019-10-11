Shader "Sprites/Custom/Unlit-Shadow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest"
            "IgnoreProjector"="True"
            "RenderType"="TransparentCutout"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        LOD 200
        Cull Off
        Lighting On
        ZWrite Off
        CGPROGRAM
        #pragma surface surf NoLighting addshadow alphatest:_Cutoff
        sampler2D _MainTex;
        fixed4 _Color;
        struct Input
        {
            float2 uv_MainTex;
        };
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
             fixed4 c;
             c.rgb = s.Albedo;
             c.a = s.Alpha;
             return c;
        }
       
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    Fallback "Legacy Shaders/Transparent/Cutout/VertexUnlit"
}