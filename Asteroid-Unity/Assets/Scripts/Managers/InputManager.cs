using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem input;

    //Ship
    public Vector2 rotateInput;
    public Vector2 thrustInput;

    private void OnEnable()
    {
        //Init Input System
        input = new InputSystem();
        input.Player.Enable();

        //Input Events
        input.Player.Movement.started += Rotate;
        input.Player.Movement.canceled += Rotate;
        input.Player.Thrust.started += Thrust;
        input.Player.Thrust.canceled += Thrust;
        input.Player.Fire.started += Fire;
        input.Player.Shield.started += Shield;
    }



    private void OnDisable()
    {
        input.Player.Movement.started -= Rotate;
        input.Player.Thrust.started -= Thrust;
        input.Player.Fire.started -= Fire;
        input.Player.Shield.started -= Shield;
    }

    void Start()
    {

    }

    private void Rotate(InputAction.CallbackContext obj)
    {
        rotateInput = obj.ReadValue<Vector2>();
    }

    private void Thrust(InputAction.CallbackContext obj)
    {
        thrustInput = obj.ReadValue<Vector2>();
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        print("Fire");
    }

    private void Shield(InputAction.CallbackContext obj)
    {
        print("Shield");
    }

}
