using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int lookSpeed = 50;
    public Transform playerCamera;

    public float runSpeedMultiplier = 1.5f; // 走行速度の倍率

    private float xRotation = 0f;
    private bool canMove = true; // 移動を制御するフラグ
    private bool canLook = true; // 視点移動を制御するフラグ

    private bool isCursorLocked = true; // カーソルがロックされているかどうかのフラグ

    // キー設定用の変数
    private KeyCode forwardKey;  // Wキー（前進）
    private KeyCode backwardKey; // Sキー（後退）
    private KeyCode leftKey;     // Aキー（左移動）
    private KeyCode rightKey;    // Dキー（右移動）
    private KeyCode runKey;      // 走るキー

    //　なめらかな移動のための変数
    private float currentMoveX = 0f;
    private float currentMoveZ = 0f;
    public float acceleration = 10f; // 加速度
    public float deceleration = 3f; // 減速度

    private PauseMenuController pauseMenuController;

    void Start()
    {
        // PlayerPrefsからLookSpeedの値を取得。存在しない場合はデフォルト値50を使用
        lookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);

        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        forwardKey = (KeyCode)PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        backwardKey = (KeyCode)PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        runKey = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);

        // シーン内の PauseMenuController を探して参照を設定
        pauseMenuController = FindObjectOfType<PauseMenuController>();

        if (pauseMenuController == null)
        {
            Debug.LogError("シーン内に PauseMenuController が見つかりません。");
        }

        // ゲーム開始時にカーソルをロック
        LockCursor();
    }

    void Update()
    {
        // 視点移動（canLookがtrueのときのみ有効）
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed * 0.04f;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * 0.04f;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // 移動（canMoveがtrueのときのみ有効）
        if (canMove)
        {
            // 目標の移動量をリセット
            float targetMoveX = 0f;
            float targetMoveZ = 0f;

            // 各キーに応じて目標移動量を設定
            if (Input.GetKey(forwardKey))
                targetMoveZ += 1f;
            if (Input.GetKey(backwardKey))
                targetMoveZ -= 1f;
            if (Input.GetKey(leftKey))
                targetMoveX -= 1f;
            if (Input.GetKey(rightKey))
                targetMoveX += 1f;

            // 入力の正規化
            Vector3 inputDirection = new Vector3(targetMoveX, 0, targetMoveZ).normalized;

            // 加速度と減速度を考慮して現在の移動量を更新
            if (inputDirection.magnitude > 0)
            {
                // 加速
                currentMoveX = Mathf.MoveTowards(currentMoveX, targetMoveX, acceleration * Time.deltaTime);
                currentMoveZ = Mathf.MoveTowards(currentMoveZ, targetMoveZ, acceleration * Time.deltaTime);
            }
            else
            {
                // 減速
                currentMoveX = Mathf.MoveTowards(currentMoveX, 0f, deceleration * Time.deltaTime);
                currentMoveZ = Mathf.MoveTowards(currentMoveZ, 0f, deceleration * Time.deltaTime);
            }

            // 移動ベクトルを計算
            Vector3 move = transform.right * currentMoveX + transform.forward * currentMoveZ;

            // Shiftキーを押している間だけ移動速度を増加
            float currentSpeed = moveSpeed;
            bool isRunning = false;

            if (Input.GetKey(runKey))
            {
                currentSpeed *= runSpeedMultiplier; // 走行速度
                isRunning = true;
            }

            // 実際の移動を適用
            transform.position += move * currentSpeed * Time.deltaTime;

            // 足音の再生
            if (move.magnitude > 0.5f && pauseMenuController != null && !pauseMenuController.isPaused)
            {
                if (isRunning)
                {
                    // 現在再生中か確認して、走行中の足音が再生されていなければ再生
                    if (SoundManager.instance.footstepsSource.clip != SoundManager.instance.runFootsteps || !SoundManager.instance.footstepsSource.isPlaying)
                    {
                        SoundManager.instance.PlayRunFootsteps(); // 走行時の足音再生
                    }
                }
                else
                {
                    // 現在再生中か確認して、歩行中の足音が再生されていなければ再生
                    if (SoundManager.instance.footstepsSource.clip != SoundManager.instance.walkFootsteps || !SoundManager.instance.footstepsSource.isPlaying)
                    {
                        SoundManager.instance.PlayWalkFootsteps(); // 歩行時の足音再生
                    }
                }
            }
            else
            {
                SoundManager.instance.StopFootsteps(); // 移動していない場合は足音をフェードアウト
            }
        }
    }

    // 移動を無効化するメソッド
    public void DisableMovement()
    {
        canMove = false;
        currentMoveX = 0f; // 現在の移動量をリセット
        currentMoveZ = 0f; // 現在の移動量をリセット
        SoundManager.instance.StopFootsteps(); // 足音を停止する
    }

    // 移動を有効化するメソッド
    public void EnableMovement()
    {
        canMove = true;
    }

    // 視点移動を無効化するメソッド
    public void DisableLook()
    {
        canLook = false;
    }

    // 視点移動を有効化するメソッド
    public void EnableLook()
    {
        canLook = true;
    }

    // カーソルをロックするメソッド
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
        //EnableLook(); // 視点操作を有効にする
    }

    // カーソルをアンロックするメソッド
    public void UnlockCursor()
    {
        //Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isCursorLocked = false;
        //DisableLook(); // 視点操作を無効にする
    }

    // カーソルのロック/アンロックを切り替えるメソッド
    public void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void LoadKeySettings()
    {
        // PlayerPrefsからLookSpeedの値を取得。存在しない場合はデフォルト値50を使用
        lookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);

        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        forwardKey = (KeyCode)PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        backwardKey = (KeyCode)PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        runKey = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
    }
}
