using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("����ݒ�")]
    public Transform opponent;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Input System
    private PlayerControls controls;
    private float moveAxis;    // 1D Axis �̒l
    private bool jumpInput;
    public bool canMove = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // ���ړ� (float)
        controls.Player.Move.performed += ctx => moveAxis = ctx.ReadValue<float>();
        controls.Player.Move.canceled += ctx => moveAxis = 0f;

        // �W�����v
        controls.Player.Jump.performed += ctx => jumpInput = true;
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Update()
    {
        if (!canMove) return;

        // �n�ʔ���
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = 0;

        // ���ړ�
        Vector3 move = new Vector3(moveAxis * moveSpeed, 0, 0);
        controller.Move(move * Time.deltaTime);

        // �W�����v
        if (jumpInput && isGrounded)
        {
            velocity.y = jumpForce;
        }
        jumpInput = false;

        // �d��
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // �������������
        if (opponent != null)
        {
            if (opponent.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
