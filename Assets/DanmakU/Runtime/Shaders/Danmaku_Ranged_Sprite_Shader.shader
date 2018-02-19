// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Shader "Sprites/Danmaku Ranged" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _ColorDark ("Tint Dark", Color) = (0,0,0,0)
    _ColorBright ("Tint Bright", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
    [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
  }

  SubShader {
    Tags {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Blend One OneMinusSrcAlpha

    Pass {
    CGPROGRAM
        #pragma vertex SpriteVert
        #pragma fragment SpriteFrag
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

        #include "UnityCG.cginc"

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(fixed2, _Flip)
        UNITY_INSTANCING_BUFFER_END(Props)

        CBUFFER_START(UnityPerDrawSprite)
            float _EnableExternalAlpha;
        CBUFFER_END

        float4 _ColorDark;
        float4 _ColorBright;

        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f SpriteVert(appdata_t IN)
        {
            v2f OUT;

            UNITY_SETUP_INSTANCE_ID (IN);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

        #ifdef UNITY_INSTANCING_ENABLED
            IN.vertex.xy *= UNITY_ACCESS_INSTANCED_PROP(Props, _Flip);
        #endif

            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;
            OUT.color = IN.color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

            #ifdef PIXELSNAP_ON
            OUT.vertex = UnityPixelSnap (OUT.vertex);
            #endif

            return OUT;
        }

        sampler2D _MainTex;
        sampler2D _AlphaTex;

        fixed4 SampleSpriteTexture (float2 uv, float4 medColor)
        {
            fixed4 color = tex2D (_MainTex, uv);
            half greyscale = (color.a + color.b + color.g) / 3;
            if (greyscale < 0.5) {
              color = color * lerp(_ColorDark, medColor, greyscale * 2);
            } else {
              color = color * lerp(medColor, _ColorBright, (greyscale - 0.5) * 2);
            }

        #if ETC1_EXTERNAL_ALPHA
            fixed4 alpha = tex2D (_AlphaTex, uv);
            color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
        #endif

            return color;
        }

        fixed4 SpriteFrag(v2f IN) : SV_Target
        {
            fixed4 c = SampleSpriteTexture (IN.texcoord, IN.color);
            c.rgb *= c.a;
            return c;
        }
    ENDCG
    }
  }
}