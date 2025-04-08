Shader "Custom/CircleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // 円の色
        _Radius ("Radius", Float) = 0.5     // 円の半径
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
        float random (fixed2 p) { 
            return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
        }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV空間の中央を計算
                float2 center = float2(0.5, 0.5);
                
                // 中央からの距離を計算
                float dist = distance(i.uv, center);

                // 距離が半径内なら色を描画、外なら透明
                if (dist > _Radius)
                {
                    discard; // 半径を超える部分を透明にする
                }

                //距離が半径の85%より遠ければ、見た目を変える
                if (dist > 0.85 * _Radius)
                {
                    return 0.85 / distance(_Radius * 0.85, dist) * //距離と半径の85%が近いほど大きい値をかける
                        clamp(tan((i.uv.x + i.uv.y) / 0.02), 0.0, 8.0) * //UVのxとyを足して斜め向きに模様が描かれるようにする
                         _Color;
                }
                return _Color;

            }
            ENDCG
        }
    }
}
