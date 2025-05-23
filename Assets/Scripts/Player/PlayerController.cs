using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : BaseController
{
    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;

    private float camCurXRot;
    public float lookSensitivity;
    public Action inventory;
    private Vector2 mouseDelta;
    [HideInInspector]
    public bool canLook = true;
    public bool isRun = false;
    private PlayerInput playerInput;
    private InputActionMap inputActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction dashAction;
    private InputAction invenAction;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        inputActionMap = playerInput.actions.FindActionMap("Player");
        moveAction = inputActionMap.FindAction("Move");
        lookAction = inputActionMap.FindAction("Look");
        jumpAction = inputActionMap.FindAction("Jump");
        dashAction = inputActionMap.FindAction("Dash");
        invenAction = inputActionMap.FindAction("Inventory");
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        moveAction.started += OnMove;
        lookAction.started += OnLook;
        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;
        jumpAction.started += OnJump;
        dashAction.started += OnDash;
        dashAction.canceled += OnDash;
        invenAction.started += OnInventoryButton;
        EventBus.Subscribe("OnSpeedUp", OnSpeedUp);
        EventBus.Subscribe("OnSpeedReset", OnResetSpeed);

    }
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        inventory?.Invoke();
        ToggleCursor();
        
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {

        if (IsGrounded())
        {
            Debug.Log("Á¡ÇÁ");
            Jump(jumpPower);
            }
    }
    public void Jump(float power)
    {
        _rigidbody.AddForce(Vector2.up * power, ForceMode.Impulse);


    }
    private Coroutine _coroutine;
    public void RunEnd()
    {
        isRun=false;
        nowSpeed= moveSpeed;
    }
    public void OnDash(InputAction.CallbackContext ctx)
    {
     
        if (ctx.phase == InputActionPhase.Started)
        {
            isRun = true;
            nowSpeed = DashSpeed;
        }
        if (ctx.phase == InputActionPhase.Canceled)
        {
            isRun=false;
           
            nowSpeed=moveSpeed;
        }
            
    }
    public void OnSpeedUp(object a)
    {
        nowSpeed += (int)a;
    }
    public void OnResetSpeed(object a)
    {
        nowSpeed = moveSpeed;
    }
    public void OnLook(InputAction.CallbackContext ctx)
    {

        mouseDelta = ctx.ReadValue<Vector2>();

    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>());
        curMovementInput = ctx.ReadValue<Vector2>();
    }

    protected void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    protected void LateUpdate()
    {
      
        if (canLook)
        {
            CameraLook();
        }
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
             new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.3f, Color.red, 1f);
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        Debug.Log("¶¥ ¾Æ´Ô");
        return false;
    }
}
