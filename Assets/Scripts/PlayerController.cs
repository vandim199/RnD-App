using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float raycastLength;
    [SerializeField]
    private LayerMask layerIgnore;
    private bool facingRight = true;
    private bool grounded;
    private Rigidbody2D rb;
    private LineRenderer lineRend;
    private Vector2 dragStartPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRend = GetComponent<LineRenderer>();

        transform.localScale = MapGeneration.MapSingleton.grid.cellSize;
    }

    void Update()
    {
        //PC Controls
        if(Input.GetMouseButtonDown(0) && grounded)
        {
            dragStartPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0) && grounded)
        {
            lineRend.enabled = true;

            Vector2 dragEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 velocity = (dragStartPos - dragEndPos) * jumpForce;

            Vector2[] trajectory = AimLine(rb, gameObject.transform.position, velocity, 500);
            lineRend.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }

            lineRend.SetPositions(positions);
        }

        if(Input.GetMouseButtonUp(0) && grounded)
        {
            Vector2 dragEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 velocity = (dragStartPos - dragEndPos) * jumpForce;

            rb.velocity = velocity;
            lineRend.enabled = false;
        }


        //Mobile Controls
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began && grounded)
            {
                dragStartPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if(touch.phase == TouchPhase.Moved && grounded)
            {
                lineRend.enabled = true;

                Vector2 dragEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector2 velocity = (dragStartPos - dragEndPos) * jumpForce;

                Vector2[] trajectory = AimLine(rb, gameObject.transform.position, velocity, 500);
                lineRend.positionCount = trajectory.Length;

                Vector3[] positions = new Vector3[trajectory.Length];
                for (int i = 0; i < trajectory.Length; i++)
                {
                    positions[i] = trajectory[i];
                }

                lineRend.SetPositions(positions);
            }

            if(touch.phase == TouchPhase.Ended && grounded)
            {
                Vector2 dragEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector2 velocity = (dragStartPos - dragEndPos) * jumpForce;

                rb.velocity = velocity;
                lineRend.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = RayTest(-Vector2.up, transform.position) || 
            RayTest(-Vector2.up, transform.position + new Vector3(0.5f, 0, 0)) || 
            RayTest(-Vector2.up, transform.position - new Vector3(0.5f, 0, 0));

    }

    private bool RayTest(Vector2 direction, Vector3 startPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, raycastLength, layerIgnore);

        if (hit.collider != null)
        {
            Debug.DrawRay(startPos, direction * raycastLength);
            float distance = Vector2.Distance(startPos, hit.point);

            if (distance <= raycastLength)
            {
                return true;
            }
        }
        return false;
    }

    public void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        
        facingRight = !facingRight;
    }

    public Vector2[] AimLine(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] result = new Vector2[steps];

        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;

        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timeStep * timeStep;

        float drag = 1f - timeStep * rigidbody.drag;
        Vector2 moveStep = velocity * timeStep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;

            result[i] = pos;
        }

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            GameManager.GameManagerSingleton.AddScore(500);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            Destroy(gameObject);
            GameManager.GameManagerSingleton.EndGame();
        }
    }
}
