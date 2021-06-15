using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModel: MonoBehaviour
{
    public bool _permissaoAndar = true,
                lento;

    public float velocidade = 4f, 
                 velocidadeM = 1f;

    public virtual void PermitirMovimento(bool permissao)
    {
        _permissaoAndar = permissao;
    }

    public virtual void Lento(bool condicao)
    {
        lento = condicao;
    }
}
