using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    SpriteRenderer ren;
    Animator animator;
    Player_State state;
    Player_Timers timers;
    PlayerManager manager;

    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        state = GetComponent<Player_State>();
        timers = GetComponent<Player_Timers>();
        manager = GetComponent<PlayerManager>();
    }
    public void AnimatorHealth()
    {
        animator.SetFloat("Health", manager.currHealth);
    }

    public void AnimatorMomentum()
    {
        animator.SetFloat("Momentum Timer", timers.momentumTimer);
    }

    public void AnimatorSpeedX(float speed)
    {
        animator.SetFloat("SpeedX", speed);
    }

    public void AnimatorSpeedY(float speed)
    {
        animator.SetFloat("SpeedY", speed);
    }

    public void AnimatorJump()
    {
        animator.SetBool("Jumping", state.jumping);
    }

    public void AnimatorDoubleJump()
    {
        animator.SetBool("Double Jump", state.doubleJumping);
    }

    public void AnimatorAirborne()
    {
        animator.SetBool("Airborne", state.airborne);
    }

    public void AnimatorAttack()
    {
        animator.SetBool("Attack", state.attacking);
    }

    public void AnimatorGrounded()
    {
        animator.SetBool("Grounded", state.onGround);
    }

    public void AnimatorStunned()
    {
        animator.SetBool("Stunned", state.stunned);
    }

    public void AnimatorDash()
    {
        animator.SetBool("Dashing", state.dashing);
    }

    public void AnimatorDashAttack()
    {
        animator.SetBool("Dash Attack", state.dashAttacking);
    }

    public void AnimatorWallSlide()
    {
        animator.SetBool("Wall Sliding", state.wallSliding);
    }

    public void AnimatorWallClimb()
    {
        animator.SetBool("Wall Climbing", state.wallClimbing);
    }

    public void AnimatorWallJump()
    {
        animator.SetBool("Wall Jumping", state.wallJumping);
    }

    public void AnimatorPivot()
    {
        animator.SetBool("Pivoting", state.pivoting);
    }

    public void AnimatorDive()
    {
        animator.SetBool("Diving", state.diving);
    }

    public void AnimatorLanding()
    {
        animator.SetBool("Landing", state.landing);
    }

    public void AnimatorDiveAttack()
    {
        animator.SetBool("Dive Attack", state.diveHit);
    }

    public void SetFlip(bool flip) //Flip sprite horizontally if facing the opposite direction
    {
        ren.flipX = flip;
    }

    public void ToggleFlip() //Flip sprite horizontally from the direction player is currently facing
    {
        if (ren.flipX) //if facing left
        {
            ren.flipX = false; //face right
        }
        else //if facing right
        {
            ren.flipX = true; //face left
        }
    }
}
