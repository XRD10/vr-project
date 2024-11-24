Shader "Custom/TransparentQuadScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _GlowColor ("Glow Color", Color) = (0,1,0,0.5)
        _GlowIntensity ("Glow Intensity", Range(0,10)) = 4.0
        _FlickerSpeed ("Flicker Speed", Range(0,50)) = 10.0
        _FlickerIntensity ("Flicker Intensity", Range(0,1)) = 0.1
        _ScanlineSpeed ("Scanline Speed", Float) = 10.0
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.1
        _Transparency ("Transparency", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

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
            float4 _GlowColor;
            float _GlowIntensity;
            float _FlickerSpeed;
            float _FlickerIntensity;
            float _ScanlineSpeed;
            float _ScanlineIntensity;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float scanline = sin(i.uv.y * 100 + _Time.y * _ScanlineSpeed) * _ScanlineIntensity;
                float flicker = 1 - (_FlickerIntensity * random(float2(_Time.y * _FlickerSpeed, 0)));

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = _GlowColor.rgb * _GlowIntensity;
                col.rgb += float3(0, scanline, 0);
                col.rgb *= flicker;
                col.a = _Transparency;

                return col;
            }
            ENDCG
        }
    }
}