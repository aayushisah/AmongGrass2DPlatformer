using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrog : MonoBehaviour
{
    [SerializeField] private float leftcap;
    [SerializeField] private float rightcap;
    [SerializeField] private float jumplength = 2f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private LayerMask Ground;

    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingleft = true;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (facingleft)
        {
            if (transform.position.x > leftcap)
            {
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1,1,1);
                }
                if(coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumplength,jumpheight);
                }

            }
            else
            {
                facingleft = false;
            }
        }
        else 
        {
            if (transform.position.x < rightcap)
            {
                if(transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1,1,1);
                }
                if(coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumplength,jumpheight);
                }

            }
            else
            {
                facingleft = true;
            }
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void jumpedOn()
    {
        anim.SetTrigger("Death");
        
        Debug.Log("ok");
    }

    private void Death()
    {
        Destroy(gameObject);
        Debug.Log("ok death");
    }
}
