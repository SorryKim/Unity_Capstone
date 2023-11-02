using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;





public class GameSystem : MonoBehaviourPunCallbacks
{
    public static GameSystem instance;

    public List<Player> players = new List<Player>();
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    public void AddPlayer(Player player)
    {
        if(!players.Contains(player))
            players.Add(player);
    }


    private IEnumerator GameReady()
    {
        int num = PhotonNetwork.CurrentRoom.PlayerCount;
        while(num != players.Count)
        {
            yield return null;
        }

        int liarIndex = Random.Range(0, players.Count);
        var liar = players[liarIndex];
        //liar.playerType = EPlayerType.Liar;
        

        
    }
}
