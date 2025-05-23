using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : BaseController
{
    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public int MaxJump = 1;
    private int curJump = 1;
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
    private InputAction attackAction;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        inputActionMap = playerInput.actions.FindActionMap("Player");
        moveAction = inputActionMap.FindAction("Move");
        lookAction = inputActionMap.FindAction("Look");
        jumpAction = inputActionMap.FindAction("Jump");
        dashAction = inputActionMap.FindAction("Dash");
        attackAction = inputActionMap.FindAction("Attack");
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
        attackAction.started += OnClimb;
        attackAction.performed += OnClimb;
        attackAction.canceled += OnClimb;
        EventBus.Subscribe("OnSpeedUp", OnSpeedUp);
        EventBus.Subscribe("OnSpeedReset", OnResetSpeed);
        EventBus.Subscribe("OnDoubleJump", OnDoubleJump);
        EventBus.Subscribe("OnShoot", OnShoot);


    }
    public void OnShoot(object a)
    {
        Debug.Log("발생");
        _rigidbody.velocity = Vector3.zero; 
        _rigidbody.AddForce((Vector3)a, ForceMode.Impulse);
    }
    public void OnClimb(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Canceled)
        {
            _rigidbody.useGravity = true;
        }
        else 
        {
            Debug.Log("올라감");
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Wall");
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, mask))
            {
                _rigidbody.useGravity = false;
                _rigidbody.velocity = new Vector3(0, moveSpeed/2, 0);
            }
            else
                _rigidbody.useGravity = true;
        }
       
        
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
        if(MaxJump>curJump)
        {
            Jump(jumpPower);
            curJump++;
        }
        if (IsGrounded())
        {
            Debug.Log("점프");
            Jump(jumpPower);
            curJump++;
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
    public void OnDoubleJump(object a)
    {
        StartCoroutine(DJMode());
    }
    IEnumerator DJMode()
    {
        MaxJump = 2;
        yield return new WaitForSeconds(20);
        MaxJump = 1;
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
        if (IsGrounded())
        {
            curJump = 1;
        }
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
      
        return false;
    }
}
