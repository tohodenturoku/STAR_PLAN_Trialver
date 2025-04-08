// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SingleFlame"
{
    Properties
    {
        _FlameSpeed ("Flame Speed", Float) = 1.0
        _FlameDistort ("Flame Distortion", Float) = 0.2
        _FlameThickness ("Flame Thickness", Float) = 0.5
        _Color1 ("Base Color (White)", Color) = (1, 1, 1, 1)
        _Color2 ("Middle Color (Yellow)", Color) = (1, 1, 0, 1)
        _Color3 ("Tip Color (Orange)", Color) = (1, 0.5, 0, 1)
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float _FlameSpeed;
            float _FlameDistort;
            float _FlameThickness;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // UV の中心を (0,0) にして扱う
                o.uv = v.uv * 2.0 - 1.0;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _FlameSpeed;

                // 中心を基準にした炎の形
                float distortion = sin(i.uv.y * 8.0 + time) * _FlameDistort;
                float flameShape = abs(i.uv.x) + distortion;

                // 炎の太さ
                float thickness = smoothstep(_FlameThickness, _FlameThickness + 0.2, 1.0 - flameShape);

                // 炎のグラデーション
                float gradient = smoothstep(0.0, 1.0, i.uv.y + 0.5);
                half4 flameColor = lerp(_Color1, _Color2, gradient);
                flameColor = lerp(flameColor, _Color3, gradient * 1.2);

                // 透明マスク処理
                half4 finalColor = half4(0, 0, 0, 0);
                if (thickness > 0.0)
                {
                    finalColor = flameColor;
                    finalColor.a = thickness;
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
