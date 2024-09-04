using UnityEngine;
using UnityEngine.UI;

namespace RunGame
{
    // ユーザー入力を受け取ってプレイヤーを操作します。
    public class Player : MonoBehaviour
    {
        // 通常時の移動速度を指定します。
        [SerializeField]
        [Tooltip("通常時の移動速度を指定します。")]
        private float normalSpeed = 4;
        // スプリント時の移動速度を指定します。
        [SerializeField]
        [Tooltip("スプリント時の移動速度を指定します。")]
        private float sprintSpeed = 8;

        // ジャンプ力を指定します。
        [SerializeField]
        [Tooltip("ジャンプ力を指定します。")]
        private Vector2 jumpForce = new(0, 40);
        // ジャンプに必要な速度を指定します。
        [SerializeField]
        [Tooltip("ジャンプに必要な速度を指定します。")]
        private float requiredJumpSpeed = 0.1f;
        // 側面衝突時のノックバック力を指定します。
        [SerializeField]
        [Tooltip("側面衝突時のノックバック力を指定します。")]
        private Vector2 knockBackForce = new(-16, 4);

        // 転倒していないと判定する最小の角度を指定します。
        [SerializeField]
        private float minStandAngle = -60;
        // 転倒していないと判定する最大の角度を指定します。
        [SerializeField]
        private float maxStandAngle = 60;
        // 転倒状態の場合は true、それ以外は false
        bool isTumbled = false;

        // 接地状態の場合は true、それ以外は false
        bool isGrounded = true;

        // 静止していないと判定する最小速度(2乗値)
        [SerializeField]
        private float minMoveVelocity = 0.001f;
        // 制御不能状態の継続でゲームオーバーと判定する時間(秒)
        [SerializeField]
        private float uncontrollableTimeout = 1;
        // 制御不能状態の累積時間(秒)
        float uncontrollableTime = 0;

        // 地面との交差判定用のチェッカーを指定します。
        [SerializeField]
        [Tooltip("地面との交差判定用のチェッカーを指定します。")]
        private BoxCaster2D groundChecker = null;
        // 壁との交差判定用のチェッカーを指定します。
        [SerializeField]
        [Tooltip("壁との交差判定用のチェッカーを指定します。")]
        private BoxCaster2D wallChecker = null;

        // エフェクト再生用の AudioSource を指定します。
        [SerializeField]
        private AudioSource effectAudio = null;
        // ジャンプ時のサウンドを指定します。
        [SerializeField]
        private AudioClip soundOnJump = null;

        // スプリントの最大時間
        [SerializeField]
        private float sprintDurationBase = 100f;
        // スプリントのクールダウン時間
        [SerializeField]
        private float sprintCooldown = 3f;
        // スプリントのタイマー
        [SerializeField]
        private float sprintTimer = 0f;
        // スプリントのクールダウンタイマー
        private float sprintCooldownTimer = 0f;
        // スプリント中かどうか
        private bool isSprinting = false;
        // スコアアイテムを集めた数
        [SerializeField]
        private int scoreItemsCollected = 0;
        // スプリントタイマー用スライダー
        [SerializeField]
        private Slider sprintSlider = null;

        // スーパージャンプの力を指定します。
        [SerializeField]
        [Tooltip("スーパージャンプの力を指定します。")]
        private Vector2 superJumpForce = new(0, 80); // スーパージャンプの力を設定します。

        // このキャラクターのスリープ状態を取得します。
        public bool IsSleeping { get; private set; } = false;

        // プレイヤーの状態を表します。
        enum PlayerState
        {
            // 通常の走行状態
            Walking,
            // スプリント状態
            Sprinting,
            // ジャンプの予備動作
            JumpStart,
            // 地面から足が離れて上昇中
            Jumping,
            // 走行状態からの落下
            Falling,
        }
        // 現在のプレイヤー状態
        PlayerState currentState = PlayerState.Walking;

        // コンポーネントを参照しておく変数
        new Rigidbody2D rigidbody;
        Animator animator;
        // AnimatorのパラメーターID
        static readonly int isSprintId = Animator.StringToHash("isSprint");
        static readonly int jumpId = Animator.StringToHash("Jump");
        static readonly int landId = Animator.StringToHash("Land");

        void Awake()
        {
            // コンポーネントを事前に参照
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            currentState = PlayerState.Walking;
            UpdatePhysicsParameters();

            if (sprintSlider != null)
            {
                sprintSlider.maxValue = sprintDurationBase;
                sprintSlider.value = sprintDurationBase;
            }
        }

        // 毎フレームに一度実行される更新処理です。
        void Update()
        {
            if (IsSleeping)
            {
                return;
            }

            // スプリントのクールダウンタイマー更新
            if (sprintCooldownTimer > 0)
            {
                sprintCooldownTimer -= Time.deltaTime;
                if (sprintCooldownTimer <= 0)
                {
                    sprintCooldownTimer = 0;
                }
            }

            // スプリントタイマーの値を常にスライダーに反映
            sprintSlider.value = sprintTimer;

            // プレイヤーの状態に応じた処理
            switch (currentState)
            {
                case PlayerState.Walking:
                case PlayerState.Sprinting:
                    // スーパージャンプ
                    if (Input.GetButton("Fire1") && Input.GetButtonDown("Jump"))
                    {
                        SuperJump();
                        return;
                    }
                    // ジャンプ
                    if (Input.GetButtonDown("Jump"))
                    {
                        Jump();
                        return;
                    }
                    // スプリント
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Sprint();
                        return;
                    }
                    Move();
                    break;
                case PlayerState.JumpStart:
                    // ジャンプの予備動作後、足が離れた場合
                    if (!isGrounded)
                    {
                        currentState = PlayerState.Jumping;
                        return;
                    }
                    // ジャンプ予備動作中に必要なジャンプ速度が得られなかった場合
                    else if (rigidbody.velocity.y < requiredJumpSpeed)
                    {
                        animator.SetTrigger(landId);
                        currentState = PlayerState.Walking;
                        return;
                    }
                    break;
                case PlayerState.Jumping:
                    // 着地判定
                    if (isGrounded)
                    {
                        animator.SetTrigger(landId);
                        currentState = PlayerState.Walking;
                        return;
                    }
                    break;
                case PlayerState.Falling:
                    // 着地判定
                    if (isGrounded)
                    {
                        animator.SetTrigger(landId);
                        currentState = PlayerState.Walking;
                        return;
                    }
                    break;
                default:
                    break;
            }
        }

        // 固定フレームレートで呼び出される更新処理です。
        void FixedUpdate()
        {
            if (IsSleeping)
            {
                return;
            }

            UpdatePhysicsParameters();
            // 制御不能によるゲームオーバー判定
            if (!isGrounded && rigidbody.velocity.sqrMagnitude < minMoveVelocity)
            {
                uncontrollableTime += Time.fixedDeltaTime;
                if (uncontrollableTime >= uncontrollableTimeout)
                {
                    StageScene.Instance.GameOver();
                }
            }
            else
            {
                uncontrollableTime = 0;
            }

            // 壁との衝突
            if (!isTumbled && wallChecker.IsCasted)
            {
                // ノックバック
                rigidbody.AddForce(transform.TransformDirection(knockBackForce), ForceMode2D.Force);
            }
        }

        // Physics に依存したパラメーターを更新します。
        void UpdatePhysicsParameters()
        {
            // 転倒判定
            // z軸角度を正規化[-180, 180]
            var normalizedAngle = Mathf.Repeat(rigidbody.rotation + 180, 360) - 180;
            isTumbled = normalizedAngle < minStandAngle || normalizedAngle > maxStandAngle;
            // 接地判定
            isGrounded = groundChecker.IsCasted && !isTumbled;
        }

        // プレイヤーをスリープ状態に設定します。
        public void Sleep()
        {
            IsSleeping = true;
            rigidbody.simulated = false;
        }

        // プレイヤーのスリープ状態を解除します。
        public void WakeUp()
        {
            IsSleeping = false;
            rigidbody.simulated = true;
        }

        // このキャラクターを移動させます。
        public void Move()
        {
            // 等速度運動
            var velocity = rigidbody.velocity;
            switch (currentState)
            {
                case PlayerState.Walking:
                    velocity.x = normalSpeed;
                    break;
                case PlayerState.Sprinting:
                    velocity.x = sprintSpeed;
                    break;
                case PlayerState.JumpStart:
                case PlayerState.Jumping:
                case PlayerState.Falling:
                default:
                    break;
            }
            rigidbody.velocity = velocity;
        }

        // このキャラクターのスプリント状態を解除します。
        public void Walk()
        {
            animator.SetBool(isSprintId, false);
            effectAudio.Stop();

            currentState = PlayerState.Walking;

            // スライダーのバリューをマックスバリューに戻す
            if (sprintSlider != null)
            {
                sprintTimer = sprintSlider.maxValue;
                sprintSlider.value = sprintTimer;
            }
        }

        // このキャラクターをスプリント状態に設定します。
        public void Sprint()
        {
            isSprinting = true;
            sprintTimer = sprintDurationBase + (scoreItemsCollected * 0.5f); // スコアアイテム1つにつき0.5秒追加
            sprintCooldownTimer = sprintCooldown; // クールダウンタイマーをリセット
            currentState = PlayerState.Sprinting;
            animator.SetBool(isSprintId, true);
            effectAudio.Play();
        }

        // このキャラクターをジャンプさせます。
        public void Jump()
        {
            currentState = PlayerState.JumpStart;
            animator.SetTrigger(jumpId);
            effectAudio.PlayOneShot(soundOnJump);
            rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
        }

        // スーパージャンプを実行するメソッド
        private void SuperJump()
        {
            // スーパージャンプの力を加える
            rigidbody.AddForce(superJumpForce, ForceMode2D.Impulse);
            // スーパージャンプ後はジャンプ状態に設定
            currentState = PlayerState.Jumping;
            // ジャンプのアニメーションを再生
            animator.SetTrigger(jumpId);
            // スーパージャンプのサウンドを再生
            effectAudio.PlayOneShot(soundOnJump);
        }

        // スコアアイテムを収集したときの処理
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("ScoreItem"))
            {
                // スコアアイテムを集める
                scoreItemsCollected++;
                Destroy(other.gameObject);

                // スプリントの効果を更新
                UpdateSprintEffects();
            }
        }

        // スプリントの効果を更新するメソッド
        private void UpdateSprintEffects()
        {
            if (sprintSlider != null)
            {
                // アイテムによるスプリントの最大時間を計算
                float newMaxSprintDuration = sprintDurationBase + (scoreItemsCollected * 0.5f);

                // スライダーの最大値を更新
                sprintSlider.maxValue = newMaxSprintDuration;

                // スプリントタイマーを最大値に設定
                sprintTimer = newMaxSprintDuration;

                // スライダーの現在値も最大値に設定
                sprintSlider.value = sprintTimer;
            }
        }
    }
}
