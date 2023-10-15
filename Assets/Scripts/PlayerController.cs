using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameInput gameInput;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameInput = GameInput.Instance;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 direction;
        if (Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
        {
            movementVector.y = 0;
            direction = Vector3.right * movementVector.x;
        }
        else
        {
            movementVector.x = 0;
            direction = Vector3.forward * movementVector.y;
        }

        if (movementVector != Vector2.zero)
        {
            rb.Move(transform.position + new Vector3(Mathf.Round(movementVector.x), 0, Mathf.Round(movementVector.y)) * movementSpeed * Time.deltaTime,
                Quaternion.LookRotation(direction.normalized));
        }

    }

}
