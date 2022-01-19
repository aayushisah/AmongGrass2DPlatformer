using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //physics things 
    private Rigidbody2D rb;
    private Animator anim; 
    private CircleCollider2D coll;
    private Collider2D col;
    private enum State{idle,run,jump,falling,hurt};
    private State state = State.idle;
    private float startTime =210;
    private float fallMultiplier = 5f;
    
    
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int pineapple = 0;
    [SerializeField] private Text pText ;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private int health =100;
    [SerializeField] private Text healthAmt;
    [SerializeField] private Text timerText;
    [SerializeField] private float rotationSpeed;

    
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>();   
        coll = GetComponent<CircleCollider2D>();
        healthAmt.text = health.ToString();

        FindObjectOfType<AudioManager>().Play("mainmenu");
        FindObjectOfType<AudioManager>().Play("powerup");           
    }


    void Update()
    {   
        if(state != State.hurt)
        Movement();
        
        float t = startTime - Time.deltaTime;
        startTime -= Time.deltaTime;
        string seconds = t.ToString("f0");
        timerText.text = seconds;

        if (t<=10)
        {
            timerText.color = Color.red;
            TimeUp();
        }
        if (t<=0)
        {
            anim.SetTrigger("Death");
            GameOver();
        }
        
        if(gameObject.transform.position.y<-15)
        {
            anim.SetTrigger("Death");
            GetComponent <SpriteRenderer>().color = Color.magenta;
            if(gameObject.transform.position.y<-30)
            GameOver();           
        }      

        VelocityState();
        anim.SetInteger("State", (int)state);    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("collectible"))//faster than collision.tag
        {
            Destroy(collision.gameObject);
            pineapple += 1;
            FindObjectOfType<AudioManager>().Play("yum");
            pText.text = pineapple.ToString();
        }
        if (collision.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("powerup");
            jumpForce =27f;
            
            GetComponent <SpriteRenderer>().color =  Color.magenta;
            StartCoroutine(ResetPower());
            
        }
        if (collision.CompareTag("end"))
        {
            state = State.idle;
            FinishedGame();
        }
        if( collision.CompareTag("end2"))
        {
            
            if (5 > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt", 5);
            }
            state = State.idle;
            LoadL2();
            Start();
        }
        
    }
   
       
              
            

    private void OnCollisionEnter2D(Collision2D other)
    {
         
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Trap"))
        {
            if(state == State.falling && other.gameObject.CompareTag("Enemy" ))
            {
                NinjaFrog ninjaFrog = other.gameObject.GetComponent<NinjaFrog>();
                ninjaFrog?.jumpedOn();
                //frogtrouble frog = other.gameObject.GetComponent<frogtrouble>();
                //ninjaFrog?.Death();
                //Destroy(other.gameObject);
                Damagable ninjafrog = other.gameObject.GetComponent<Damagable>();
                ninjafrog.OnDeath();

                Damagable frog2 = other.gameObject.GetComponent<Damagable>();
                frog2.OnDeath();

                Jump();
                FindObjectOfType<AudioManager>().Play("enemypop");
                FindObjectOfType<AudioManager>().Play("sjump");
                
            }
        
            else
            {
                
                state = State.hurt;
                FindObjectOfType<AudioManager>().Play("hit");
                HandleHealth();
                
                
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    state = State.idle;
                }

                else 
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    state = State.idle;
                    
                }
            }    
        }
    }

   
    private void HandleHealth()
    {
        health -= 10;
        healthAmt.text = health.ToString();
        if(health <= 50)
        {
            healthAmt.color = Color.magenta;
        }
        if(health <= 0)
        {
            healthAmt.color = Color.red;
            healthAmt.text = "0";
            SceneManager.LoadScene(2);
            FindObjectOfType<AudioManager>().Play("ono");
        }
    }


    private void VelocityState()
    {
        if(state == State.jump )
        {
            //if (rb.velocity.y < 0.1f)
            if (rb.velocity.y <2f)//latest
            {
                state = State.falling;               
            }
        }
        else if (state == State.falling)
        {
            
            if(rb.velocity.y<0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1)*Time.deltaTime;                            
            }
            if(coll.IsTouchingLayers(Ground))
            {
                state = State.idle;
            }
        }
        
        else if(Mathf.Abs(rb.velocity.x)>2f){
            state = State.run;  
        }
        else{
            state = State.idle;
        }
    }


    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        if (coll.IsTouchingLayers(Ground))
        {
            transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);
            if (movementDirection != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

        }


        float hDirection = Input.GetAxis("Horizontal");
        if ( hDirection > 0 )
        {
            rb.velocity = new Vector2(5,rb.velocity.y);
            transform.localScale = new Vector2(1,1);
        }
        else if ( hDirection < 0)
        {
            rb.velocity = new Vector2(-5,rb.velocity.y);
            transform.localScale = new Vector2(-1,1);
        }

        
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
            FindObjectOfType<AudioManager>().Play("sJump");       
        }
        else if ((Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) )) )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
            FindObjectOfType<AudioManager>().Play("sJump");             
        }
    }


    private void Jump()
    {
        //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        rb.velocity = new Vector2(1, jumpForce);//latest
        state = State.jump;
        FindObjectOfType<AudioManager>().Play("sJump");
    }


    public void PauseGame()
    {
        
        if(Time.timeScale==1)
        {
            Time.timeScale = 0;
            FindObjectOfType<AudioManager>().Pause("bgmusic");
        }
        else
        {
            Time.timeScale = 1;
            FindObjectOfType<AudioManager>().Play("bgmusic");   
        }
    }


    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(15);
        jumpForce =15f;
        GetComponent <SpriteRenderer>().color = Color.white;
    }

    public void LoadL2()
    {
        
        timerText.color = Color.green;
        timerText.text = "YAY";
        SceneManager.LoadScene(5);
        FindObjectOfType<AudioManager>().Play("yay");
        Start();
    }

    public void FinishedGame()
    {   
        timerText.color = Color.green;
        timerText.text = "YAY";
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("yay");
    }


    private IEnumerator DelayFn()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }


    public void GameOver()
    {
        timerText.color = Color.red;
        timerText.text = "OVER";
        StartCoroutine(DelayFn());
    }


    public void MainMenu()
    {        
        SceneManager.LoadScene(3);
        FindObjectOfType<AudioManager>().Play("mainmenu");
    }
    

    private void TimeUp()
    {
        FindObjectOfType<AudioManager>().Play("timeup"); 
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}