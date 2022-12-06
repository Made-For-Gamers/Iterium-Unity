using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem input;

    private void OnEnable()
    {
        //Init Input System
        input = new InputSystem();
        input.Player.Enable();

        //Input Events
        input.Player.Movement.started += Move;
        input.Player.Fire.started += Fire;
        input.Player.Shield.started += Shield;
    }

    private void OnDisable()
    {

    }

    void Start()
    {

    }

    private void Move(InputAction.CallbackContext obj)
    {

    }

    private void Fire(InputAction.CallbackContext obj)
    {
       
    }

    private void Shield(InputAction.CallbackContext obj)
    {
       
    }

}
