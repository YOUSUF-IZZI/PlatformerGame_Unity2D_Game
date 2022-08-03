using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Movements : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float jump = 5;


    //canvas  ------------------------------------------------------
    [SerializeField]
    Transform holder;
    TextMeshProUGUI healthText;
    TextMeshProUGUI scoreText;

    [SerializeField]
    GameObject panel_finish;

    [SerializeField]
    GameObject gameOver;


    //sounds -------------------------------------------------------
    [SerializeField]
    Transform sound_collectSound;

    [SerializeField]
    Transform sound_playerDeath;

    [SerializeField]
    Transform sound_Jumping;

    [SerializeField]
    Transform sound_heartCollecting;

    [SerializeField]
    Transform sound_PlayerDamage;

    bool isJump=false;
    int playerHealth ;
    int score ;

    Transform trans;
    SpriteRenderer sr;
    Rigidbody2D rb;
    private Animator anim;



    void Start()
    {
        //we use this way to save the time fo invoking the method "GetComponent"
        sr = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // The Two component in the holder in the canvas
        healthText = holder.Find("Text_Health").GetComponent<TextMeshProUGUI>();
        scoreText = holder.Find("Text_Score").GetComponent<TextMeshProUGUI>();

        //we should give the initial value for some objects at the start of the game because there is ablility to replay the game again
        score = 0;
        playerHealth = 3;

        healthText.text = playerHealth + "/" + "5";
        scoreText.text = "Score: " + score ;
    }




    void Update()
    {
        // Moving Left  ----------------------------------------------------------------------------------------------

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            trans.position=trans.position - new Vector3(speed, 0, 0) *Time.deltaTime;
            sr.flipX = true;
        }

        // Moving Right ----------------------------------------------------------------------------------------------
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // its the same job above but with a different way
            //transform.Translate(new Vector2(speed, 0));
            trans.position = trans.position + new Vector3(speed, 0, 0) * Time.deltaTime;
            sr.flipX = false;
        }

        // Running Animation
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        // Jump function  ---------------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.Space) & !isJump)
        {
            isJump = true;
            rb.velocity = new Vector2(0, jump);
            soundOn3(transform.position);
        }
        // Fixing multijump problem  ----------------------------------------------------------------------------------
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            isJump = false;
        }

        
        // Jumping and falling animations  ---------------------------------------------------------------------------

        if (rb.velocity.y <0.01)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (rb.velocity.y > 0.01)
            anim.SetBool("isJumping", true);
        if(rb.velocity.y<0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }


        // Game Over 
        if (playerHealth<=0)
        {
            soundOn2(transform.position);
            gameOver.SetActive(true);
        }
    }




    // Collision by other objects  ----------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //There are Two types of collesion one of them reduce player health and other one kill the Enemy
        if (collision.CompareTag("Enemy"))
        {
            if (isJump && rb.velocity.y<0 )
            {
                Destroy(collision.gameObject);
                //anim.SetTrigger("EnemyDeathAnimation");
            }
            else
            {
                playerHealth--;
                healthText.text= playerHealth + "/" + "5";
            }
        }
        // Gem
        if (collision.CompareTag("Gem"))
        {
            score = score + 100;
            soundOn(collision.transform.position);
            Destroy(collision.gameObject);
            scoreText.text = "Score: " + score;
        }

        // Cherry
        if (collision.CompareTag("Cherry"))
        {
            score = score + 50;
            soundOn(collision.transform.position);
            Destroy(collision.gameObject);
            scoreText.text = "Score: " + score;
        }
        // Heart
        if (collision.CompareTag("Heart"))
        {
            if (playerHealth < 5)
            {
                playerHealth++;
                healthText.text = playerHealth + "/" + "5";
                Destroy(collision.gameObject);
                soundOn4(collision.transform.position);
            }      
        }

        // Star 
        if (collision.CompareTag("Finish"))
        {
            panel_finish.SetActive(true);
            soundOn(collision.transform.position);
        }

        // Game Over
        if (collision.CompareTag("GameOver"))
        {
            gameOver.SetActive(true);
            soundOn2(transform.position);
        }


    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isHurt", false);

        //collision.gameObject mean collision with the phisics of object
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isJump && rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(0, 9);
                Destroy(collision.gameObject);
                soundOn3(transform.position);
            }
            else
            {
                anim.SetBool("isHurt", false);
                playerHealth--;
                healthText.text = playerHealth + "/" + "5";
                anim.SetBool("isHurt", true);
                soundOn5(collision.transform.position);
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            playerHealth--;
            healthText.text = playerHealth + "/" + "5";
            anim.SetBool("isHurt", true);
            soundOn5(collision.transform.position);
        }
        
    }

  

    // sound ---------------------------------------------------------------------------
    void soundOn(Vector3 itemPos)
    {
        Transform obj = Instantiate(sound_collectSound, itemPos,new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }


    void soundOn2(Vector3 itemPos)
    {
        Transform obj = Instantiate(sound_playerDeath, itemPos, new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }


    void soundOn3(Vector3 itemPos)
    {
        Transform obj = Instantiate(sound_Jumping, itemPos, new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }

    void soundOn4(Vector3 itemPos)
    {
        Transform obj = Instantiate(sound_heartCollecting, itemPos, new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }

    void soundOn5(Vector3 itemPos)
    {
        Transform obj = Instantiate(sound_PlayerDamage, itemPos, new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }
}
