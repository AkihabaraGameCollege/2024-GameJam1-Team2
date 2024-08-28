using UnityEngine;
using UnityEngine.UI;

namespace RunGame
{
    // アイテム獲得数を表示するUIを表します。
    public class ItemUI : MonoBehaviour
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
            var itemCount = StageScene.Instance.ItemCount;
            for (int index = 0; index < values.Length; index++)
            {
                values[index].sprite = numbers[itemCount % 10];
                itemCount /= 10;
            }
        }
    }
}