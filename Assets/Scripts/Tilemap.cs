using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTransparencyChanger : MonoBehaviour
{
    public Tilemap tilemap; // タイルマップ
    public float minAlpha = 0.1f; // 最小透明度
    public float maxAlpha = 1.0f; // 最大透明度
    public float changeInterval = 1.0f; // 変更間隔（秒）

    private Material material;
    private float timeElapsed = 0f;

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        if (tilemap != null)
        {
            material = tilemap.GetComponent<TilemapRenderer>().material;
        }
        else
        {
            Debug.LogError("Tilemapが見つかりません。");
        }
    }

    void Update()
    {
        if (material == null)
            return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= changeInterval)
        {
            // ランダムな透明度を生成
            float randomAlpha = Random.Range(minAlpha, maxAlpha);
            Color color = material.color;
            color.a = randomAlpha;
            material.color = color;

            // タイマーをリセット
            timeElapsed = 0f;
        }
    }
}
