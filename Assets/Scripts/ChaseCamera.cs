using UnityEngine;

namespace RunGame
{
    // 対象を追尾するカメラの機能を提供します。
    public class ChaseCamera : MonoBehaviour
    {
        // 追尾対象を指定します。
        [SerializeField]
        [Tooltip("追尾対象を指定します。")]
        private Transform target = null;
        // 追尾対象とのオフセット値を指定します。
        [SerializeField]
        [Tooltip("追尾対象とのオフセット値を指定します。")]
        Vector2 offset = new(4, 1.5f);

        // Start is called before the first frame update
        void Start()
        {
            // カメラの現在位置を更新
            var position = transform.position;
            position.x = target.position.x + offset.x;
            // y座標を固定したい場合は、以下をコメントアウト
            position.y = target.position.y + offset.y;
            transform.position = position;
        }

        // Update is called once per frame
        void Update()
        {
            // カメラの現在位置を更新
            var position = transform.position;
            position.x = target.position.x + offset.x;
            // y座標を固定したい場合は、以下をコメントアウト
            position.y = target.position.y + offset.y;
            transform.position = position;
        }
    }
}