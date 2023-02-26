using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raskulls.ScriptableSystem;
using Nakama.TinyJson;

public class ServerController : MonoBehaviour
{
    //Events to this ServerController ==> ToServerAwsdfasfgag
    //Events from this ServerController ==> FromServerSafagadg

    [Header("Scriptable Variables")]
    [SerializeField] private MatchMakingManager matchMakingManager = null;
    [SerializeField] private UI_ManagerScriptable uiManagerScriptable = null;
    [SerializeField] private SV_Player myPlayer = null;

    [Header("In-Game Scriptable Events")]
    private float variable = 0f;

    private void OnEnable()
    {
        matchMakingManager.matchStateQueue.Clear();
    }

    private void Start()
    {
        matchMakingManager.busy = false;
        matchMakingManager.Start();
        matchMakingManager.OnInitMatchState += OnInitMatchState;

        uiManagerScriptable.ChangePanel(UIPanels.WaitingForMatch);

        OnCreateOrJoinMatch();
    }

    private void OnDisable()
    {
        matchMakingManager.Unsubscribe();
        matchMakingManager.OnInitMatchState -= OnInitMatchState;
    }

    private void Update()
    {
        //Debug.Log($"matchmaking busy: {matchMakingManager.busy}");

        if (!matchMakingManager.busy)
        {
            if (matchMakingManager.matchStateQueue.Count > 0)
            {
                NextMatchState();
            }
        }


        ////REMOVE AFTER TESTING
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    changeBoneBodyInputState.Raise(boolOfInput);
        //    boolOfInput = !boolOfInput;
        //}
        ////REMOVE AFTER TESTING

    }
    public void NextMatchState()
    {
        matchMakingManager.busy = true;
        matchMakingManager.matchStateQueue.Dequeue().Invoke();
    }
    public void OnMatchStateFinished()
    {
        matchMakingManager.busy = false;
    }


    private void OnApplicationQuit()
    {
        matchMakingManager.OnQuitMatch();
    }

    public async void OnCreateOrJoinMatch()
    {
        if (matchMakingManager.currentMatchId != "")
            await matchMakingManager.ReconnectToMatch();
        else
            await matchMakingManager.FindMatch();
    }

    #region FromServer
    public void OnInitMatchState()
    {
        Debug.Log("OnInitMatchState");
        uiManagerScriptable.ChangePanel(UIPanels.InGame);
    }
    #endregion

    #region ToServer
    public void OnToServerRequestDraw()
    {
        //Send the matchmaker an OpCode
        //use myPlayer.ID to tell the server who wants to draw.
        //wait for the server to reply with the bones drawn
        //then send an event with all the bones drawn to the playerController
        //Dictionary<string, string> sendDictionary = new Dictionary<string, string>();
        //sendDictionary.Add("playerID", myPlayer.Value.id);
        //sendDictionary.Add("boneyardDrawDirection", ((int)boneyard.Value.BoneyardDrawDirection).ToString());

        //matchMakingManager.SendMatchState(OpCode.PLAYER_DRAW_REQUEST, sendDictionary.ToJson());
    }

    public void OnQuitMatch(System.Action value)
    {
        matchMakingManager.OnQuitMatch(value);
    }

    #endregion
    public async void OnInternetAvailabilityChanged(System.Boolean value)
    {
        if (!value)
        {
            uiManagerScriptable.ChangeSubPanel(UISubPanels.ReconnectingScreen);
            return;
        }
        bool isReconnected = await matchMakingManager.ReconnectToMatch();
        if (isReconnected)
        {
            Debug.Log("Reconnected");
            uiManagerScriptable.ChangeSubPanel(UISubPanels.None);
        }
    }

}
