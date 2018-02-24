Shader "Custom/Danmaku_Standard_Shader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma instancing_options assumeuniformscaling
    #pragma instancing_options procedural:setup

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half4 _Color;
		half _Glossiness;
		half _Metallic;

    #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
      StructuredBuffer<float2> positionBuffer;
      StructuredBuffer<float> rotationBuffer;
      StructuredBuffer<float4> colorBuffer;
    #endif

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

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
      #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * colorBuffer[unity_InstanceID];
      #else
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
      #endif
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}