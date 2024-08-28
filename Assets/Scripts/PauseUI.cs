using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunGame
{
    // ポーズUIの進行制御を管理します。
    public class PauseUI : MonoBehaviour
    {
        // Resume Button が押されたときに発生する UnityEvent
        public UnityEvent onResumeButtonClick;
        // Restart Button が押されたときに発生する UnityEvent
        public UnityEvent onRetryButtonClick;
        // Exit Button が押されたときに発生する UnityEvent
        public UnityEvent onExitButtonClick;

        // Resume Button を指定します。
        [SerializeField]
        private Button resumeButton = null;
        // Retry Button を指定します。
        [SerializeField]
        private Button retryButton = null;
        // Exit Button を指定します。
        [SerializeField]
        private Button exitButton = null;

        void Awake()
        {
            // UnityEvent を追加
            resumeButton.onClick.AddListener(() => { onResumeButtonClick.Invoke(); });
            retryButton.onClick.AddListener(() => { onRetryButtonClick.Invoke(); });
            exitButton.onClick.AddListener(() => { onExitButtonClick.Invoke(); });

            Hide();
        }

        // このUIを表示します。
        public void Show()
        {
            // 子オブジェクトをすべてアクティブ化
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            resumeButton.Select();
        }

        // このUIを非表示に設定します。
        public void Hide()
        {
            // 子オブジェクトをすべて非アクティブ化
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}