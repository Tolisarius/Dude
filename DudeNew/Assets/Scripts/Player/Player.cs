using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    [Space(10)]
    [Header("Jumps")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;

    [Space(10)]
    [Header("Movement")]
    public float moveSpeed = 6;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    [Space(10)]
    [Header("Wall")]
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public Vector2 wallJumpOff;

    [Header("Wall climb")]
    public bool wallClimbEnabled;
    public Vector2 wallJumpClimb;


    [Header("Wall leap")]
    public bool wallLeapEnabled;
    public bool wallLeapDirectionRequired;
    public float wallLeapAfterWallBuffer, wallLeapAfterButtonBuffer;
    public Vector2 wallLeap;

    float timeToWallUnstick;

    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float verticalPush;
    [HideInInspector]
    public float maxJumpVelocity, minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    public ControllerStates controllerStates; 
    PlayerStates playerStates;

    Animator animator;

    [HideInInspector]
    public Vector2 directionalInput;
    Vector2 _directionalInputBuffer;
    bool _jumpPressed, _jumpPressedBuffer;
    int _wallDirX, _wallDirXBuffer;

    bool _directionalInputRestrictedX, _directionalInputRestrictedY;


    [HideInInspector]
    public string currentDirection;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        controllerStates = controller.controllerStates;
        playerStates = GetComponent<PlayerStates>();       
        animator = GetComponent<Animator>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        print("Gravity:" + gravity);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        if (controllerStates.faceDir == -1)
        {
            currentDirection = "left";
        }
        else
        {
            currentDirection = "right";
        }
    }

    void Update()
    {
        EveryFrame();
        //print("State:" + playerStates.currentState);
    }

    void EveryFrame()
    {
        CalculateVelocity();     
        controller.Move(velocity * Time.deltaTime, directionalInput);
        VerticalCollisionsHandling();       // must be AFTER controller.Move
        HandleCharacterDirection();
      //PlayerStates();

    }


    /// <summary>
    /// Input reactions
    /// </summary>

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        if (_directionalInputRestrictedX)
        {
            directionalInput.x = 0;
        }
        if (_directionalInputRestrictedY)
        {
            directionalInput.y = 0;
        }
    }

    /// <summary>
    /// Controlls stuff
    /// </summary>
    /// <summary>
    /// Physics stuff
    /// </summary>
    void HandleCharacterDirection()
    {
        if (controllerStates.faceDir == -1)
        {
            SwitchFaceOrientation("left");
        }
        if (controllerStates.faceDir == 1)
        {
            SwitchFaceOrientation("right");
        }
    }
    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controllerStates.IsCollidingBelow) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += (gravity * Time.deltaTime)+verticalPush;
    }
    void VerticalCollisionsHandling()
    {
        if (controllerStates.IsCollidingAbove || controllerStates.IsCollidingBelow)
        {
            if (controllerStates.slidingDownMaxSlope)
            {
                velocity.y += controllerStates.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

      

    /// <summary>
    ///  Player methods
    /// </summary>
    /// 
    public void SwitchFaceOrientation(string dir)
    {
        Vector2 newScale = gameObject.transform.localScale;
        if (dir == "left")
        {
            newScale.x = Mathf.Abs(newScale.x);
            currentDirection = "left";
        }
        else if (dir == "right")
        {
            newScale.x = Mathf.Abs(newScale.x) * -1;
            currentDirection = "right";
        }
        gameObject.transform.localScale = newScale;
    }

    public void RestrictMovement(bool xAxisrestricted, bool yAxisRestricted, bool jumpRestricted)
    {
        if (xAxisrestricted)
        {
            _directionalInputRestrictedX = true;
        }
        else
        {
            _directionalInputRestrictedX = false;
        }
        if (yAxisRestricted)
        {
            _directionalInputRestrictedY = true;
        }
        else
        {
            _directionalInputRestrictedY = false;
        }
        if (jumpRestricted)
        {
            playerStates.JumpRestricted=true;
        }
        else
        {
            playerStates.JumpRestricted = false;
        }
    }
/*
    /// <summary>
    ///  Player FSM
    /// </summary>
    /// 
    void PlayerStates()
    {
        switch (playerStates.currentState)
        {
            case global::PlayerStates.State.standing: Standing(); break;
            case global::PlayerStates.State.walking: Walking(); break;
            case global::PlayerStates.State.falling: Falling(); break;
            case global::PlayerStates.State.jumping: Jumping(); break;
            case global::PlayerStates.State.wallSliding: WallSliding(); break;
            case global::PlayerStates.State.wallLeaping: WallLeap(); break;
            case global::PlayerStates.State.attackingNormal: AttackNormal(); break;
            case global::PlayerStates.State.attackingAerial:AttackAerial(); break;
            case global::PlayerStates.State.attackingSmash: AttackSmash(); break;

            default: print("SOMETHING IS FUCKED UP WITH PLAYER STATE"); break;
        }
    }

    void Standing()
    {
        //print("STANDING");
        animator.SetBool("IsStanding", true);

        if (T_WallSlidingTransition())
        {
            SwitchState("IsStanding", global::PlayerStates.State.wallSliding);
        }
        else if (T_JumpTransition())
        {
            SwitchState("IsStanding", global::PlayerStates.State.jumping);
        }
        else if (T_AttackingNormal())
        {
            SwitchState("IsStanding", global::PlayerStates.State.attackingNormal);
        }
        else if (T_WalkingTransition())
        {
            SwitchState("IsStanding", global::PlayerStates.State.walking);
        }
    }
    void Walking()
    {
        //print("WALKING");
        animator.SetBool("IsRunning", true);
        if (T_StandingTransition())
        {
            SwitchState("IsRunning", global::PlayerStates.State.standing);
        }
        else if (T_FallingTransition())
        {
            SwitchState("IsRunning", global::PlayerStates.State.falling);
        }
        else if (T_JumpTransition())
        {
            SwitchState("IsRunning", global::PlayerStates.State.jumping);
        }
        else if (T_AttackingNormal())
        {
            SwitchState("IsRunning", global::PlayerStates.State.attackingNormal);
        }
    }


    void Falling()
    {
        //print("FALLING");
        animator.SetBool("IsFalling", true);

        if (T_WallSlidingTransition())
        {
            SwitchState("IsFalling", global::PlayerStates.State.wallSliding);
        }
        else if (T_WallLeapTransition())
        {
            SwitchState("IsFalling", global::PlayerStates.State.wallLeaping);
        }
        else if (T_AttackingSmash())
        {
            SwitchState("IsFalling", global::PlayerStates.State.attackingSmash);
        }
        else if (T_AttackingAerial())
        {
            SwitchState("IsFalling", global::PlayerStates.State.attackingAerial);
        }
        else if (T_WalkingTransition())
        {
            SwitchState("IsFalling", global::PlayerStates.State.walking);
        }

        else if (T_StandingTransition())
        {
            SwitchState("IsFalling", global::PlayerStates.State.standing);
        }

    }

    void Jumping()
    {
        print("JUMPING");
        animator.SetBool("IsJumping", true);

        if (T_WallSlidingTransition())
        {
            SwitchState("IsJumping", global::PlayerStates.State.wallSliding);
        }
        else if (T_AttackingSmash())
        {
            SwitchState("IsJumping", global::PlayerStates.State.attackingSmash);
        }
        else if (T_AttackingAerial())
        {
            SwitchState("IsJumping", global::PlayerStates.State.attackingAerial);
        }
        else if (T_StandingTransition())
        {
            SwitchState("IsJumping", global::PlayerStates.State.standing);
        }
        else if (T_FallingTransition())
        {
            SwitchState("IsJumping", global::PlayerStates.State.falling);
        }

    }
    void WallSliding()
    {
        print("WALL SLIDING PYCO!!!");
        animator.SetBool("IsWallSliding", true);

        if (T_StandingTransition())
        {
            SwitchState("IsWallSliding", global::PlayerStates.State.standing);
        }
        else if (T_WallLeapTransition())
        {
            SwitchState("IsWallSliding", global::PlayerStates.State.wallLeaping);
        }

        else if (T_FallingTransition())
        {
            SwitchState("IsWallSliding", global::PlayerStates.State.falling);
        }

    }

    void WallLeap()
    {
        print("WALL LEAPING");       
        animator.SetBool("IsWallLeaping", true);

        if (T_WallSlidingTransition())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.wallSliding);
        }
        else if (T_AttackingSmash())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.attackingSmash);
        }
        else if (T_AttackingAerial())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.attackingAerial);
        }
        else if (T_AttackingSmash())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.attackingSmash);
        }

        else if (T_FallingTransition())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.falling);
        }
     
        else if (T_StandingTransition())
        {
            SwitchState("IsWallLeaping", global::PlayerStates.State.standing);
        }
    }
    void AttackNormal()
    {
        animator.SetBool("IsAttackingNormal", true);
        if (!T_AttackingNormal())
        {
            SwitchState("IsAttackingNormal", global::PlayerStates.State.standing);
        }       
    }

    void AttackAerial()
    {
        print("ATTACK AERIAL!");
        animator.SetBool("IsAttackingAerial", true);
        if (!T_AttackingAerial())
        {
            SwitchState("IsAttackingAerial", global::PlayerStates.State.falling);
        }
    }

    void AttackSmash()
    {
        animator.SetBool("IsAttackingSmash", true);
        if (!T_AttackingSmash())
        {
            SwitchState("IsAttackingSmash", global::PlayerStates.State.standing);
            RestrictMovement(false, false,false);
        }
    }


    /// <summary>
    ///  State transitions
    /// </summary>

    bool T_StandingTransition()
    {
        return (controllerStates.IsGrounded && Mathf.Abs(velocity.x) < 1);
    }
    bool T_WalkingTransition()
    {
        return (controllerStates.IsGrounded && Mathf.Abs(velocity.x) >= 1);
    }

    bool T_FallingTransition()
    {
        return (controllerStates.IsFalling && !playerStates.IsWallSliding && !playerStates.IsWallLeaping);
    }

    bool T_JumpTransition()
    {
        return (controllerStates.IsNormalJumping);
    }
    bool T_WallSlidingTransition()
    {
        return (playerStates.IsWallSliding);
    }
    bool T_WallLeapTransition()
    {
        return playerStates.IsWallLeaping;
    }
    bool T_AttackingNormal()
    {
        return playerStates.IsAttackingNormal;
    }
    bool T_AttackingAerial()
    {
        return playerStates.IsAttackingAerial;
    }
    bool T_AttackingSmash()
    {
        return playerStates.IsAttackingSmash;
    }

    void SwitchState(string currentAnimStateOff, global::PlayerStates.State state)
    {       
        animator.SetBool(currentAnimStateOff, false);
        playerStates.currentState = state;
    }
    IEnumerator SwitchStateDelayed(string currentAnimStateOff, global::PlayerStates.State state, float time)
    {
        animator.SetBool(currentAnimStateOff, false);
        yield return new WaitForSeconds(time);       
        playerStates.currentState = state;
    }
*/
}
