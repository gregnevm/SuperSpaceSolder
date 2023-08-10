using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class IsometricCharacterController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private JoystickInputHandler _joystick;
    private CharacterController _characterController;
    private Animator _animator;
    public MovementState State { get; private set; }
    private Vector2 _movementDirection = Vector2.zero;

    public enum MovementState
    {
        Idle, Move
    }    

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void StopMoving()
    {
        _characterController.SimpleMove(Vector3.zero);
    }

    private void Update()
    {
        AnimatorBoolController();
    }

    private void Move()
    {
        _movementDirection = _joystick.CurrentPosition;
        State = (_movementDirection.x == 0 && _movementDirection.y == 0) ? MovementState.Idle : MovementState.Move;

        Vector2 normalizedDirection = _movementDirection.normalized;
        Vector3 movement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * new Vector3(normalizedDirection.x, 0f, normalizedDirection.y);

        LookRotate(movement);
       
        _characterController.SimpleMove(movement * _movementSpeed);
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

    private void AnimatorBoolController()
    {
        switch (State)
        {
            case MovementState.Idle:
                _animator.SetBool("isIdle", true);
                _animator.SetBool("isMoving", false);
                break;
            case MovementState.Move:
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isMoving", true);
                break;
            default:
                break;
        }
        //TODO: use trigers for animations, not bools
    }
}
