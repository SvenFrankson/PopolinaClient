Shader "Popolina/DebugTerrain" {
	Properties {
		_LineColor ("Line Color", Color) = (0,0,0,0)
		_LineSize ("Line Size", Float) = 1
		_GroundSize ("Ground Size", Int) = 10
		_Ramp ("Height Ramp (RGB)", 2D) = "gray" {} 
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		fixed4 _LineColor;
		float _LineSize;
		int _GroundSize;
		sampler2D _Ramp;

		struct Input {
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = (0,0,0,0);
			half d = IN.worldPos.y / 205;
			c.rgb = tex2D (_Ramp, float2(d,d)).rgb;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}