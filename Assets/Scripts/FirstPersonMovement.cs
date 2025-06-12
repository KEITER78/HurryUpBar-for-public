using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int lookSpeed = 50;
    public Transform playerCamera;

    public float runSpeedMultiplier = 1.5f; // ���s���x�̔{��

    private float xRotation = 0f;
    private bool canMove = true; // �ړ��𐧌䂷��t���O
    private bool canLook = true; // ���_�ړ��𐧌䂷��t���O

    private bool isCursorLocked = true; // �J�[�\�������b�N����Ă��邩�ǂ����̃t���O

    // �L�[�ݒ�p�̕ϐ�
    private KeyCode forwardKey;  // W�L�[�i�O�i�j
    private KeyCode backwardKey; // S�L�[�i��ށj
    private KeyCode leftKey;     // A�L�[�i���ړ��j
    private KeyCode rightKey;    // D�L�[�i�E�ړ��j
    private KeyCode runKey;      // ����L�[

    //�@�Ȃ߂炩�Ȉړ��̂��߂̕ϐ�
    private float currentMoveX = 0f;
    private float currentMoveZ = 0f;
    public float acceleration = 10f; // �����x
    public float deceleration = 3f; // �����x

    private PauseMenuController pauseMenuController;

    void Start()
    {
        // PlayerPrefs����LookSpeed�̒l���擾�B���݂��Ȃ��ꍇ�̓f�t�H���g�l50���g�p
        lookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);

        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        forwardKey = (KeyCode)PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        backwardKey = (KeyCode)PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        runKey = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);

        // �V�[������ PauseMenuController ��T���ĎQ�Ƃ�ݒ�
        pauseMenuController = FindObjectOfType<PauseMenuController>();

        if (pauseMenuController == null)
        {
            Debug.LogError("�V�[������ PauseMenuController ��������܂���B");
        }

        // �Q�[���J�n���ɃJ�[�\�������b�N
        LockCursor();
    }

    void Update()
    {
        // ���_�ړ��icanLook��true�̂Ƃ��̂ݗL���j
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed * 0.04f;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * 0.04f;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // �ړ��icanMove��true�̂Ƃ��̂ݗL���j
        if (canMove)
        {
            // �ڕW�̈ړ��ʂ����Z�b�g
            float targetMoveX = 0f;
            float targetMoveZ = 0f;

            // �e�L�[�ɉ����ĖڕW�ړ��ʂ�ݒ�
            if (Input.GetKey(forwardKey))
                targetMoveZ += 1f;
            if (Input.GetKey(backwardKey))
                targetMoveZ -= 1f;
            if (Input.GetKey(leftKey))
                targetMoveX -= 1f;
            if (Input.GetKey(rightKey))
                targetMoveX += 1f;

            // ���͂̐��K��
            Vector3 inputDirection = new Vector3(targetMoveX, 0, targetMoveZ).normalized;

            // �����x�ƌ����x���l�����Č��݂̈ړ��ʂ��X�V
            if (inputDirection.magnitude > 0)
            {
                // ����
                currentMoveX = Mathf.MoveTowards(currentMoveX, targetMoveX, acceleration * Time.deltaTime);
                currentMoveZ = Mathf.MoveTowards(currentMoveZ, targetMoveZ, acceleration * Time.deltaTime);
            }
            else
            {
                // ����
                currentMoveX = Mathf.MoveTowards(currentMoveX, 0f, deceleration * Time.deltaTime);
                currentMoveZ = Mathf.MoveTowards(currentMoveZ, 0f, deceleration * Time.deltaTime);
            }

            // �ړ��x�N�g�����v�Z
            Vector3 move = transform.right * currentMoveX + transform.forward * currentMoveZ;

            // Shift�L�[�������Ă���Ԃ����ړ����x�𑝉�
            float currentSpeed = moveSpeed;
            bool isRunning = false;

            if (Input.GetKey(runKey))
            {
                currentSpeed *= runSpeedMultiplier; // ���s���x
                isRunning = true;
            }

            // ���ۂ̈ړ���K�p
            transform.position += move * currentSpeed * Time.deltaTime;

            // �����̍Đ�
            if (move.magnitude > 0.5f && pauseMenuController != null && !pauseMenuController.isPaused)
            {
                if (isRunning)
                {
                    // ���ݍĐ������m�F���āA���s���̑������Đ�����Ă��Ȃ���΍Đ�
                    if (SoundManager.instance.footstepsSource.clip != SoundManager.instance.runFootsteps || !SoundManager.instance.footstepsSource.isPlaying)
                    {
                        SoundManager.instance.PlayRunFootsteps(); // ���s���̑����Đ�
                    }
                }
                else
                {
                    // ���ݍĐ������m�F���āA���s���̑������Đ�����Ă��Ȃ���΍Đ�
                    if (SoundManager.instance.footstepsSource.clip != SoundManager.instance.walkFootsteps || !SoundManager.instance.footstepsSource.isPlaying)
                    {
                        SoundManager.instance.PlayWalkFootsteps(); // ���s���̑����Đ�
                    }
                }
            }
            else
            {
                SoundManager.instance.StopFootsteps(); // �ړ����Ă��Ȃ��ꍇ�͑������t�F�[�h�A�E�g
            }
        }
    }

    // �ړ��𖳌������郁�\�b�h
    public void DisableMovement()
    {
        canMove = false;
        currentMoveX = 0f; // ���݂̈ړ��ʂ����Z�b�g
        currentMoveZ = 0f; // ���݂̈ړ��ʂ����Z�b�g
        SoundManager.instance.StopFootsteps(); // �������~����
    }

    // �ړ���L�������郁�\�b�h
    public void EnableMovement()
    {
        canMove = true;
    }

    // ���_�ړ��𖳌������郁�\�b�h
    public void DisableLook()
    {
        canLook = false;
    }

    // ���_�ړ���L�������郁�\�b�h
    public void EnableLook()
    {
        canLook = true;
    }

    // �J�[�\�������b�N���郁�\�b�h
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
        //EnableLook(); // ���_�����L���ɂ���
    }

    // �J�[�\�����A�����b�N���郁�\�b�h
    public void UnlockCursor()
    {
        //Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isCursorLocked = false;
        //DisableLook(); // ���_����𖳌��ɂ���
    }

    // �J�[�\���̃��b�N/�A�����b�N��؂�ւ��郁�\�b�h
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
        // PlayerPrefs����LookSpeed�̒l���擾�B���݂��Ȃ��ꍇ�̓f�t�H���g�l50���g�p
        lookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);

        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        forwardKey = (KeyCode)PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        backwardKey = (KeyCode)PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        runKey = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
    }
}
