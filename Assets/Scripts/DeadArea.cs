using UnityEngine;

namespace RunGame
{
    // マップからの落下を判定します。
    public class DeadArea : MonoBehaviour
    {
        // このオブジェクトのトリガーに他のオブジェクトが侵入した際に呼び出されます。
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // ゲームオーバー判定
            if (collision.CompareTag("Player"))
            {
                StageScene.Instance.GameOver();
            }
        }
    }
}