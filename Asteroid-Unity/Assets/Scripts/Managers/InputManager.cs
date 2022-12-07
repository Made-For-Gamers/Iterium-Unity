using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Input System - see "Settings/Input System" to change the controls
    private InputSystem input;
    
    //The following values are read from the PlayerController script
    public Vector2 rotateInput; //Ship rotation
    public Vector2 thrustInput; //Ship thrust
    public bool isfire; //Player firing

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

    //Clean up of input events
    private void OnDisable()
    {
        input.Player.Movement.started -= Rotate;
        input.Player.Movement.canceled -= Rotate;
        input.Player.Thrust.started -= Thrust;
        input.Player.Thrust.canceled -= Thrust;
        input.Player.Fire.started -= Fire;
        input.Player.Shield.started -= Shield;
    }

    //Ship rotation input
    private void Rotate(InputAction.CallbackContext obj)
    {
        rotateInput = obj.ReadValue<Vector2>();
    }

    //Ship thrust input
    private void Thrust(InputAction.CallbackContext obj)
    {
        thrustInput = obj.ReadValue<Vector2>();
    }

    //Ship fire input
    private void Fire(InputAction.CallbackContext obj)
    {
        isfire = true;
    }

    //Ship shield input
    private void Shield(InputAction.CallbackContext obj)
    {
        print("Shield");
    }

}
