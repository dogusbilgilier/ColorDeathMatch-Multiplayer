using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    #region Fields

    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] LayerMask ground;

    [BoxGroup("Forces")][SerializeField] float defaultForce;
    [BoxGroup("Forces")][SerializeField] float boostedForce;
    [BoxGroup("Forces")][SerializeField] float targetForce;
    [BoxGroup("Forces")][SerializeField] float groundDrag;
    [BoxGroup("Forces")][SerializeField][Range(0, 1)] float airMultiplier;

    [BoxGroup("Move Speeds")][SerializeField] float defaultMoveSpeed;
    [BoxGroup("Move Speeds")][SerializeField] float boostedMoveSpeed;
    [BoxGroup("Move Speeds")][SerializeField] float moveSpeed;

    [BoxGroup("Jump")][SerializeField] float jumpForce;
    [BoxGroup("Jump")][SerializeField] float jumpHeight;
    [BoxGroup("Jump")][SerializeField] float fallMultiplier;

    private bool grounded = true;
    private bool checkGround = true;
    private bool checkGrounded;
    private Vector3 moveVector;
    private bool afterJump;

    Fsm fsm;
    Fsm.State boostState;
    Fsm.State idleState;
    Fsm.State moveState;
    Fsm.State jumpState;

    PlayerAnimation playerAnimation;
    AimMamager aimManager;
    private bool startAim;

     InputManager inputManager;
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        aimManager = GetComponent<AimMamager>();
        inputManager = GetComponent<InputManager>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        checkGrounded = true;

        playerAnimation = GetComponent<PlayerAnimation>();

        fsm = new Fsm();
        idleState = Fsm_IdleState;
        boostState = Fsm_BoostState;
        moveState = Fsm_MoveState;
        jumpState = Fsm_JumpState;
        fsm.Start(idleState);
        orientation.rotation = transform.rotation;
        //orientation.position = transform.position;
    }

    private void Update()
    {
      

        
        fsm.OnUpdate();

        if (inputManager.speedBoost && inputManager.movementInputDelta.y>0)
            targetForce = boostedForce;
        else
            targetForce = defaultForce;

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (inputManager.jump && grounded)
            fsm.TransitionTo(jumpState);

        SpeedControl();
    }

    private void FixedUpdate()
    {
       
        if (checkGrounded)
            CheckIfLanded();
        if (inputManager.aim && !startAim)
        {
            playerAnimation.AimAnim(true);
            startAim = true;
            aimManager.StartAim();
        }
        else if(!inputManager.aim && startAim)
        {
            startAim = false;
            playerAnimation.AimAnim(false);
            aimManager.EndAim();
        }

        Move();
        // Fall Speed
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;

    }
    #endregion

    #region PrivateMethods

    void Move()
    {
        moveVector = orientation.forward * inputManager.movementInputDelta.y + orientation.right * inputManager.movementInputDelta.x;
        transform.localEulerAngles += new Vector3(0, inputManager.mouseInputDelta.x, 0);

        if (grounded)
            rb.AddForce(targetForce * Time.fixedDeltaTime * moveVector.normalized, ForceMode.Force);
        else
            rb.AddForce(targetForce * Time.fixedDeltaTime * airMultiplier * moveVector.normalized, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (inputManager.speedBoost)
            moveSpeed = boostedMoveSpeed;
        else
            moveSpeed = defaultMoveSpeed;
        
        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        
    }

    void Jump()
    {
        checkGround= false;
        grounded = false;
        afterJump = true;
        rb.drag = 0;
        //Invoke(nameof(SetCheckGroundTrue), 0.5f);

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        StartCoroutine(JumpCoroutuine()); 
        //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator JumpCoroutuine()
    {
        float startY = transform.position.y;
        float defaultJumpForce = jumpForce;
        while (transform.position.y < startY + jumpHeight)
        {
            yield return new WaitForFixedUpdate();
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpForce -= 0.1f;
           
        }
        checkGround = true;
        jumpForce = defaultJumpForce;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       
    }

    void CheckIfLanded()
    {
        if (checkGround)
        {
            grounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), transform.TransformDirection(Vector3.down), 0.2f, ground);
            if(grounded && afterJump)
            {
                afterJump = false;
                rb.velocity = Vector3.zero;
                if (rb.velocity.magnitude > 0)
                    fsm.TransitionTo(moveState);
                else
                    fsm.TransitionTo(idleState);
            }
            Debug.DrawRay(transform.position + (Vector3.up * 0.1f), transform.TransformDirection(Vector3.down*0.2f),Color.red);
        }
           
    }

    #endregion

    #region StateMachine
    void Fsm_BoostState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        switch(step)
        {
            case Fsm.Step.Enter:
                playerAnimation.BoostAnim(true);
                break;
            case Fsm.Step.Update:
                playerAnimation.SetRunAnimMovement(inputManager.movementInputDelta);

                if (!inputManager.speedBoost || inputManager.movementInputDelta.y > 0)
                    fsm.TransitionTo(moveState);

                break;
            case Fsm.Step.Exit:
                playerAnimation.BoostAnim(false);
                break;
        } 
    }
    void Fsm_IdleState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        switch (step)
        {
            case Fsm.Step.Enter:
                playerAnimation.IdleAnim(true);
                break;
            case Fsm.Step.Update:

                if(inputManager.movementInputDelta.magnitude>0)
                    fsm.TransitionTo(moveState);

                break;
            case Fsm.Step.Exit:
                playerAnimation.IdleAnim(false);
                break;
        }
    }
    void Fsm_MoveState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        switch (step)
        {
            case Fsm.Step.Enter:
                playerAnimation.RunAnim(true);
                break;
            case Fsm.Step.Update:

                playerAnimation.SetRunAnimMovement(inputManager.movementInputDelta);

                if (inputManager.speedBoost && inputManager.movementInputDelta.magnitude > 0 && inputManager.movementInputDelta.y > 0) 
                    fsm.TransitionTo(boostState);

                if (inputManager.movementInputDelta.magnitude <= 0)
                    fsm.TransitionTo(idleState);

                break;
            case Fsm.Step.Exit:
                playerAnimation.RunAnim(false);
                break;
        }
    }
    void Fsm_JumpState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        switch (step)
        {
            case Fsm.Step.Enter:
                playerAnimation.RunAnim(false);
                playerAnimation.BoostAnim(false);
                playerAnimation.JumpAnim();
                Jump();
                break;
            case Fsm.Step.Update:

               /* if (EventManager.SpeedBoost())
                    fsm.TransitionTo(boostState);

                if (EventManager.MovementInputDelta().magnitude <= 0)
                    fsm.TransitionTo(idleState);*/

                break;
            case Fsm.Step.Exit:
                
                break;
        }
    }
    #endregion
}
