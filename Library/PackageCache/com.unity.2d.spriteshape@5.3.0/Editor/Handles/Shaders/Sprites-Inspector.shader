// Texture is forced to be in Gamma space regardless of the active ColorSpace.
Shader "Hidden/InternalSpritesInspector"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog{ Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            uniform bool _AdjustLinearForGamma;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord  : TEXCOORD0;
                float2 clipUV : TEXCOORD1;
            };

            fixed4 _Color;
            uniform float4x4 unity_GUIClipTextureMatrix;

            v2f vert(appdata_t IN)
            {
                float3 screenUV = UnityObjectToViewPos(IN.vertex);
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                OUT.clipUV = mul(unity_GUIClipTextureMatrix, float4(screenUV.xy, 0, 1.0));
                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _GUIClipTexture;

            fixed4 frag(v2f IN) : COLOR
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord);
                fixed alpha = col.a;
                if (_AdjustLinearForGamma)
                    col.rgb = LinearToGammaSpace(col.rgb);
                col.a = alpha * tex2D(_GUIClipTexture, IN.clipUV).a;
                return col * IN.color;
            }
            ENDCG
        }
    }
}
