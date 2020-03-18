using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Timers : MonoBehaviour
{
    public float jumpBufferTimer; //(* used to be timeTilJumpExpire)
    public float jumpTimer;
    public float attackTimer;
    public float dashAttackTimer;
    public float displacementTimer;
    public float pivotTimer;
    public float diveWindUpTimer;
    public float dashCooldownTimer;
    public float dashTimer;
    public float momentumTimer;
    public float wallStickTimer;
    public float wallClimbTimer;
    public float wallClimbCooldownTimer;
    public float wallJumpTimer;
    public float wallJumpCooldownTimer;
    public float landingTimer;
    public float stunTimer;
    public float immuneTimer;

    void Start()
    {
        jumpBufferTimer = 0.0f;
        jumpTimer = 0.0f;
        attackTimer = 0.0f;
        dashAttackTimer = 0.0f;
        displacementTimer = 0.0f;
        pivotTimer = 0.0f;
        diveWindUpTimer = 0.0f;
        dashCooldownTimer = 0.0f;
        dashTimer = 0.0f;
        momentumTimer = 0.0f;
        wallStickTimer = 0.0f;
        wallClimbTimer = 0.0f;
        wallClimbCooldownTimer = 0.0f;
        wallJumpTimer = 0.0f;
        wallJumpCooldownTimer = 0.0f;
        landingTimer = 0.0f;
        stunTimer = 0.0f;
        immuneTimer = 0.0f;
    }

    void Update()
    {
        DecrementTimer(ref jumpBufferTimer);
        DecrementTimer(ref jumpTimer);
        DecrementTimer(ref attackTimer);
        DecrementTimer(ref dashAttackTimer);
        DecrementTimer(ref displacementTimer);
        DecrementTimer(ref pivotTimer);
        DecrementTimer(ref diveWindUpTimer);
        DecrementTimer(ref dashCooldownTimer);
        DecrementTimer(ref dashTimer);
        DecrementTimer(ref wallStickTimer);
        DecrementTimer(ref wallClimbTimer);
        DecrementTimer(ref wallClimbCooldownTimer);
        DecrementTimer(ref wallJumpTimer);
        DecrementTimer(ref wallJumpCooldownTimer);
        DecrementTimer(ref landingTimer);
        DecrementTimer(ref stunTimer);
        DecrementTimer(ref immuneTimer);
    }

    private void DecrementTimer(ref float timer)
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
