using UnityEngine;
using UnityEngine.UI;

namespace RunGame
{
    // プレイ時間を表示するUIを表します。
    public class PlayTimeUI : MonoBehaviour
    {
        // 数値表示用のスプライト画像を指定します。
        [SerializeField]
        private Sprite[] numbers = new Sprite[10];
        // 値を表示するImageを指定します。
        [SerializeField]
        private Image[] values = null;

        // Start is called before the first frame update
        void Start()
        {
            UpdateValue();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateValue();
        }

        // UIを表示更新します。
        private void UpdateValue()
        {
            // 100分の1秒単位に変換
            var playTime = Mathf.FloorToInt(StageScene.Instance.PlayTime * 100);
            for (int index = 0; index < values.Length; index++)
            {
                values[index].sprite = numbers[playTime % 10];
                playTime /= 10;
            }
        }
    }
}