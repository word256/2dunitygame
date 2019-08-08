using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed;
    public float jumpForce;
    public float maxWalkingSpeed;

    public float gravity;
    public float friction;
    public float skinWidth;
    public float groundCheckDistance;

    public int horizontalRayCount;
    public int verticalRayCount;

    private bool walking = true;
    private bool grounded = false;

    private Vector2 input;
    private Vector2 velocity;

    private BoxCollider2D collider;
    private RaycastCorners raycastCorners;

    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    public bool isGrounded()
    {
        return grounded;
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Physics();
        Move();
    }

    private void Physics()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity.x += input.x * speed * Time.deltaTime;

        if (velocity.x > 0)
        {
            velocity.x -= friction * Time.deltaTime;
            if (velocity.x < 0)
                velocity.x = 0;
        }

        if (velocity.x < 0)
        {
            velocity.x += friction * Time.deltaTime;
            if (velocity.x > 0)
                velocity.x = 0;
        }

        if (walking)
        {
            if (velocity.x > maxWalkingSpeed)
            {
                velocity.x = maxWalkingSpeed;
            }

            if (velocity.x < -maxWalkingSpeed)
            {
                velocity.x = -maxWalkingSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            velocity.y = jumpForce;
        }
    }

    private void Move()
    {
        // Returns distance between each ray for the horizontal and vertical rays whilst also updating the raycast corners.
        Vector2 spacings = CalculateRaycastCorners();

        Vector2 movementVector = new Vector2(CheckHorizontalCollisions(spacings.x), CheckVerticalCollisions(spacings.y));
        transform.Translate(movementVector);
    }

    private float CheckHorizontalCollisions(float raySpacing)
    {
        if (velocity.x == 0)
            return 0.0f;

        Vector2 rayCorner = Vector2.zero;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        float hitDist = rayLength;

        if (velocity.x > 0.0f)
            rayCorner = raycastCorners.bottomRight;
        if (velocity.x < 0.0f)
            rayCorner = raycastCorners.bottomLeft;

        for (int i = 0; i < horizontalRayCount; i ++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCorner + (Vector2.up * raySpacing * i), Vector2.right * Mathf.Sign(velocity.x), rayLength, collisionMask);
            if (hit)
            {
                rayLength = hit.distance;
                hitDist = hit.distance;
            }
        }

        if (hitDist > 0)
            return hitDist * Mathf.Sign(velocity.x) - skinWidth;
        else
            return 0.0f;
    }

    private float CheckVerticalCollisions(float raySpacing)
    {
        Vector2 rayCorner = raycastCorners.bottomLeft;
        float rayLength = Mathf.Abs(velocity.y + skinWidth);
        float hitDist = rayLength;
        int verticalDirection = (int)Mathf.Sign(velocity.y);
        
        if (verticalDirection != 1)
            verticalDirection = -1;

        grounded = false;
        
        if (velocity.y > 0)
            rayCorner = raycastCorners.topLeft;

        for (int i = 0; i < verticalRayCount; i ++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCorner + (Vector2.right * raySpacing * i), Vector2.up * verticalDirection, rayLength, collisionMask);
            
            if (hit)
            {
                rayLength = hit.distance;
                hitDist = hit.distance;

                if (verticalDirection == -1)
                {
                    grounded = true;
                    velocity.y = 0;
                }
            }
            else
            {
                RaycastHit2D groundHit = Physics2D.Raycast(rayCorner + (Vector2.right * raySpacing * i), Vector2.down, skinWidth, collisionMask);
                if (groundHit)
                {
                    grounded = true;
                    velocity.y = 0;

                    return -groundHit.distance + skinWidth;
                }
            }
        }
        
        return hitDist * verticalDirection - (skinWidth * verticalDirection);
    }

    private Vector2 CalculateRaycastCorners()
    {
        Bounds bounds = collider.bounds;

        raycastCorners.topLeft = new Vector2(bounds.min.x + skinWidth, bounds.max.y - skinWidth);
        raycastCorners.topRight = new Vector2(bounds.max.x - skinWidth, bounds.max.y - skinWidth);
        raycastCorners.bottomLeft = new Vector2(bounds.min.x + skinWidth, bounds.min.y + skinWidth);
        raycastCorners.bottomRight = new Vector2(bounds.max.x - skinWidth, bounds.min.y + skinWidth);

        return new Vector2((raycastCorners.topRight.y - raycastCorners.bottomRight.y) / (horizontalRayCount - 1),
        (raycastCorners.bottomRight.x - raycastCorners.bottomLeft.x) / (verticalRayCount - 1));
    }

    struct RaycastCorners
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }
}