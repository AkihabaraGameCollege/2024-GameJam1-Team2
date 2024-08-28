using UnityEngine;

namespace RunGame
{
    // 指定した判定対象のコライダーと交差しているかを判定する機能を提供します。
    public class LineCaster2D : MonoBehaviour
    {
        // ラインの開始点のオフセットを指定します。
        [SerializeField]
        [Tooltip("ラインの開始座標のオフセットを指定します。")]
        private Vector3 startOffset;
        // ラインの終了点のオフセットを指定します。
        [SerializeField]
        [Tooltip("ラインの終了点のオフセットを指定します。")]
        private Vector3 endOffset;
        // 判定対象のレイヤーを指定します。
        [SerializeField]
        [Tooltip("判定対象のレイヤーを指定します。")]
        private LayerMask targetLayers = default;

        // 判定対象と交差しているかどうかを取得します。
        public bool IsCasted { get => isCasted; private set => isCasted = value; }
        private bool isCasted = false;

        // 固定フレームレートで呼び出される更新処理です。
        void FixedUpdate()
        {
            // 交差判定用のポイントを設定
            var start = transform.TransformDirection(startOffset);
            start += transform.position;
            var end = transform.TransformDirection(endOffset);
            end += transform.position;
            // 交差を判定
            IsCasted = Physics2D.Linecast(start, end, targetLayers);
        }

        // Unityエディター上で常時描画するギズモを記述します。
        private void OnDrawGizmos()
        {
            // 交差判定用のポイントを設定
            var start = transform.TransformDirection(startOffset);
            start += transform.position;
            var end = transform.TransformDirection(endOffset);
            end += transform.position;
            // 判定ラインを描画
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, end);
        }
    }
}