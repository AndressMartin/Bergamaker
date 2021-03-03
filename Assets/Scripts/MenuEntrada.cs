using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEntrada : MonoBehaviour
{
    [SerializeField] private Text nomeDoJogador;
    [SerializeField] private Text nomeDaSala;

    public void CriaSala()
    {
        GestorDeRede.Instancia.MudaNick(nomeDoJogador.text);
        GestorDeRede.Instancia.CriaSala(nomeDaSala.text);
    }

    public void EntraSala()
    {
        GestorDeRede.Instancia.MudaNick(nomeDoJogador.text);
        GestorDeRede.Instancia.EntraSala(nomeDaSala.text);
    }

}
