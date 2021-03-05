using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D Mybox2d;
    public float horizontal { get;private  set; }
    public float vertical { get; private set; }
    public bool interagindo { get; private set; }

    public float runSpeed = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Mybox2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
       
    }
  
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * runSpeed;
    }

    public void AlterarDirecao(float HorizontalEST,float VerticalEST)
    {
        horizontal = HorizontalEST;
        vertical = VerticalEST;
    }
    public void AlterarInteracao(bool InteracaoEST)
    {
        interagindo = InteracaoEST;
    }
}
