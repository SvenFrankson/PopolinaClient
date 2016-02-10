Shader "Popolina/DebugTerrain" {
	Properties {
		_Ramp ("Height Ramp (RGB)", 2D) = "gray" {} 
		_CliffColor ("Cliff Color", Color) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _Ramp;
		fixed4 _CliffColor;

		struct Input {
			float3 worldNormal;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = (0,0,0,0);
			half d = IN.worldPos.y / 205;
			c.rgb = tex2D (_Ramp, float2(d,d)).rgb;
			if (IN.worldNormal.y < .5) {
				c = _CliffColor;
			}
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}