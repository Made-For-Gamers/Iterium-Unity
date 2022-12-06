using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private SO_Ship ship;
    private Vector3 shipRotate;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
  
    void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Thrust();
    }

    private void Rotate()
    {
        transform.Rotate(input.rotateInput.x * ship.TurnSpeed * Time.deltaTime,0 ,0 );
    }

    private void Thrust()
    {
        rigidBody.AddRelativeForce(new Vector3 (0,input.thrustInput.y * ship.Thrust * Time.deltaTime,0), ForceMode.Force);
    }
}
