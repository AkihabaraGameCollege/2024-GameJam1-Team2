using UnityEngine;
using UnityEngine.UI;

namespace RunGame
{
    // アイテムの総獲得数を表示するUIを表します。
    public class TotalItemUI : MonoBehaviour
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
            var totalItemCount = PlayerPrefs.GetInt("TotalItemCountKey", 0);
            for (int index = 0; index < values.Length; index++)
            {
                values[index].sprite = numbers[totalItemCount % 10];
                totalItemCount /= 10;
            }
        }
    }
}