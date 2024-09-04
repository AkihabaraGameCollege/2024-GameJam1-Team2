using UnityEngine;

namespace RunGame
{
    public class GameClearSceneManager : MonoBehaviour
    {
        void Start()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            // プレイヤーのアイテムカウントなどをリセットする処理
            PlayerPrefs.SetInt("CurrentItemCount", 0);
            // 必要に応じて他のリセット処理も追加
        }
    }
}
