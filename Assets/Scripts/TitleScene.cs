using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunGame
{
    // タイトル画面の進行を制御します。
    public class TitleScene : MonoBehaviour
    {
        // 次のシーンを読み込み可能な場合はtrue、それ以外はfalse
        bool isLoadable = false;

        // コンポーネントを事前に参照しておく変数
        Animator animator;
        // AnimatorパラメーターID
        static readonly int outroId = Animator.StringToHash("Outro");

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントを事前に取得
            animator = GetComponent<Animator>();

            // ステージ途中でゲームを終了させた場合に対応するため、
            // アイテムの総獲得数をクリアーする
            PlayerPrefs.DeleteKey("TotalItemCountKey");

            StartCoroutine(OnStart());
        }

        // ２秒待機後に次のシーンを読込み可能にします。
        IEnumerator OnStart()
        {
            yield return new WaitForSeconds(2);
            isLoadable = true;
        }

        // 決定ボタン操作の際に呼び出され、次のシーンを読み込みます。
        public void LoadNextScene()
        {
            if (isLoadable)
            {
                StartCoroutine(OnLoadNextScene());
            }
        }

        IEnumerator OnLoadNextScene()
        {
            // アウトロ アニメーションへ切り替える
            animator.SetTrigger(outroId);
            // アニメーションが終了するまで1秒待機
            yield return new WaitForSeconds(1);
            // 次のシーンを読み込む
            SceneManager.LoadScene("Stage 0");
        }

        // ゲームを終了します。
        public void ExitGame()
        {
            StartCoroutine(OnExitGame());
        }

        IEnumerator OnExitGame()
        {
            // アウトロ アニメーションへ切り替える
            animator.SetTrigger(outroId);
            // アニメーションが終了するまで1秒待機
            yield return new WaitForSeconds(1);
            // アプリケーションを終了する
            Application.Quit();
        }
    }
}