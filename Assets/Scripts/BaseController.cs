using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [Header("Movement")]
    protected float nowSpeed;
    public float moveSpeed;
    public float DashSpeed;
    protected Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;
    protected Rigidbody _rigidbody;
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        nowSpeed=moveSpeed;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= nowSpeed;
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }
}
