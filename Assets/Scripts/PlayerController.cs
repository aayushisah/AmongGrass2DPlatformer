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
    private float startTime = 180;
    private float fallMultiplier = 5f;
    
    //serializables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int pineapple = 0;
    [SerializeField] private int health =100;
    [SerializeField] private Text pText ;
    [SerializeField] private Text healthAmt;
    [SerializeField] private Text timerText;
    

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>();   
        coll = GetComponent<CircleCollider2D>();

        if (healthAmt!= null)
            healthAmt.text = health.ToString();

        FindObjectOfType<AudioManager>().Play("powerup");           
    }


    void Update()
    {   

        if(state != State.hurt)//player moves when not hurt
        {
            Movement();
        }
        if (SceneManager.GetActiveScene().name == "L1" || SceneManager.GetActiveScene().name == "L2")
        TimeUI();//time UI 
        
        if(gameObject.transform.position.y<-15)//player dies on falling off the platform 
        {
            anim.SetTrigger("Death");
            GetComponent <SpriteRenderer>().color = Color.magenta;
            if(gameObject.transform.position.y<-30)
            GameOver();           
        }      

        VelocityState();
        anim.SetInteger("State", (int)state);    

    }


    private void OnTriggerEnter2D(Collider2D collision)//handle collectibles/end tags
    {
        if(collision.CompareTag("collectible"))//faster than collision.tag
        {
            Destroy(collision.gameObject);
            pineapple += 1;
            FindObjectOfType<AudioManager>().Play("yum");
            pText.text = pineapple.ToString();
        }
        if(collision.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("powerup");
            jumpForce =27f;
            
            GetComponent <SpriteRenderer>().color =  Color.magenta;
            StartCoroutine(ResetPower());   
        }
        if(collision.CompareTag("end"))
        {
            PlayerPrefs.Save(); 
            state = State.idle;
            FinishedGame();
        }
        if(collision.CompareTag("end2"))
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
   
    private void OnCollisionEnter2D(Collision2D other)//handle enemy
    {
         
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Trap"))
        {
            if(state == State.falling && other.gameObject.CompareTag("Enemy"))
            {
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

   
    private void HandleHealth()//handle health UI 
    {
        health -= 10;
        healthAmt.text = health.ToString();
        if(health <= 50)
            healthAmt.color = Color.magenta;
        if(health <= 0)
        {
            healthAmt.color = Color.red;
            healthAmt.text = "0";
            SceneManager.LoadScene(2);
            FindObjectOfType<AudioManager>().Play("ono");
        }
    }


    private void VelocityState()//for the FSM 
    {
        if(state == State.jump )
        {
            if (rb.velocity.y <2f)
            {
                state = State.falling;               
            }
        }
        else if (state == State.falling)
        {
            
            if(rb.velocity.y<0)
            {
                //for better Mario-like jump 
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


    private void Movement()//simulate player mechanics
    {
        //input 
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        //enables player to move on angled floors
        if (coll.IsTouchingLayers(Ground))
        {
            transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);
            if (movementDirection != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

        }

        //enables player to change the direction they are facing
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

        //jump mechanics
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
            FindObjectOfType<AudioManager>().Play("sJump");       
        }
        else if ((Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground)  && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) )) )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
            FindObjectOfType<AudioManager>().Play("sJump");             
        }
    }

    private void Jump()//extra jump on destroying the enemy 
    {
        //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        rb.velocity = new Vector2(1, jumpForce);//latest
        state = State.jump;
        FindObjectOfType<AudioManager>().Play("sJump");
    }

    private void TimeUI()//handle time UI 
    {
        float t = startTime - Time.deltaTime;

        startTime -= Time.deltaTime;

        string seconds = t.ToString("f0");

        if(timerText!=null)
            timerText.text = seconds;

        if (t<=10)
        {
            if(timerText != null)
            {
                timerText.color = Color.red;
                FindObjectOfType<AudioManager>().Play("timeup"); 
            }
        }
        if (t<=0)
        {
            anim.SetTrigger("Death");
            GameOver();
        }
    }

    public void PauseGame()// pause game and SFX/ bg music
    {
        if(Time.timeScale==1)
        {      
           Time.timeScale = 0;
           FindObjectOfType<AudioManager>().Pause("Twitch Prime OST");
        }
        else
        {  
            Time.timeScale = 1;
            FindObjectOfType<AudioManager>().Play("Twitch Prime OST");
        }
    }

    private IEnumerator ResetPower()// coroutine for powerup  
    {
        yield return new WaitForSeconds(15);
        jumpForce =15f;
        GetComponent <SpriteRenderer>().color = Color.white;
    }

    public void LoadL2()//transition to L2
    {
        timerText.color = Color.green;
        timerText.text = "YAY";
        SceneManager.LoadScene(5);
        FindObjectOfType<AudioManager>().Play("yay");
        Start();
    }

    public void FinishedGame()//won the game method
    {   
        timerText.color = Color.green;
        timerText.text = "YAY";
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("yay");
    }

    public void GameOver()//game over method
    {
        timerText.color = Color.red;
        timerText.text = "OVER";
        StartCoroutine(DelayFn());
    }

    private IEnumerator DelayFn()//induce delay 
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}