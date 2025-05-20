using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    public static PlayerInput PlayerInput; 


    [Header("Move Reference")]
    public static Vector2 Move;


    [Header("Run Reference")]
    public static bool Run;


    [Header("Jump Reference")]
    public static bool PressJump;
    public static bool HoldJump;
    public static bool ReleaseJump;


    [Header("Jump Reference")]
    public static Vector2 Look;


    [Header("Interact Reference")]
    public static bool Interact;


    // Input Actions
    private InputAction moveIA;
    private InputAction jumpIA;
    private InputAction lookIA;
    private InputAction interactIA;
    private InputAction runIA;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveIA = PlayerInput.actions["Move"];
        jumpIA = PlayerInput.actions["Jump"];
        lookIA = PlayerInput.actions["Look"];
        interactIA = PlayerInput.actions["Interact"];
        runIA = PlayerInput.actions["Run"];
    }

    private void Update()
    {
        Move = moveIA.ReadValue<Vector2>();
        Run = runIA.IsPressed();

        PressJump = jumpIA.WasPressedThisFrame();
        HoldJump = jumpIA.IsPressed();
        ReleaseJump = jumpIA.WasReleasedThisFrame();

        Look = lookIA.ReadValue<Vector2>();

        Interact = interactIA.WasPressedThisFrame(); // Depends on interaction
    }

}
