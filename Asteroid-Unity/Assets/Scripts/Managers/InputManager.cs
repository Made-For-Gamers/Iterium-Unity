using UnityEngine;
using UnityEngine.InputSystem;

namespace Iterium
{
    /// <summary>
    /// Input System for Keyboard/Mouse/Controller (see "Settings/Input System" folder)
    /// Handles device input for the following functions...
    /// * Movement
    /// * Firing
    /// * Shields
    /// </summary>
 
    public class InputManager : MonoBehaviour
    {
        private InputSystem input;
        
        public Vector2 rotateInput;
        public Vector2 thrustInput;
        public bool isfire;
        public bool isShield;
        public bool isWarping;

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
            input.Player.Warp.started += Warp;
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
            input.Player.Warp.started -= Warp;
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
            isShield = true;
        }

        private void Warp(InputAction.CallbackContext obj)
        {
            isWarping = true;
        }
    }
}