using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField]
    private float movementSpeed = 3.0f;

    private Transform rotator;

    [SerializeField] 
    private float smoothing = 0.1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        InputSystem.EnableDevice(Accelerometer.current);
        InputSystem.EnableDevice(AttitudeSensor.current);

        rotator = new GameObject("Rotator").transform;
        rotator.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void Update()
    {
        Move();
        LookAround();
    }

    private void Move()
    {
        Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

        Vector3 moveDirection = new(acceleration.x * movementSpeed * Time.deltaTime, 0, -acceleration.z * movementSpeed * Time.deltaTime);
        Vector3 transformedDirection = transform.TransformDirection(moveDirection);

        characterController.Move(transformedDirection);
    }

    private void LookAround()
    {
        Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();

        rotator.rotation = attitude;
        rotator.Rotate(0f, 0f, 180f, Space.Self);
        rotator.Rotate(90f, 180f, 0f, Space.World);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotator.rotation, smoothing);
    }
}