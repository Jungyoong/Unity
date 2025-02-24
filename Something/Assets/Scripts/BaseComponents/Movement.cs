using System;
using System.Data;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;


enum MovementState
{
    Walking, Running,
    Crouching, Laying,
}
public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider _collider;
    internal Vector3 moveDirection;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float acceleration;
    public float groundDrag;
    public float airMultiplayer;
    public float airDrag;
    public float jumpForce;
    public bool inCover;

    [Header("Ground Check")]
    public LayerMask Ground;
    public float distance;
    internal bool isUsing;

    public bool Grounded
    {
        get
        {
            var l = Physics.Raycast(new Vector3 (transform.position.x, transform.position.y, transform.position.z), Vector3.down, distance, Ground);
            return l;
        }
    }

    void FixedUpdate()
    {
        DragCalculation();
    }
    
    public void DragCalculation()
    {
        if (!inCover)
            MovementFunc();

        if (Grounded)
        {
            rb.linearDamping = groundDrag;
            airDrag = 0;
        }
        else
        {
            rb.linearDamping = 0;
            airDrag = 0.57f;
        }
    }
    
    public void MovementFunc()
    {
        Vector3 flatVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float ZforwardSpeedRatio = Mathf.Clamp01(flatVel.z > 0 ? Mathf.Abs(flatVel.z) / moveSpeed + airDrag : 0);
        float ZbackwardSpeedRatio = Mathf.Clamp01(flatVel.z < 0 ? Mathf.Abs(flatVel.z) / moveSpeed + airDrag : 0);
        float XrightSpeedRatio = Mathf.Clamp01(flatVel.x > 0 ? Mathf.Abs(flatVel.x) / moveSpeed + airDrag : 0);
        float XleftSpeedRatio = Mathf.Clamp01(flatVel.x < 0 ? Mathf.Abs(flatVel.x) / moveSpeed + airDrag : 0);

        float ZforwardScalingFactor = ZforwardSpeedRatio > 0.9f ? (1 - ZforwardSpeedRatio) / (1 - 0.9f) : 1;
        float ZbackwardScalingFactor = ZbackwardSpeedRatio > 0.9f ? (1 - ZbackwardSpeedRatio) / (1 - 0.9f) : 1;
        float XrightScalingFactor = XrightSpeedRatio > 0.9f ? (1 - XrightSpeedRatio) / (1 - 0.9f) : 1;
        float XleftScalingFactor = XleftSpeedRatio > 0.9f ? (1 - XleftSpeedRatio) / (1 - 0.9f) : 1;

        float scaledZForward = moveDirection.z > 0 ? moveDirection.z * (ZforwardSpeedRatio > 0.9f ? ZforwardScalingFactor : 1) : 0;
        float scaledZBackward = moveDirection.z < 0 ? moveDirection.z * (ZbackwardSpeedRatio > 0.9f ? ZbackwardScalingFactor : 1) : 0;
        float scaledXLeft = moveDirection.x > 0 ? moveDirection.x * (XrightSpeedRatio > 0.9f ? XrightScalingFactor : 1) : 0;
        float scaledXRight = moveDirection.x < 0 ? moveDirection.x * (XleftSpeedRatio > 0.9f ? XleftScalingFactor : 1) : 0;

        moveDirection = new Vector3(scaledXRight + scaledXLeft, moveDirection.y, scaledZForward + scaledZBackward);

        rb.AddForce(moveDirection * moveSpeed * acceleration * (Grounded ? 1 : airMultiplayer), ForceMode.Force);
    }

    public void Jump()
    {
        if (Grounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
