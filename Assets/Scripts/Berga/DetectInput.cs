using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInput : MonoBehaviour
{
    public Player jogador;
    float horizontal, vertical;
    bool interagindo;

    void Start()
    {
        jogador = FindObjectOfType<Player>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        jogador.AlterarDirecao(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.E))
        {
            jogador.AlterarInteracao(true);
        }


    }
}

