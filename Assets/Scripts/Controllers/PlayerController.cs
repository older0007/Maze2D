using System;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
         [SerializeField] private InputActionAsset inputSystem;
         [SerializeField] private Player mover;

         private InputAction moveAction;
         private Vector2 moveInput;

         private const string MoveInput = "Move";
         private void Awake()
         {
             InitInput();
         }
         
         private void InitInput()
         {
             moveAction = inputSystem.FindAction(MoveInput);
            
             moveAction.Enable();
         }

         private void FixedUpdate()
         {
             Vector2 input = moveAction.ReadValue<Vector2>();

             mover.Move(input);
         }

         private void OnDestroy()
         {
             moveAction.Disable();
         }
    }
}