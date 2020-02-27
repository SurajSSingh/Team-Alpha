using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerObject : ScriptableObject
{
    // Time Values
    public float wallJumpTime;
    public float stunTime;
    public float immuneTime;
    public float wallClimbTime;

    // Float Values
    public float moveSpeed;
    public float maxDescendAngle;
    //public float rbDrag;
    //public float rbMass;
    public float gravity;
    public float jumpTime;
    //public float timeToJumpApex;
    public float jumpVelocity;
    public float wallJumpForce;
    public float reboundHeight;
    public float knockbackSpeed;
    public float wallSlideSpeed;
    public float wallStickCooldown;
    public float wallClimbCooldown;
    public float jumpDirX;

    // Vector Values
    public Vector2 terminalVel;
    public Vector2 airTerminalVel;
    public Vector2 wallClimb;
    public Vector2 wallJump;
}