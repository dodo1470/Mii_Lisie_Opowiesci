using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float BasicMoveSpeed = 1.0f;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed;
    [Range(0.01f, 20.0f)][SerializeField] private float runningMultiplier = 1.5f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private AudioClip track;
    [SerializeField] private AudioClip dzwiekSkoku;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip deathsound;
    [SerializeField] private AudioClip killsound;
    [SerializeField] private AudioClip zycie;
    [SerializeField] private AudioClip klucze;
    [SerializeField] private AudioClip przegrana;
    [SerializeField] private AudioClip wygrana;
    [SerializeField] private AudioClip owip1;
    [SerializeField] private AudioClip owip2;
    [SerializeField] private AudioClip owip3;

    [Space(10)]
    private AudioSource source;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isrunning = false;
    private bool isfacingright = true;
    [SerializeField] private LayerMask groundLayer;
    const float rayLength = 0.2f;
    Vector2 startPosition;
    private bool hold = false;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        source = GetComponent<AudioSource>();
        source.clip = track;
        source.volume = 0.01f;
        source.Play();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    bool IsGrounded()
    {
        Vector2 vleft = this.transform.position;
        Vector2 vright = this.transform.position;
        vleft.x -= 0.055f;
        vright.x += 0.055f;
        return (Physics2D.Raycast(vleft, Vector2.down, rayLength, groundLayer.value) || Physics2D.Raycast(vright, Vector2.down, rayLength, groundLayer.value));
    }

    void Jump()
    {

        if (IsGrounded() && hold == false)
        {
            if (moveSpeed == 1) { 
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            } else
            {
                rigidBody.AddForce(Vector2.up * jumpForce * 1.2f, ForceMode2D.Impulse);
            }
                UnityEngine.Debug.Log("jumping");
            source.PlayOneShot(dzwiekSkoku, AudioListener.volume);
            hold = true;
        }
        hold = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance == null) return;
        if (GameManager.instance.currentGameState == GameState.GAME)
        {
            isrunning = false;
            moveSpeed = BasicMoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed *= runningMultiplier;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isrunning = true;
                if (isfacingright == false)
                {
                    Flip();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isrunning = true;
                if (isfacingright)
                {
                    Flip();
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) && hold == false)
            {
                Jump();
                hold = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow) != true)
            {
                hold = false;
            }
            UnityEngine.Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 0.2f, false);
            animator.SetBool("IsGrounded", IsGrounded());
            animator.SetBool("IsRunning", isrunning);

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("LevelExit"))
        {
            if (GameManager.instance.keysCompleted)
            {
                GameManager.instance.AddPoints(100 * GameManager.instance.lives);
                GameManager.instance.LevelCompleted();
                UnityEngine.Debug.Log("You Won");
            }
            else
            {
                UnityEngine.Debug.Log("You need all the keys");
            }
        }
        else
        if (col.CompareTag("LevelFall"))
        {
            UnityEngine.Debug.Log("fall over");
            GameManager.instance.AddLives(-1);
            transform.position = startPosition;
        }
        else
        if (col.CompareTag("Bonus"))
        {
            //UnityEngine.Debug.Log("Bonus");
            GameManager.instance.AddPoints(1);
            col.gameObject.SetActive(false);
            source.PlayOneShot(pickup, AudioListener.volume);
        }
        else
        if (col.CompareTag("Enemy"))
        {
            if (transform.position.y > col.gameObject.transform.position.y)
            {
                UnityEngine.Debug.Log("Killed an enemy");
                GameManager.instance.AddKills(1);
                source.PlayOneShot(killsound, AudioListener.volume);
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                transform.position = startPosition;
                UnityEngine.Debug.Log("You died");
                GameManager.instance.AddLives(-1);
                source.PlayOneShot(deathsound, AudioListener.volume);
            }
        }
        else
        if (col.CompareTag("KR"))
        {
            GameManager.instance.AddKeys(0);
            col.gameObject.SetActive(false);
            source.PlayOneShot(klucze, AudioListener.volume);
        }
        else
        if (col.CompareTag("KG"))
        {
            GameManager.instance.AddKeys(1);
            col.gameObject.SetActive(false);
            source.PlayOneShot(klucze, AudioListener.volume);
        }
        else
        if (col.CompareTag("KB"))
        {
            GameManager.instance.AddKeys(2);
            col.gameObject.SetActive(false);
            source.PlayOneShot(klucze, AudioListener.volume);
        }
        else
        if (col.CompareTag("Heart"))
        {
            GameManager.instance.AddLives(1);
            col.gameObject.SetActive(false);
            UnityEngine.Debug.Log("You gained 1 life");
            source.PlayOneShot(zycie, AudioListener.volume);
        }
        else
        if (col.CompareTag("OP"))
        {
            GameManager.instance.AddPoints(10);
            col.gameObject.SetActive(false);
            source.PlayOneShot(owip1, 30.0f);
        }
        else
        if (col.CompareTag("SP"))
        {
            GameManager.instance.AddPoints(10);
            col.gameObject.SetActive(false);
            source.PlayOneShot(owip2, 30.0f);
        }
        else
        if (col.CompareTag("DP"))
        {
            GameManager.instance.AddPoints(10);
            col.gameObject.SetActive(false);
            source.PlayOneShot(owip3, 30.0f);
        }

        if (col.CompareTag("MovingPlatform"))
        {
            transform.SetParent(col.transform);
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        transform.SetParent(null);
    }

    private void Flip()
    {
        isfacingright = !isfacingright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
    void winsound()
    {
        source.PlayOneShot(wygrana, AudioListener.volume);
    }
    void losesound()
    {
        source.PlayOneShot(przegrana, AudioListener.volume);
    }

}
