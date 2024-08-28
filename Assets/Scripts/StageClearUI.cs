using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunGame
{
    // ステージクリアーUIの進行制御を管理します。
    public class StageClearUI : MonoBehaviour
    {
        // 「NEXT」ボタンが押されたときに発生する UnityEvent
        public UnityEvent onNextButtonClick;
        // 「NEXT」ボタンを指定します。
        [SerializeField]
        private Button nextButton = null;
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
            nextButton.onClick.AddListener(() =>
            {
                onNextButtonClick.Invoke();
                animator.SetTrigger(outroId);
            });
        }

        // このUIを表示します。
        public void Show()
        {
            nextButton.Select();
            animator.SetTrigger(showId);
            musicAudio.Play();
        }
    }
}