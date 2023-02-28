using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 60;
    public InputActionProperty turnInput;
    public InputActionProperty moveInput;
    public Rigidbody rb;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;
    public Transform directionSource;
    private Vector2 inputMoveAxis;
    public LayerMask groundLayer;
    private float inputTurnAxis;
    private void Update()
    {
        inputMoveAxis = moveInput.action.ReadValue<Vector2>();
        inputTurnAxis = turnInput.action.ReadValue<Vector2>().x;
        
    }

    private void FixedUpdate()
    {
        
        Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePos = rb.position + direction * Time.fixedDeltaTime * moveSpeed;

        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MoveRotation(rb.rotation * q);
        Vector3 newPosition = q * (targetMovePos - turnSource.position) + turnSource.position;

        rb.MovePosition(newPosition);
    }

    //public bool isGrounded()
    //{
    //    Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
    //    float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

    //    bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    //    return hasHit;
    //}
}
