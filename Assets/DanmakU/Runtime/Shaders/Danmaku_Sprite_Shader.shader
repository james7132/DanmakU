// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Shader "Sprites/Danmaku" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
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
        #pragma instancing_options procedural:setup

        #include "UnityCG.cginc"

        CBUFFER_START(UnityPerDrawSprite)
            float _EnableExternalAlpha;
        CBUFFER_END

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
          StructuredBuffer<float2> positionBuffer;
          StructuredBuffer<float> rotationBuffer;
          StructuredBuffer<float4> colorBuffer;
        #endif

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

        void setup() {
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
          float2 position = positionBuffer[unity_InstanceID];
          float rotation = rotationBuffer[unity_InstanceID];
          float cosR = cos(rotation);
          float sinR = sin(rotation);

          unity_ObjectToWorld = float4x4(
            cosR, -sinR, 0, position.x,
            sinR,  cosR, 0, position.y,
               0,     0, 1,          0,
               0,     0, 0,          1
          );

          unity_WorldToObject = unity_ObjectToWorld;
          unity_WorldToObject._14_24_34 *= -1;
          unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
        #endif
        }

        v2f SpriteVert(appdata_t IN)
        {
            v2f OUT;

            UNITY_SETUP_INSTANCE_ID (IN);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            OUT.color = IN.color * colorBuffer[unity_InstanceID];
        #else
            OUT.color = IN.color;
        #endif

            #ifdef PIXELSNAP_ON
            OUT.vertex = UnityPixelSnap (OUT.vertex);
            #endif

            return OUT;
        }

        sampler2D _MainTex;
        sampler2D _AlphaTex;

        fixed4 SampleSpriteTexture (float2 uv)
        {
            fixed4 color = tex2D (_MainTex, uv);

        #if ETC1_EXTERNAL_ALPHA
            fixed4 alpha = tex2D (_AlphaTex, uv);
            color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
        #endif

            return color;
        }

        fixed4 SpriteFrag(v2f IN) : SV_Target
        {
            fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
            c.rgb *= c.a;
            return c;
        }
    ENDCG
    }
  }
}