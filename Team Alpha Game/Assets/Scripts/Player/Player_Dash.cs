using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dash : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    Speed_Manager speedManager;
    WallCheck leftWallCheck;
    WallCheck rightWallCheck;

    //Velocity
    private Vector3 velocity;

    //Positional States
    private bool onWall;

    //Action States
    private bool control;
    private bool dashing;
    private bool dashReady;
    private bool dashAttacking;
    private bool stunned;

    //Directional States
    private Vector2 input;
    private Vector2 dashDir;

    //Ability Access States
    private bool dash;

    //Timers
    private float dashCooldownTimer;
    private float dashAttackTimer;
    private float dashTimer;
    private float momentumTimer;

    //Movement Stats
    private float moveSpeed;
    private float dashSpeed;

    //Timer Values
    private float dashCooldownTime;
    private float dashTime;
    private float dashAttackTime;
    private float momentumTime;

    //Physics
    private LayerMask collisionMask;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        speedManager = GetComponent<Speed_Manager>();
        leftWallCheck = gameObject.transform.GetChild(2).gameObject.GetComponent<WallCheck>();
        rightWallCheck = gameObject.transform.GetChild(3).gameObject.GetComponent<WallCheck>();
        velocity = speedManager.velocity;
        onWall = state.onWall;
        control = state.control;
        dashing = state.dashing;
        dashReady = state.dashReady;
        dashAttacking = state.dashAttacking;
        stunned = state.stunned;
        input = state.input;
        dashDir = state.dashDir;
        dash = state.dash;
        dashCooldownTimer = timers.dashCooldownTimer;
        dashAttackTimer = timers.dashCooldownTimer;
        dashTimer = timers.dashTimer;
        momentumTimer = timers.momentumTimer;
        moveSpeed = attributes.moveSpeed;
        dashSpeed = attributes.dashSpeed;
        dashCooldownTime = attributes.dashCooldownTime;
        dashTime = attributes.dashTime;
        dashAttackTime = attributes.dashAttackTime;
        momentumTime = attributes.momentumTime;
        collisionMask = attributes.collisionMask;
    }

    void Update()
    {
        ReceiveValues();
        if (dashCooldownTimer <= 0.0f)
        {
            dashReady = true;
        }
        if (control && dash) //If player has control and has access to dash ability
        {
            if (Input.GetKeyDown(KeyCode.E) && dashReady && input != Vector2.zero && input.y != -1.0f) //Player can not dash downward or without inputting a direction
            {
                StartDash();
            }
        }
        else if (dashing && !dashAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartDashAttack();
            }
        }
        SendValues();
    }

    public void Dash(ref Vector3 velocity, Vector2 direction) //Adjusts dash speed based on angle and collisions in dash path
    {
        Debug.Log(Time.deltaTime);
        AngleCheck(ref velocity, direction);
        if (direction != Vector2.zero)
        {
            Vector2 rayOrigin = GenerateRaycastOrigins(direction);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 0.8f, collisionMask);
            //If a collision is detected along the path of the dash, speed will be dampened to ensure player doesn't phase through object
            if (hit && hit.distance - 0.15f <= velocity.magnitude * Time.deltaTime * 1.65f)
            {
                velocity = Vector2.zero;
            }
        }
    }

    public void ManageDash() //If player is stunned, cancel dash, else continue
    {
        animator.AnimatorDash();
        if (!state.stunned && !state.onWall)
        {
            if (timers.dashTimer <= 0.0f)
            {
                if (dashAttacking)
                {
                    StartDashAttack();
                }
                else
                {
                    ResetDash();
                }
            }
        }
        else
        {
            state.dashing = false;
            animator.AnimatorDash();
            if (!state.stunned)
            {
                state.control = true;
            }
        }
    }

    public void DashAttack(ref Vector3 velocity) //Suspends player in place while dash attacking
    {
        velocity = Vector2.zero;
    }

    public void ManageDashAttack() //If player is stunned, cancel dash attack, else continue
    {
        animator.AnimatorDash();
        animator.AnimatorDashAttack();
        if (!state.stunned)
        {
            if (timers.dashAttackTimer <= 0.0f)
            {
                state.dashAttacking = false;
                animator.AnimatorDashAttack();
                state.control = true;
                leftWallCheck.attacking = false;
                rightWallCheck.attacking = false;
            }
        }
        else
        {
            state.dashAttacking = false;
            animator.AnimatorDashAttack();
            leftWallCheck.attacking = false;
            rightWallCheck.attacking = false;
        }
    }

    private void AngleCheck(ref Vector3 velocity, Vector2 direction) //Dampens dashSpeed if dashing up or diagonally
    {
        if ((Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1))
        {
            velocity = direction * (dashSpeed / 2);
        }
        else if (direction.x == 0 && direction.y != 0)
        {
            velocity = direction * (dashSpeed * 0.75f);
        }
        else if (direction.x != 0 && direction.y == 0)
        {
            velocity = direction * dashSpeed;
        }
    }

    private Vector2 GenerateRaycastOrigins(Vector2 direction)
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 rayOrigin = Vector2.zero;
        if (direction.x == 0.0f && direction.y < 0.0f) //Down
        {
            rayOrigin = new Vector2(bounds.center.x, bounds.max.y);
        }
        else if (direction.x == 0.0f && direction.y > 0.0f) //Up
        {
            rayOrigin = new Vector2(bounds.center.x, bounds.min.y);
        }
        else if (direction.x < 0.0f && direction.y == 0.0f) //Left
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.center.y);
        }
        else if (direction.x > 0.0f && direction.y == 0.0f) //Right
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.center.y);
        }
        else if (direction.x < 0.0f && direction.y < 0.0f) //DownLeft
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.min.y);
        }
        else if (direction.x > 0.0f && direction.y < 0.0f) //DownRight
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.min.y);
        }
        else if (direction.x < 0.0f && direction.y > 0.0f) //UpLeft
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.max.y);
        }
        else if (direction.x > 0.0f && direction.y > 0.0f) //UpRight
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.max.y);
        }
        return rayOrigin;
    }

    private void StartDash() //Starts dash in player input direction
    {
        dashDir = state.input;
        dashing = true;
        control = false;
        dashReady = false;
        dashTimer = dashTime;
        momentumTimer = momentumTime;
        dashCooldownTimer = dashCooldownTime;
    }

    private void ResetDash() //Resets dash cooldown timer and gives control back to player, ending the dash
    {
        state.dashing = false;
        animator.AnimatorDash();
        state.control = true;
        timers.momentumTimer = momentumTime;
        speedManager.velocity = speedManager.velocity / 4.0f;
    }

    private void StartDashAttack() //Queues dash attack to occur immediately after dash
    {
        dashAttacking = true;
        dashAttackTimer = dashAttackTime;
        dashing = false;
        if (state.sign == 1.0f) //If facing right, set right wall to attack mode
        {
            rightWallCheck.attacking = true;
        }
        else
        {
            leftWallCheck.attacking = true;
        }
    }

    private void ReceiveValues()
    {
        velocity = speedManager.velocity;
        onWall = state.onWall;
        control = state.control;
        dashing = state.dashing;
        dashReady = state.dashReady;
        dashAttacking = state.dashAttacking;
        stunned = state.stunned;
        input = state.input;
        dashDir = state.dashDir;
        dash = state.dash;
        dashCooldownTimer = timers.dashCooldownTimer;
        dashAttackTimer = timers.dashAttackTimer;
        dashTimer = timers.dashTimer;
        momentumTimer = timers.momentumTimer;
    }

    private void SendValues()
    {
        state.control = control;
        state.dashing = dashing;
        state.dashReady = dashReady;
        state.dashAttacking = dashAttacking;
        state.stunned = stunned;
        state.input = input;
        state.dashDir = dashDir;
        state.dash = dash;
        timers.dashCooldownTimer = dashCooldownTimer;
        timers.dashAttackTimer = dashAttackTimer;
        timers.dashTimer = dashTimer;
        timers.momentumTimer = momentumTimer;
    }
}
