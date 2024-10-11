/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
        private float maxSpeed;

    InputSystem inputActions;
    InputAction move;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem();
    }

    private void OnEnable()
    {
        move = inputActions.Player.Move();
        inputActions.Enable();

    }

    void Update()
    {
    }
}*/