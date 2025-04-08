Shader "Custom/SmoothGradientBeamShader"
{
    Properties
    {
        _WhiteColor ("White Color", Color) = (1, 1, 1, 1) // 白色
        _YellowColor ("Yellow Color", Color) = (1, 1, 0, 1) // 黄色
        _OrangeColor ("Orange Color", Color) = (1, 0.5, 0, 1) // 橙色
        _WhiteThreshold ("White Threshold", Float) = 0.3 // 白色の範囲
        _YellowThreshold ("Yellow Threshold", Float) = 0.5 // 黄色の範囲
        _OrangeThreshold ("Orange Threshold", Float) = 1.0 // 橙色の範囲
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

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            fixed4 _WhiteColor;
            fixed4 _YellowColor;
            fixed4 _OrangeColor;
            float _WhiteThreshold;
            float _YellowThreshold;
            float _OrangeThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = (v.vertex.xy + float2(1.0, 1.0)) * 0.5; // UV座標を[0,1]に正規化
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UVのY座標を中心に白を配置
                float distance = abs(i.uv.x - 0.5) * 2.0; // 中心からの距離を計算（0.5を中心とする）

                // 白色から黄色へのグラデーション
                float whiteToYellow = smoothstep(_WhiteThreshold, _YellowThreshold, distance);

                // 黄色から橙色へのグラデーション
                float yellowToOrange = smoothstep(_YellowThreshold, _OrangeThreshold, distance);

                // 色の合成
                fixed4 color = lerp(_WhiteColor, _YellowColor, whiteToYellow);
                color = lerp(color, _OrangeColor, yellowToOrange);

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
