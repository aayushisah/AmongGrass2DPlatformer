using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    [SerializeField] private float upLimit;
    
    [SerializeField] private float jumplength = 0f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private LayerMask Ground;

    private Collider2D coll;
    private Rigidbody2D rb;
    

    //private bool up= true;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
            if (transform.position.y < upLimit)
            {
                transform.localScale = new Vector3(1,-1,1);
                rb.velocity = new Vector2(jumplength,-jumpheight);

            }
        
            if (transform.position.x > upLimit)
            {   
                transform.localScale = new Vector3(1,1,1);
                rb.velocity = new Vector2(jumplength,jumpheight);
                
            }   
    }
/*
    public float bounceStrength;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Vector2 normal = collision.GetContact(0).normal;
            ball.AddForce(-normal * this.bounceStrength);
        }
    }
    */
}
