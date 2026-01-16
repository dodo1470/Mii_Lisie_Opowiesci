using System.Collections;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool isfacingright = true;
    public float startpostionx;
    private float moverange = 0.3f;
    private bool ismovingright = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startpostionx = transform.position.x;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ismovingright)
        {
            if (this.transform.position.x < startpostionx + moverange)
            {
                MoveRight();
            }
            else
            {
                ismovingright = false;
            }
        }
        else
        {
            if (this.transform.position.x > startpostionx - moverange)
            {
                MoveLeft();
            }
            else
            {
                ismovingright = true;
            }
        }
    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
}
