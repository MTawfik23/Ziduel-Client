using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string id = "";
    [SerializeField]private bool isMine = false;

    public bool IsMine { get => isMine; }

    public Player()
    {
        id = "";
        isMine = false;
    }
    public Player(Player player)
    {
        id = player.id;
        isMine = player.isMine;
    }
    
    public void SetIsMine(string inID)
    {
        isMine = id == inID;
    }
    public override string ToString()
    {
        return "Player (" + id.ToString() + "), IsMine: " + isMine.ToString();
    }
}
