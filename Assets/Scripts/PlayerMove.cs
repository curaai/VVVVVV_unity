using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static readonly float MaxSpeedX = 6f;
    public static readonly float MaxSpeedY = 10f;
    public static readonly Vector2 SPEED = new Vector2(3f, 1);
    public static readonly Vector2 INERTIA = new Vector2(1.1f, 0.25f);

    private float x { get { return transform.position.x; } set { transform.position = new Vector3(value, transform.position.y, transform.position.z); } }
    private float y { get { return transform.position.y; } set { transform.position = new Vector3(transform.position.x, value, transform.position.z); } }

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

        var hInput = Input.GetAxis("Horizontal");
        force.x = 0;
        if (hInput != 0) force.x = hInput < 0 ? -SPEED.x : SPEED.x;

        TapMove();

        velocity += force;
        velocity = applyFriction(velocity, INERTIA);

        // TODO: Check Collision
        x += velocity.x;
        y += velocity.y;
    }

    static Vector2 applyFriction(Vector2 v, Vector2 inertia)
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
}