using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static readonly float MaxSpeedX = 6f;
    public static readonly float MaxSpeedY = 10f;
    public static readonly Vector2 SPEED = new Vector2(3f, 3f);
    public static readonly Vector2 INERTIA = new Vector2(1.1f, 0.25f);

    private float x { get { return transform.position.x; } set { transform.position = new Vector3(value, transform.position.y, transform.position.z); } }
    private float y { get { return transform.position.y; } set { transform.position = new Vector3(transform.position.x, value, transform.position.z); } }

    [ReadOnly] public bool JumpNow;
    [ReadOnly] public bool OnWall;
    [ReadOnly] public bool GravityOrientation;
    [ReadOnly] public bool HorizontalDirection;

    private Vector2 force; // ax,ay
    private Vector2 velocity; // vx,vy

    private int tapLeft = 0;
    private int tapRight = 0;

    void Start()
    {
        // TODO: Remove when implement map
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        Vector2 applyFriction(Vector2 v, Vector2 inertia)
        {
            float signX = 0 < v.x ? 1 : -1;
            float signY = 0 < v.y ? 1 : -1;

            var x = Mathf.Abs(v.x);
            var y = Mathf.Abs(v.y);

            x -= inertia.x;
            y -= inertia.y;
            x = Mathf.Min(x, MaxSpeedX);
            y = Mathf.Min(y, MaxSpeedY);

            if (x < inertia.x) x = 0;
            if (y < inertia.y) y = 0;

            return new Vector2(x * signX, y * signY);
        }

        void TapMove()
        {
            if (force.x < 0) tapLeft++;
            else
            {
                if (0 < tapLeft && velocity.x < 0) velocity.x = 0;
                tapLeft = 0;
            }

            if (0 < force.x) tapRight++;
            else
            {
                if (0 < tapRight && 0 < velocity.x) velocity.x = 0;
                tapRight = 0;
            }
        }

        force.x = 0;
        var hInput = Input.GetAxis("Horizontal");
        if (hInput != 0)
        {
            HorizontalDirection = hInput < 0;
            force.x = HorizontalDirection ? -SPEED.x : SPEED.x;
        }

        var vInput = Input.GetAxis("Vertical");
        if (vInput != 0)
        {
            if (OnWall)
            {
                GravityOrientation = !GravityOrientation;
                velocity.y = GravityOrientation ? 4 : -4; // TODO: Replace to constant
                force.y = GravityOrientation ? SPEED.y : -SPEED.y;
                JumpNow = true;
            }
        }
        if (OnWall && !JumpNow)
        {
            force.y = 0;
            velocity.y = 0;
        }

        TapMove();

        velocity += force;
        velocity = applyFriction(velocity, INERTIA);

        // TODO: Check Collision
        x += velocity.x;
        y += velocity.y;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var layer = collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("wall"))
        {
            Debug.Log("Collide to wall");
            OnWall = true;
            JumpNow = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        var layer = collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("wall"))
        {
            OnWall = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // TODO: Adjust velocity before collide (overlap animation bug)
        var overlapDist = GetComponent<BoxCollider2D>().Distance(collider);
        var diff = overlapDist.pointA.y - overlapDist.pointB.y;
        diff += diff < 0 ? 0.1f : -0.1f;

        y -= diff;
    }
}