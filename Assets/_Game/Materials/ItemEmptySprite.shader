// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Empty Item"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_BackAlpha("Back Alpha", Range(0, 1)) = 0.5
		_BorderAlpha("Border Alpha", Range(0, 1)) = 1.0
		_BorderDistance("Border Distance", Range(0.001, 0.01)) = 0.005
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
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
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
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				float _BorderDistance;

				float _BackAlpha;
				float _BorderAlpha;

				fixed4 frag(v2f IN) : SV_Target
				{
					float a = SampleSpriteTexture(IN.texcoord).a;
					float b = SampleSpriteTexture(IN.texcoord + float2(0, _BorderDistance)).a;

					float border = saturate(a - b);
					float back = a - border;

					fixed4 c = IN.color;
					c.a *= a;
					c.a *= lerp(_BackAlpha, _BorderAlpha, border);

					c.rgb *= c.a;
					return c;
				}
			ENDCG
			}
		}
}