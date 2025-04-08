// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RealisticFlame"
{
    Properties
    {
        _FlameSpeed ("Flame Speed", Float) = 1.0
        _FlameDistort ("Flame Distortion", Float) = 0.15
        _FlameThickness ("Flame Thickness", Float) = 0.5
        _HorizontalWobble ("Horizontal Wobble", Float) = 0.08
        _VerticalWobble ("Vertical Wobble", Float) = 0.05
        _TipBlur ("Tip Blur", Float) = 0.3
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
            float _HorizontalWobble;
            float _VerticalWobble;
            float _TipBlur;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0; // UVの範囲を [-1,1] に正規化
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _FlameSpeed;

                // **横方向の揺らぎ**（炎が左右にフワフワする）
                float sideWobble = sin(time + i.uv.y * 8.0) * _HorizontalWobble;

                // **縦方向の揺らぎ**（炎全体が上下にフワフワする）
                float upDownWobble = sin(time * 1.2 + i.uv.x * 5.0) * _VerticalWobble;

                // **炎の形状を決定**
                float flameShape = abs(i.uv.x + sideWobble) + upDownWobble;

                // 炎の太さ
                float thickness = smoothstep(_FlameThickness, _FlameThickness + 0.2, 1.0 - flameShape);

                // **炎のグラデーション（白 → 黄 → オレンジ）**
                float gradient = smoothstep(0.0, 1.0, i.uv.y + 0.5);
                half4 flameColor = lerp(_Color1, _Color2, gradient);
                flameColor = lerp(flameColor, _Color3, gradient * 1.2);

                // **先端のボカし処理**
                float tipBlur = smoothstep(1.0 - _TipBlur, 1.0, i.uv.y + 0.5);
                thickness *= tipBlur;

                // **透明マスク**
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
