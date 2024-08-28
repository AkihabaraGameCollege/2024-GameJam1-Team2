using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunGame
{
    // ゲームオーバーUIの進行制御を管理します。
    public class GameOverUI : MonoBehaviour
    {
        // Retry Button が押されたときに発生する UnityEvent
        public UnityEvent onRetryButtonClick;
        // Exit Button が押されたときに発生する UnityEvent
        public UnityEvent onExitButtonClick;

        // Retry Button を指定します。
        [SerializeField]
        private Button retryButton = null;
        // Exit Button を指定します。
        [SerializeField]
        private Button exitButton = null;
        // 楽曲再生用の AudioSource を指定します。
        [SerializeField]
        private AudioSource musicAudio = null;

        // コンポーネントを事前に参照しておく変数
        Animator animator;
        // AnimatorパラメーターID
        static readonly int showId = Animator.StringToHash("Show");
        static readonly int outroId = Animator.StringToHash("Outro");

        void Awake()
        {
            // コンポーネントを参照しておく
            animator = GetComponent<Animator>();
            // UnityEvent を追加
            retryButton.onClick.AddListener(() =>
            {
                onRetryButtonClick.Invoke();
                animator.SetTrigger(outroId);
            });
            exitButton.onClick.AddListener(() =>
            {
                onExitButtonClick.Invoke();
                animator.SetTrigger(outroId);
            });
        }

        // このUIを表示します。
        public void Show()
        {
            retryButton.Select();
            animator.SetTrigger(showId);
            musicAudio.Play();
        }
    }
}