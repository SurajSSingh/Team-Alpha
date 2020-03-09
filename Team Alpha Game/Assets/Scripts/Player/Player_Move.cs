using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Attributes attributes;

    //Movement Stats
    private float moveSpeed;
    private float maxDescendAngle;
    private float momentumFactor;

    //Timer Values
    private float pivotTime;

    //Physics
    private LayerMask slopeCollisionMask;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        attributes = state.attributes;
        moveSpeed = attributes.moveSpeed;
        pivotTime = attributes.pivotTime;
        maxDescendAngle = attributes.maxDescendAngle;
        momentumFactor = attributes.momentumFactor;
        slopeCollisionMask = attributes.slopeCollisionMask;
    }

    public void Sprint(ref Vector3 velocity, float sign, float inputX, bool onSlope)
    {
        velocity.x = sign * inputX * moveSpeed * momentumFactor;
        if (onSlope)
        {
            velocity.y = velocity.y * momentumFactor;
        }
    }

    public void Pivot(ref Vector3 velocity, float pivotSign, float pivotTimer) //Quickly come to a stop when moving in a direction
    {
        float multiplier = pivotTimer;
        if (pivotTimer <= 0.0f)
        {
            multiplier = 0.01f;
        }
        velocity.x = pivotSign * velocity.x / 1.2f + (0.2f * multiplier); //velocity grows slower as pivotTimer counts down to 0.0f
    }

    public void DescendSlope(ref Vector3 velocity) //Changes player velocity to match the angle of the slope beneath
    {
        float dirX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 botLeft = new Vector2(bounds.min.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 botRight = new Vector2(bounds.max.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 rayOrigin = (dirX == -1) ? botRight : botLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, slopeCollisionMask);
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == dirX)
                {
                    if (hit.distance - 0.15f <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {

                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * dirX;
                        velocity.y -= descendVelocityY - 0.15f;
                    }
                }
            }
        }
    }
}
