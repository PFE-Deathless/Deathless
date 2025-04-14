Shader "Custom/AlwaysOnTopSprite"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue" = "Overlay" }  // Renders after everything
		Pass
		{
			ZTest Always     // Ignores depth testing, always in front
			ZWrite Off       // Does not write to depth buffer
			Cull Off         // Double-sided rendering

			Blend SrcAlpha OneMinusSrcAlpha  // Standard sprite transparency

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			fixed4 _Color;

			v2f vert (appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.color = IN.color;
				return OUT;
			}

			fixed4 frag (v2f IN) : SV_Target
			{
				return tex2D(_MainTex, IN.uv) * IN.color * _Color;
			}
			ENDCG
		}
	}
}
