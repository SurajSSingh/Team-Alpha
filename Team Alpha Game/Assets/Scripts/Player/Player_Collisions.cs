using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collisions : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;

    //Positional States
    private bool onGround;
    private bool airborne;

    //Action States
    private bool stunned;

    //Directional States
    private float enemyColSign;

    //Timers
    private bool stunTimer;
    private bool immuneTimer;

    //Movement Stats
    private float knockbackSpeed;

    //Timer Values
    private float stunTime;
    private float immuneTime;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        knockbackSpeed = attributes.knockbackSpeed;
        stunTime = attributes.stunTime;
        immuneTime = attributes.immuneTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!state.immune)
            {
                StartKnockback();
            }
        }
    }
    
    public void Knockback(ref Vector3 velocity, float enemyColSign)
    {
        if (timers.stunTimer >= stunTime-0.5f && timers.stunTimer <= stunTime)
        {
            velocity.x = knockbackSpeed * 5.0f * enemyColSign;
            velocity.y = knockbackSpeed/1.5f;
        }
        else if (timers.stunTimer >= stunTime-1.0f && timers.stunTimer < stunTime - 0.5f && !onGround)
        {
            velocity.x = knockbackSpeed/2.0f * enemyColSign;
            velocity.y = -knockbackSpeed;
        }
        else if (timers.stunTimer >= 0.0f && timers.stunTimer < 0.5f)
        {
            if (state.onGround)
            {
                animator.AnimatorGetup(true);
            }
        }
    }

    public void ManageKnockback()
    {
        if (timers.stunTimer <= 0.0f)
        {
            state.stunned = false;
            animator.AnimatorStunned();
            animator.AnimatorGetup(false);
            state.control = true;
        }
    }

    private void StartKnockback()
    {
        state.stunned = true;
        state.immune = true;
        state.control = false;
        animator.AnimatorStunned();
        timers.stunTimer = stunTime;
        timers.immuneTimer = immuneTime;
    }
}
