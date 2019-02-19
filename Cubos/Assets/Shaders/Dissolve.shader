Shader "Custom/Dissolve"
{
Properties {
      _MainTex ("Texture (RGB)", 2D) = "white" {}
      _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
      _DissolveAmount ("Dissolve Amount", Range(1, 50)) = 10
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off
      CGPROGRAM
      //if you're not planning on using shadows, remove "addshadow" for better performance
      #pragma surface surf Lambert addshadow
      struct Input {
          float2 uv_MainTex;
          float2 uv_SliceGuide;
          float _DissolveAmount;
      };
      sampler2D _MainTex;
      sampler2D _SliceGuide;
      float _DissolveAmount;

      void surf (Input IN, inout SurfaceOutput o) {
          clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - (sin(_Time / _DissolveAmount)));
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
}
