// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_CutTex ("Cut Texture", 2D) = "white" {}
		_Cutoff ("Cutoff", Range(0, 1)) = 0

	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CutTex;
			fixed _Cutoff;
			
			v2f vert (appdata a)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(a.vertex);
				o.uv = a.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 ccol = tex2D(_CutTex, i.uv);

				if (ccol.a > _Cutoff)
					return col;
				return fixed4(0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
