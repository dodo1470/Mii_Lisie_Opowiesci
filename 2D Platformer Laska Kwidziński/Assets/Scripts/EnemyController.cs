using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool isfacingright = true;
    public float startpostionx;
    private float moverange = 1.0f;
    private bool ismovingright = false;



    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startpostionx = transform.position.x;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ismovingright)
        {
            if(this.transform.position.x < startpostionx + moverange)
            {
                MoveRight();
            }
            else
            {
                Flip();
                ismovingright = false;
            }
        }
        else
        {
            if(this.transform.position.x > startpostionx - moverange)
            {
                MoveLeft();
            }
            else
            {
                Flip();
                ismovingright = true;
            }
        }
    }
    private void Flip()
    {
        isfacingright = !isfacingright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (transform.position.y < col.gameObject.transform.position.y)
            {
                animator.SetBool("IsDead", true);
                StartCoroutine( KillOnAnimationEnd() );
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
