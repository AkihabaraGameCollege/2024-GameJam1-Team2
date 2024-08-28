using UnityEngine;

namespace RunGame
{
    // 背景の連続スクロールを制御します。
    [ExecuteInEditMode]
    public class Backgrounds : MonoBehaviour
    {
        // スプライト画像を配列で指定します。
        [SerializeField]
        private Transform[] sprites = null;

        // 背景スプライト1枚(1グリッド)当たりのunitサイズ
        Vector3 unitsPerGrid;

        // Start is called before the first frame update
        void Start()
        {
            // 0番目の画像から表示サイズ（Unit単位）を取得
            unitsPerGrid = sprites[0].GetComponent<SpriteRenderer>().bounds.size;

            UpdateSprites();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateSprites();
        }

        // すべてのパネルの位置を更新します。
        private void UpdateSprites()
        {
            // カメラの位置
            var cameraGridX = Mathf.FloorToInt(Camera.main.transform.position.x / unitsPerGrid.x);

            // 配列の画像を並べる
            for (int index = 0; index < sprites.Length; index++)
            {
                var position = sprites[index].position;
                position.x = (index - 1 + cameraGridX) * unitsPerGrid.x;
                sprites[index].position = position;
            }
        }
    }
}