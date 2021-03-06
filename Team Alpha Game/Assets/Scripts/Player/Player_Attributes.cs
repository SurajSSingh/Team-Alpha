﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Player/Player Attributes")]
public class Player_Attributes : ScriptableObject
{
    //General Stats
    public float health = 100.0f; //Full capacity of player health
    public float damage = 10.0f; //Base value of damage player deals with normal attacks

    //Movement Stats
    public float moveSpeed = 6.0f; //Horizontal movement speed
    public float maxDescendAngle = 60.0f; //Maximum slope angle that player can descend
    public float jumpVelocity = 16.0f; //Vertical movement speed when jumping
    public float reboundVelocity = 20.0f; //(*used to be reboundHeight)
    public float dashSpeed = 25.0f; //Dash movement speed
    public float diveSpeed = -17.5f; //Dive descent speed
    public float momentumFactor = 1.8f; //Speed multiplier for sprinting
    public float knockbackSpeed = 8.0f; //Speed at which player is knocked back when colliding with enemy
    public float wallSlideSpeed = -3.0f; //Speed at which player descends while wall sliding
    public Vector2 wallClimbVel = new Vector2(1.5f, 8.0f); //(*used to be wallClimb) Speed when performing wall climb
    public Vector2 wallJumpVel = new Vector2(12.0f, 10.0f); //(*used to be wallJump) Speed when performing wall jump

    //Timer Values
    public float jumpBufferTime = 0.2f; //(*new) Time before wantToJump buffer expires
    public float jumpTime = 0.4f; //Time before player can perform a second jump
    public float attackTime = 0.75f; //Time before player regains control during attack
    public float dashAttackTime = 0.5f; //(*new / needs change) Time before player regains control during dash attack
    public float displacementTime = 0.05f; //(*new) Delay time before transitioning from grounded state to airborne
    public float pivotTime = 0.5f; //(*new / needs change) Time before player regains horizontal movement control
    public float dashCooldownTime = 4.0f; //(*new) Time before player can use dash again
    public float dashTime = 0.5f; // Time before player regains control while performing a dash
    public float diveAttackTime = 0.4f; // Time before player performs a rebound after a dive attack
    public float momentumTime = 2.5f; //(*new) Duration that the player must move in the same direction to begin sprinting
    public float wallStickCooldown = 0.3f; //(*new) Time between when player detaches from wall and can attach to a wall again
    public float wallClimbCooldown = 1.0f; //(*new) Time between when player can perform another wall climb
    public float wallClimbTime = 0.3f; // Time before player regains control after performing a wall climb
    public float wallJumpCooldown = 1.0f; //(*new) Time between when player can perform another wall jump
    public float wallJumpTime = 0.15f; //Time before player regains control after performing a wall jump
    public float landingTime = 0.6f; //Time before player regains control after landing from a dive
    public float stunTime = 2.0f; //(needs change) Time before player regains control when colliding with enemy
    public float immuneTime = 4.0f; //(*new / needs change) Time before player can be affected by further enemy collisions and collision damage
    public float deathTime = 3.0f; //Time before player respawns after death

    //Physics
    public LayerMask collisionMask; //Collision mask for dash
    public LayerMask diveCollisionMask; //Collision mask for dive
    public LayerMask slopeCollisionMask; //Collision mask for slopes
    public float rbDrag = 0.5f;
    public float rbMass = 1.5f;
    public float gravity = 6.0f;
}
