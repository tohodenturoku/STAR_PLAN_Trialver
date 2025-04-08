using UnityEngine;

public class SineMovement : MonoBehaviour
{
    public float waveSpeed = 1f; // 揺れの速さ
    public float waveStrength = 0.5f; // 揺れの強さ

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; // 初期位置を保存
    }

    void Update()
    {
        // Sin関数を使って、上下に揺れる
        float yOffset = Mathf.Sin(Time.time * waveSpeed) * waveStrength;
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}
