using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GestorDeRede : MonoBehaviourPunCallbacks
{
  public static GestorDeRede Instancia { get; private set; }
    
    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexão GG");
    }

    public void CriaSala(string nomeSala)
    {
        PhotonNetwork.CreateRoom(nomeSala);
    }

    public void EntraSala(string nomeSala)
    {
        PhotonNetwork.JoinRoom(nomeSala);
    }

    public void MudaNick(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    public string ObterListaDeJogadores()
    {
        var lista = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            lista += player.NickName + "\n";
        }
        return lista;
    }

    public bool DonoDaSala()
    {
        return PhotonNetwork.IsMasterClient;
    }
    public void SairLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    [PunRPC]
    public void ComecaJogo(string nomeCena)
    {
        PhotonNetwork.LoadLevel(nomeCena);
    }
}
