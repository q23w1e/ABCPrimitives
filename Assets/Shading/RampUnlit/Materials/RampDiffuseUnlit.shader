Shader "Custom/RampDiffuseUnlit"
{
	Properties
	{
		_Offset ("Offset", Float) = 0
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
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float rampCoord : TEXCOORD1;
			};

			float4x4 _GradientRotation;
			float _Offset;
			sampler2D _RampTex;
			float4 _RampTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.rampCoord = mul(_GradientRotation, v.vertex) + _Offset;
				// o.rampCoord = mul(_GradientRotation, v.vertex + _Offset);

				// o.rampCoord = saturate(dot(N, L));
				// o.rampCoord = dot(N, L) * 0.5 + 0.5;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = tex2D(_RampTex, float2(i.rampCoord, 1) * _RampTex_ST);
				
				return color;
			}
			ENDCG
		}
	}
}
