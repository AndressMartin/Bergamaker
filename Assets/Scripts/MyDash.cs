using UnityEngine;

public class MyDash : MonoBehaviour
{
    public float timeDash;
    private float startTimeDash = .4f;
    public bool dashing;
    private Movement _move;
    private MyInput _input;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _move = FindObjectOfType<Movement>();
        _input = FindObjectOfType<MyInput>();
        _rb = _move.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing == true)
            DoDash();
        if (_input.dashPress && _move.lento != true && _move._permissaoAndar == true)
        {
            startDash();
        }
    }
    private void DoDash()
    {
        _move.DashingColor();
        timeDash -= Time.deltaTime;
        //Debug.LogWarning($"Dashing at {_rb.velocity} with {timeDash} remaining.");
        if (timeDash <= 0)
        {
            timeDash = 0;
            dashing = false;
            _move.DefaultColor();
        }
    }

    public void startDash()
    {
        if (timeDash <= 0 && (_input.horizontal != 0 || _input.vertical != 0))
        {
            timeDash = startTimeDash;
            dashing = true;

            if (_input.horizontal == 1)
            {
                _rb.AddForce(Vector2.right * _move.velocidade / 5000, ForceMode2D.Impulse);
            }
            else if (_input.horizontal == -1)
            {
                _rb.AddForce(Vector2.left * _move.velocidade / 5000, ForceMode2D.Impulse);
            }
            if (_input.vertical == 1)
            {
                _rb.AddForce(Vector2.up * _move.velocidade / 5000, ForceMode2D.Impulse);
            }
            else if (_input.vertical == -1)
            {
                _rb.AddForce(Vector2.down * _move.velocidade / 5000, ForceMode2D.Impulse);
            }
        }
            
    }
}
