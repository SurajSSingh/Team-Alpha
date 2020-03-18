using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    WallCheck leftWallCheck;
    WallCheck rightWallCheck;

    //Positional States
    private bool onGround;

    //Action States
    private bool control;
    private bool attacking;

    //Timers
    private float attackTimer;

    //Timer Values
    private float attackTime;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        leftWallCheck = gameObject.transform.GetChild(2).gameObject.GetComponent<WallCheck>();
        rightWallCheck = gameObject.transform.GetChild(3).gameObject.GetComponent<WallCheck>();
        ReceiveValues();
        attackTime = attributes.attackTime;
    }
    void Update()
    {
        ReceiveValues();
        if (control)
        {
            if (Input.GetAxisRaw("Fire3") == 1.0f && onGround) //Player is grounded and presses Z key
            {
                StartAttack();
            }
        }
        SendValues();
    }

    public void Attack(ref Vector3 velocity, float sign) //Move player forward slightly while attacking
    {
        velocity.x = 0.5f * sign;
        velocity.y = 0;
    }

    public void ManageAttack() //If player is stunned or attack timer is up, cancel attack animation, else continue
    {
        animator.AnimatorAttack();
        if (!state.stunned)
        {
            if (timers.attackTimer <= 0.0f) 
            {
                state.attacking = false;
                animator.AnimatorAttack();
                state.control = true;
                leftWallCheck.attacking = false;
                rightWallCheck.attacking = false;
            }
        }
        else if (state.stunned)
        {
            state.attacking = false;
            animator.AnimatorAttack();
            leftWallCheck.attacking = false;
            rightWallCheck.attacking = false;
        }
    }

    private void StartAttack()
    {
        attacking = true;
        control = false;
        attackTimer = attackTime;
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
        onGround = state.onGround;
        control = state.control;
        attacking = state.attacking;
        attackTimer = timers.attackTimer;
    }

    private void SendValues()
    {
        state.control = control;
        state.attacking = attacking;
        timers.attackTimer = attackTimer;
    }
}
