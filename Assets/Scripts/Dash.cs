using UnityEngine;

public class Dash : MonoBehaviour
{
    public float timeDash;
    private float startTimeDash = .25f;
    public bool dashing;
    private Movement _move;
    private InputSys _input;
    private Rigidbody2D _rb;
    private ColorSys _colorsys;

    // Start is called before the first frame update
    void Start()
    {
        _move = GetComponent<Movement>();
        _input = GetComponent<InputSys>();
        _rb = _move.GetComponent<Rigidbody2D>();
        _colorsys = GetComponent<ColorSys>();
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
        _colorsys.DashingColor();
        timeDash -= Time.deltaTime;
        //Debug.LogWarning($"Dashing at {_rb.velocity} with {timeDash} remaining.");
        if (timeDash <= 0)
        {
            timeDash = 0;
            dashing = false;
            _colorsys.DefaultColor();
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
