using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitty : MonoBehaviour
{
    //references
    public Score score;
    public GameManager gameManager;
    public Sprite kittyDied;
    public ColumnSpawner columnSpawner;
    public Animator kittyParentAnim;
    public Animator getReadyAnim;
    public Animator hitEffect;
    public Animator cameraAnim;

    SpriteRenderer sp;
    Animator anim;
    Rigidbody2D rb;
    public float speed;


    int angle;
    int maxAngle = 20;
    int minAngle = -90;

    bool touchedGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            GameManager.gameOver == false &&
            GameManager.gameIsPaused == false)
        {
            if (GameManager.gameHasStarted == false)
            {
                rb.gravityScale = 0.8f;
                kittyParentAnim.enabled = false;
                Flap();
                //column Spawner
                //set the trigger for the get ready anim
                getReadyAnim.SetTrigger("fadeOut");
            }
            else
            {
                Flap();
            }


        }

        KittyRotation();


    }

    public void OnGetReadyAnimFinished()
    {
        columnSpawner.InstantiateColumn();
        gameManager.GameHasStarted();
    }

    void Flap()
    {
        AudioManager.audiomanager.Play("flap");
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    void KittyRotation()
    {
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = 0.8f;

            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = 0.6f;

            if (rb.velocity.y < -1.3f)
            {
                if (angle >= minAngle)
                {
                    angle = angle - 3;
                }
            }

        }

        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.gameOver == false)
        {
            if (collision.CompareTag("Column"))
            {
                //print("We have scored");
                AudioManager.audiomanager.Play("point");
                score.Scored();
            }
            else if (collision.CompareTag("Pipe"))
            {
                KittyDieEffect();
                //game over
                gameManager.GameOver();
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (GameManager.gameOver == false)
            {
                KittyDieEffect();
                //game over
                gameManager.GameOver();
                GameOver();

            }
            else
            {

                GameOver();
            }

        }
    }

    void KittyDieEffect()
    {
        AudioManager.audiomanager.Play("hit");
        hitEffect.SetTrigger("hit");
        cameraAnim.SetTrigger("shake");
    }

    void GameOver()
    {
        touchedGround = true;
        anim.enabled = false;
        sp.sprite = kittyDied;
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }


}