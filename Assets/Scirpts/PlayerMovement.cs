using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem inputSystem;
    CharacterController characterController;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRollMovement;

    bool isMovementPressed;
    bool isRollPressed;
    bool isAttackPressed;
    bool isRolling;
    bool isAttacking;
    bool isDead; // Track if the player is dead

    float rotationFactorPerFrame = 15f;
    public float rollMultiplier = 3f;
    public float movementSpeed = 5f; // New variable for movement speed

    Animator anim;
    private float gravity;

    void Awake()
    {
        inputSystem = new InputSystem();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Movement input handling
        inputSystem.CharacterControl.Move.started += onMovementInput;
        inputSystem.CharacterControl.Move.canceled += onMovementInput;
        inputSystem.CharacterControl.Move.performed += onMovementInput;

        // Attack input handling
        inputSystem.CharacterControl.Fire.started += onAttackInput;

        // Roll input handling
        inputSystem.CharacterControl.Roll.started += onRollInput;
        inputSystem.CharacterControl.Roll.canceled += onRollInput;
    }

    public void DisableInputs()
    {
        // Called when the player dies
        isDead = true;
        inputSystem.CharacterControl.Disable(); // Disable the input system
        anim.SetTrigger("Die"); // Play death animation
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        if (isDead) return; // Prevent input if dead

        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRollMovement.x = currentMovementInput.x * rollMultiplier;
        currentRollMovement.z = currentMovementInput.y * rollMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void onRollInput(InputAction.CallbackContext context)
    {
        if (isDead) return; // Prevent rolling if dead

        isRollPressed = context.ReadValueAsButton();

        if (isRollPressed && !isRolling && !isAttacking) // Prevent roll during attack
        {
            anim.SetTrigger("Roll");
            isRolling = true;
        }
    }

    void onAttackInput(InputAction.CallbackContext context)
    {
        if (isDead) return; // Prevent attacking if dead

        isAttackPressed = context.ReadValueAsButton();

        if (isAttackPressed && !isAttacking && !isRolling) // Prevent attack during roll
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    void handleRotation()
    {
        if (isDead) return; // Prevent rotation if dead

        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void handleAnimation()
    {
        if (isDead) return; // Prevent animations if dead

        bool isWalking = anim.GetBool("IsWalking");

        if (isMovementPressed && !isWalking && !isRolling && !isAttacking)
        {
            anim.SetBool("IsWalking", true);
        }
        else if (!isMovementPressed && isWalking)
        {
            anim.SetBool("IsWalking", false);
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Roll"))
        {
            isRolling = true;
        }
        else
        {
            isRolling = false;
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && isRollPressed)
            {
                anim.ResetTrigger("Roll");
            }
        }

        if (stateInfo.IsTag("Attack"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && isAttackPressed)
            {
                anim.ResetTrigger("Attack");
            }
        }
    }

    void Update()
    {
        if (isDead)
        {
            // Ensure gravity is applied so the player doesn't float
            Vector3 gravityMovement = new Vector3(0, gravity * Time.deltaTime, 0);
            characterController.Move(gravityMovement);
            return;
        }

        handleAnimation();
        handleRotation();

        if (isRolling)
        {
            characterController.Move(currentRollMovement * Time.deltaTime);
        }
        else if (!isAttacking) // Prevent movement while attacking
        {
            characterController.Move(currentMovement * movementSpeed * Time.deltaTime); // Apply movement speed here
        }
    }

    void OnEnable()
    {
        if (!isDead) inputSystem.CharacterControl.Enable(); // Enable inputs only if alive
    }

    void OnDisable()
    {
        inputSystem.CharacterControl.Disable();
    }
}
