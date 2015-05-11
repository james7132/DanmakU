// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

Shader "Danmaku/Touhou Sprite - Two Color"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_UpperColor ("Upper Color", Color) = (1,1,1,1)
		_LowerColor ("Lower Color", Color) = (-1,-1,-1,1)
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			fixed4 _UpperColor;
			fixed4 _LowerColor;

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 targetColor = IN.color;
				fixed4 texColor = tex2D(_MainTex, IN.texcoord);
				fixed greyScale = (texColor.r + texColor.g + texColor.b) / 3;
				fixed4 a = (1, 1, 1, texColor.a * targetColor.a), b;
				if(greyScale >= 0.5)
				{
					b = lerp(targetColor, _UpperColor, (greyScale - 0.5) * 2);
				}
				else
				{
					b = lerp(_LowerColor, targetColor, greyScale * 2);
				}
				return a*b;
			}
		ENDCG
		}
	}
}
