using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Timers : MonoBehaviour
{
    public float jumpBufferTimer; //(* used to be timeTilJumpExpire)
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
    public float stunTimer;
    public float immuneTimer;
    public float[] timers;

    void Start()
    {
        jumpBufferTimer = 0.0f;
        dashAttackTimer = 0.0f;
        displacementTimer = 0.0f;
        pivotTimer = 0.0f;
        diveWindUpTimer = 0.0f;
        dashCooldownTimer = 0.0f;
        dashTimer = 0.0f;
        momentumTimer = 0.0f;
        wallStickTimer = 0.0f;
        wallClimbTimer = 0.0f;
        wallJumpTimer = 0.0f;
        wallJumpCooldownTimer = 0.0f;
        stunTimer = 0.0f;
        immuneTimer = 0.0f;
    }

    void Update()
    {
        timers = new float[] { jumpBufferTimer, dashAttackTimer, pivotTimer, diveWindUpTimer, dashCooldownTimer, dashTimer, wallStickTimer, wallClimbTimer, wallClimbCooldownTimer, wallJumpTimer, wallJumpCooldownTimer, stunTimer, immuneTimer };
        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i] > 0.0f)
            {
                timers[i] -= Time.deltaTime;
            }
        }
    }
}
