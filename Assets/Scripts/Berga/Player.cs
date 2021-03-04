using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D Mybox2d;

    float horizontal;
    float vertical;

    public float runSpeed = 5.0f;

    public bool interagindo = false;

    [SerializeField]
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Mybox2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {


        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
   
        if (Input.GetKeyDown(KeyCode.E))
        {
            interagindo = !interagindo;
        }
        

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * runSpeed;
    }
}
