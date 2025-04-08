using UnityEngine;

public class FireShapeCutter : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // スプライトを適用するオブジェクト
    public float jaggedness = 0.2f; // 炎のギザギザ度（0.0 〜 1.0）
    public int seed = 42; // 乱数のシード（変えると炎の形が変わる）

    void Start()
    {
        // 元のスプライトの Texture を取得
        Texture2D originalTexture = spriteRenderer.sprite.texture;
        Texture2D newTexture = CutToFireShape(originalTexture, jaggedness, seed);

        // 新しいスプライトを作成し、スプライトレンダラーに適用
        spriteRenderer.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// スプライトを炎の形にカットする
    /// </summary>
    /// <param name="original">元のTexture</param>
    /// <param name="jaggedness">ギザギザ度（0〜1）</param>
    /// <param name="seed">ランダムシード</param>
    /// <returns>炎の形にカットされたTexture2D</returns>
    Texture2D CutToFireShape(Texture2D original, float jaggedness, int seed)
    {
        int width = original.width;
        int height = original.height;
        Texture2D newTexture = new Texture2D(width, height);
        Color[] pixels = original.GetPixels();

        System.Random random = new System.Random(seed);

        // 炎の形状を作る
        for (int x = 0; x < width; x++)
        {
            // 炎の上端の高さをランダムに設定
            float noise = Mathf.PerlinNoise(x * 0.1f, seed * 0.1f) * jaggedness;
            int fireHeight = Mathf.FloorToInt(height * (0.5f + noise)); // 50% 〜 (50% + ギザギザ) の範囲

            for (int y = 0; y < height; y++)
            {
                if (y > fireHeight)
                {
                    // 炎の外側部分を透明にする
                    pixels[y * width + x] = new Color(0, 0, 0, 0);
                }
                else
                {
                    // ランダムにギザギザを作る
                    if (random.NextDouble() < jaggedness * 0.5f)
                    {
                        pixels[y * width + x] = new Color(0, 0, 0, 0);
                    }
                }
            }
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
    }
}
