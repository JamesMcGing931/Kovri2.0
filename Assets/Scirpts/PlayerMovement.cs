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
    bool isDead; 

    float rotationFactorPerFrame = 15f;
    public float rollMultiplier = 3f;
    public float movementSpeed = 5f; 

    public GameObject bombPrefab; 
    public float bombSpawnDistance = 2f; 

    Animator anim;
    private float gravity;

    void Awake()
    {
        inputSystem = new InputSystem();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        inputSystem.CharacterControl.Move.started += onMovementInput;
        inputSystem.CharacterControl.Move.canceled += onMovementInput;
        inputSystem.CharacterControl.Move.performed += onMovementInput;

        inputSystem.CharacterControl.Fire.started += onAttackInput;

        inputSystem.CharacterControl.Bomb.started += onBombInput;


        inputSystem.CharacterControl.Roll.started += onRollInput;
        inputSystem.CharacterControl.Roll.canceled += onRollInput;
    }

    public void DisableInputs()
    {
        isDead = true;
        inputSystem.CharacterControl.Disable(); 
        anim.SetTrigger("Die"); 
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        if (isDead) return; 

        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRollMovement.x = currentMovementInput.x * rollMultiplier;
        currentRollMovement.z = currentMovementInput.y * rollMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void onRollInput(InputAction.CallbackContext context)
    {
        if (isDead) return; 

        isRollPressed = context.ReadValueAsButton();

        if (isRollPressed && !isRolling && !isAttacking) 
        {
            anim.SetTrigger("Roll");
            isRolling = true;
        }
    }

    void onAttackInput(InputAction.CallbackContext context)
    {
        if (isDead) return; 

        isAttackPressed = context.ReadValueAsButton();

        if (isAttackPressed && !isAttacking && !isRolling) 
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    void onBombInput(InputAction.CallbackContext context)
    {
        if (isDead) return; 

        BombManager bombManager = GetComponent<BombManager>();
        if (bombManager != null)
        {
            bombManager.UseBomb();
        }
    }

    void handleRotation()
    {
        if (isDead) return; 

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
        if (isDead) return; 

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
        else if (!isAttacking) 
        {
            characterController.Move(currentMovement * movementSpeed * Time.deltaTime); 
        }
    }

    void OnEnable()
    {
        if (!isDead) inputSystem.CharacterControl.Enable(); 
    }

    void OnDisable()
    {
        inputSystem.CharacterControl.Disable();
    }
}