using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MenuLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text ListaDejogadores;
    [SerializeField] private Button comecaJogo;

    [PunRPC]
    public void AtualizaLista()
    {
        ListaDejogadores.text = GestorDeRede.Instancia.ObterListaDeJogadores();
        comecaJogo.interactable = GestorDeRede.Instancia.DonoDaSala();
    }


}
