using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModel: MonoBehaviour
{
    public bool _permissaoAndar = true,
                lento;

    public float velocidade = 4f, 
                 velocidadeM = 1f;

    //Variaveis para as animacoes
    public bool acertandoAtaque = false,
                terminandoAtaque = false;
    public virtual void PermitirMovimento(bool permissao)
    {
        _permissaoAndar = permissao;
    }

    public virtual void Lento(bool condicao)
    {
        lento = condicao;
    }

    public virtual void AnimacaoIniciarCasting()
    {

    }

    public virtual void DefinirDirecaoAtaque(int AOE, Vector3 pointClicked, List<GameObject> targets)
    {

    }
    public virtual void AnimacaoAtaqueBasico()
    {

    }
}
