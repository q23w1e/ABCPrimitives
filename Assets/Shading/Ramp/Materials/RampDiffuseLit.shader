Shader "Custom/RampDiffuseLit"
{
	Properties
	{
		_EmitterDir ("Emitter Direction", Vector) = (0, 0, 1, 1)
		_RampTex ("Ramp Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv 	  : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float rampCoord : TEXCOORD1;
			};

			sampler2D _RampTex;
			float4 _RampTex_ST;
			float4 _EmitterDir;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _RampTex);
				
				float3 N = UnityObjectToWorldNormal(v.normal);
				float3 L = -_EmitterDir.xyz;
				L = normalize(L);

				o.rampCoord = saturate(dot(N, L));
				// o.rampCoord = dot(N, L) * 0.5 + 0.5;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = tex2D(_RampTex, float2(i.rampCoord, 1));
				
				return color;
			}
			ENDCG
		}
	}
}
