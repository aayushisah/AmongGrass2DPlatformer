using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float groundcap; //-7.75
    [SerializeField] private float upcap;
    [SerializeField] private float jumplength = 2f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private LayerMask Ground;

    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingup = true;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (facingup)
        {
            if (transform.position.y> groundcap)
            {
                if(transform.localScale.y != 1)
                {
                    transform.localScale = new Vector3(1,1,1);
                }
                if(coll.IsTouchingLayers(Ground))
                {
                    jumpheight = 2f;
                    rb.velocity = new Vector2(-jumplength,jumpheight);
                }
            }
            else
            {
                facingup = false;
            }
        }
        else 
        {
            if (transform.position.y < upcap)
            {
                if(transform.localScale.y != -1)
                {
                    transform.localScale = new Vector3(-1,-1,1);
                    jumpheight = -2f;
                    rb.velocity = new Vector2(jumplength,jumpheight);
                }
            }
            else
            {
                facingup = true;
            }
        }
    }
}
