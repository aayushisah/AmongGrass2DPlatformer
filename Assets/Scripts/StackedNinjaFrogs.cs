using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedNinjaFrogs : MonoBehaviour
{
    [SerializeField] private float leftcap;
    [SerializeField] private float rightcap;
    [SerializeField] private float jumplength = 2f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private LayerMask Ground;
  

    private Collider2D coll;
    private Rigidbody2D rb;
    
    private bool facingleft = true;
    public bool isBase;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
                if(coll.IsTouchingLayers(Ground) )
                {
                    if (isBase)
                    rb.velocity = new Vector2(-jumplength,jumpheight);
                    else
                    transform.localPosition= new Vector2(0,transform.localPosition.y);
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
                    if (isBase)
                    rb.velocity = new Vector2(jumplength,jumpheight);
                    else
                    transform.localPosition= new Vector2(0,transform.localPosition.y);
                }
            }
            else
            {
                facingleft = true;
            }
        }
    }
   
    public void Death()
    {
        Destroy(gameObject);
        //Debug.Log("ok death");
    }
}
