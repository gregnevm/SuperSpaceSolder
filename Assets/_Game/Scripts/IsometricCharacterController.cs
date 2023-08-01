using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class IsometricCharacterController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private CharacterController characterController;
    private Animator animator;
    public MovementState state = MovementState.Idle;
    private Vector2 movementDirection = Vector2.zero;

    public enum MovementState
    {
        Idle, Move
    }

    public float MovementSpeed { get { return movementSpeed; } }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void StopMoving()
    {
        characterController.SimpleMove(Vector3.zero);
    }

    private void Update()
    {
        AnimatorBoolController();
    }

    private void Move()
    {
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");
        state = (movementDirection.x == 0 && movementDirection.y == 0) ? MovementState.Idle : MovementState.Move;

        Vector2 normalizedDirection = movementDirection.normalized;
        Vector3 movement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * new Vector3(normalizedDirection.x, 0f, normalizedDirection.y);

        LookRotate(movement);

       
        characterController.SimpleMove(movement * movementSpeed);
    }

    private void LookRotate(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void UpdateAnimator(Vector2 movementDirection)
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
    }

    private void AnimatorBoolController()
    {
        switch (state)
        {
            case MovementState.Idle:
                animator.SetBool("isIdle", true);
                animator.SetBool("isMoving", false);
                break;
            case MovementState.Move:
                animator.SetBool("isIdle", false);
                animator.SetBool("isMoving", true);
                break;
            default:
                break;
        }
    }
}