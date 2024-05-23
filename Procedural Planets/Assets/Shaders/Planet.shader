Shader "Custom/Planet" {
    Properties{
        _ElevationMinMax("Elevation Min Max", Vector) = (0, 0, 0, 0)
        [NoScaleOffset] _Texture("Planet Texture", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _ColorGenerator("Color Generator", Range(0,1)) = 0
    }
    SubShader{
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.5

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _Texture;
        float4 _Texture_TexelSize;

        float2 _ElevationMinMax;
        float _Smoothness;
        float _ColorGenerator;

        float4 Unity_InverseLerp_float(float a, float b, float value) {
            return saturate((value - a) / (b - a));
        }

        void surf(Input IN, inout SurfaceOutputStandard o) {
            float4 tex = tex2D(_Texture, IN.uv_MainTex);
            o.Albedo = tex.rgb;
            o.Metallic = 0;
            o.Smoothness = saturate(_Smoothness);

            float elevation = IN.worldPos.y;
            float elevationMin = _ElevationMinMax.x;
            float elevationMax = _ElevationMinMax.y;
            float elevationRange = elevationMax - elevationMin;
            float elevationAlpha = 0;

            if (elevationRange > 0) {
                elevationAlpha = Unity_InverseLerp_float(elevationMin, elevationMax, elevation);
            }

            // Apply color based on vertex height
            float heightPercent = (IN.worldPos.y - elevationMin) / (elevationMax - elevationMin);
            o.Albedo = lerp(tex.rgb, _ColorGenerator, heightPercent);

            o.Alpha = elevationAlpha;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
