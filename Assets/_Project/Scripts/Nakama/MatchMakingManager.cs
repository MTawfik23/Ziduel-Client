using Nakama;
using Nakama.TinyJson;
using Raskulls.ScriptableSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System;

[CreateAssetMenu(fileName = "MatchMakingManager_Name", menuName = "Ziduel/Match Making Manager")]
public class MatchMakingManager : ScriptableObject
{
    private IUserPresence localUser = null;
    public string currentMatchId = "";
    private IMatchmakerTicket matchmakerTicket = null;
    public IPartyMatchmakerTicket partyMatchmakerTicket = null;
    private const string currentMatchKey = "CurrentMatch_";
    public bool busy = false;
    [SerializeField] private SV_Socket socket = null;
    [SerializeField] private SV_Player myPlayer = null;
    [SerializeField] private SE_LoadScene loadScene = null;
    [SerializeField] private SV_GameScene homeScreen = null;
    [SerializeField] private SV_GameScene onlineGameScene = null;

    public delegate void InitMatchState();

    public InitMatchState OnInitMatchState;
    public InitMatchState OnReconnectState;

    public Queue<Action> matchStateQueue = new Queue<Action>();

    private UnityMainThreadDispatcher mainThread;
    public void Start()
    {
        currentMatchId = "";
        mainThread = UnityMainThreadDispatcher.Instance();
        socket.receivedMatchmakerMatched += SubscribeReceivedMatchmakerMatched;
        socket.receivedMatchPresence += SubscribeReceivedMatchPresence;
        socket.receivedMatchState += SubscribeReceivedMatchState;
    }
    public void Unsubscribe()
    {
        Debug.Log("Unsubscribe");
        if (socket.Value == null) return;
        socket.receivedMatchmakerMatched -= SubscribeReceivedMatchmakerMatched;
        socket.receivedMatchPresence -= SubscribeReceivedMatchPresence;
        socket.receivedMatchState -= SubscribeReceivedMatchState;
    }
    private void SubscribeReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        mainThread.Enqueue(() => OnReceivedMatchmakerMatched(matched));
    }


    private void SubscribeReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
    {
        mainThread.Enqueue(() => OnReceivedMatchPresence(matchPresenceEvent));
    }
    private void SubscribeReceivedMatchState(IMatchState matchState)
    {
        mainThread.Enqueue(() => OnReceivedMatchState(matchState));
    }

    public void SaveCurrentMatchId()
    {
        PlayerPrefs.SetString(currentMatchKey + myPlayer.Value.id, currentMatchId);
        Debug.Log($"Match id Saved and equal {currentMatchId}");
    }
    public void LoadCurrentMatchId()
    {
        if (!PlayerPrefs.HasKey(currentMatchKey + myPlayer.Value.id))
        {
            currentMatchId = "";
            return;
        }

        currentMatchId = PlayerPrefs.GetString(currentMatchKey + myPlayer.Value.id);
    }
    public void ClearCurrentMatchId()
    {
        if (PlayerPrefs.HasKey(currentMatchKey + myPlayer.Value.id))
        {
            PlayerPrefs.DeleteKey(currentMatchKey + myPlayer.Value.id);
        }
        currentMatchId = "";
    }
    public async void OnQuitMatch(Action toDoAfterQuitting = null)
    {
        await QuitMatch();
        Debug.Log("Quit match");
        if (toDoAfterQuitting != null) SaveCurrentMatchId();
        toDoAfterQuitting?.Invoke();
    }


    /// <summary>
    /// Called when a MatchmakerMatched event is received from the Nakama server.
    /// </summary>
    /// <param name="matched">The MatchmakerMatched data.</param>
    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        // Cache a reference to the local user.
        localUser = matched.Self.Presence;

        // Join the match.
        var match = await socket.Value.JoinMatchAsync(matched);

        // Disable the main menu UI and enable the in-game UI.

        // Spawn a player instance for each connected user.
        foreach (var user in match.Presences)
        {
            Debug.Log($"Match Maker Matched UserId={user.UserId} , UserName={user.Username}");
        }

        // Cache a reference to the current match.
        currentMatchId = match.Id;
        SaveCurrentMatchId();
    }


    /// <summary>
    /// Called when a player/s joins or leaves the match.
    /// </summary>
    /// <param name="matchPresenceEvent">The MatchPresenceEvent data.</param>
    private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
    {
        // For each new user that joins, spawn a player for them.
        foreach (var user in matchPresenceEvent.Joins)
        {
            Debug.Log($"Player Joins UserId={user.UserId} , UserName={user.Username}");
        }

        // For each player that leaves, despawn their player.
        foreach (var user in matchPresenceEvent.Leaves)
        {
            Debug.Log($"Player Leaves UserId={user.UserId} , UserName={user.Username}");
        }
    }


    /// <summary>
    /// Called when new match state is received.
    /// </summary>
    /// <param name="matchState">The MatchState data.</param>
    private void OnReceivedMatchState(IMatchState matchState)
    {
        // Get the local user's session ID.
        //var userSessionId = matchState.UserPresence.SessionId;
        //Debug.Log("OpCode=" + matchState.OpCode);
        // Decide what to do based on the Operation Code as defined in OpCodes.

        switch (matchState.OpCode)
        {
            case OpCode.START_MATCH:
                //Debug.Log("Enqueue :"+matchState.OpCode + ": Start Match");
                matchStateQueue.Enqueue(() => ReceivedStartMatchState(matchState));
                break;
            default:
                break;
        }

    }

    private void ReceivedStartMatchState(IMatchState matchState)
    {
        Debug.Log("Dequeue : Start Match");
        var state = matchState.State.Length > 0 ? System.Text.Encoding.UTF8.GetString(matchState.State).FromJson<Dictionary<string, string>>() : null;
        if (state == null)
        {
            Debug.LogError("State is null");
        }
        else
        {
            foreach (var item in state)
            {
                Debug.Log($"Key={item.Key} , Value={item.Value}");
            }
            //state["ground"] = "{" + state["ground"] + "}";
            //state["game"] = "{" + state["game"] + "}";
        }
        OnInitMatchState.Invoke();
    }

    private void ReceivedReconnectState(IMatchState matchState)
    {
        //Debug.Log("Dequeue : Reconnect");
        var state = matchState.State.Length > 0 ? System.Text.Encoding.UTF8.GetString(matchState.State).FromJson<Dictionary<string, string>>() : null;
        OnReconnectState.Invoke();
    }

    /// <summary>
    /// Cancels the current matchmaking request.
    /// </summary>
    public async Task CancelMatchmaking()
    {
        await socket.Value.RemoveMatchmakerAsync(matchmakerTicket.Ticket);
    }

    /// <summary>
    /// Quits the current match.
    /// </summary>
    public async Task QuitMatch()
    {

        // Ask Nakama to leave the match.
        if (currentMatchId != "") await socket.Value.LeaveMatchAsync(currentMatchId);

        // Reset the currentMatch and localUser variables.
        currentMatchId = "";
        localUser = null;
        matchmakerTicket = null;
        partyMatchmakerTicket = null;
        // Show the main menu, hide the in-game menu.
    }

    /// <summary>
    /// Starts looking for a match with a given number of minimum players.
    /// </summary>
    public async Task FindMatch()
    {
        //var query = "*";
        var query = "+properties.mode:" + (int)0;
        var numericProperties = new Dictionary<string, double>() { { "mode", (int)0 } };
        var stringProprties = new Dictionary<string, string>() { { "partyID", "" } };
        int minPlayers = 2, maxPlayers = 2;
        if (partyMatchmakerTicket == null) await FindMatchForSinglPlayer(query, minPlayers, maxPlayers, stringProprties, numericProperties);
    }
    private async Task FindMatchForSinglPlayer(string query = "*", int minPlayers = 2, int maxPlayers = 8, Dictionary<string, string> stringProperties = null, Dictionary<string, double> numericProperties = null)
    {
        try
        {
            // Add this client to the matchmaking pool and get a ticket.
            matchmakerTicket = await socket.Value.AddMatchmakerAsync(query, minPlayers, maxPlayers, stringProperties, numericProperties);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't AddMatchmaker because of {ex.Message}");
            loadScene.Raise(onlineGameScene, true, true);
        }
    }

    private async Task FindMatchForParty(string partyID, string query = "*", int minPlayers = 2, int maxPlayers = 8, Dictionary<string, string> stringProperties = null, Dictionary<string, double> numericProperties = null)
    {
        try
        {
            // Add this client to the matchmaking pool and get a ticket.
            partyMatchmakerTicket = await socket.Value.AddMatchmakerPartyAsync(partyID, query, minPlayers, maxPlayers, stringProperties, numericProperties);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't AddMatchmakerParty because of {ex.Message}");
            loadScene.Raise(onlineGameScene, true, true);
        }
    }
    /// <summary>
    /// Sends a match state message across the network.
    /// </summary>
    /// <param name="opCode">The operation code.</param>
    /// <param name="state">The stringified JSON state data.</param>
    public void SendMatchState(long opCode, string state)
    {
        socket.Value.SendMatchStateAsync(currentMatchId, opCode, state);
    }


    /// <summary>
    /// Sends a match state message across the network.
    /// </summary>
    /// <param name="opCode">The operation code.</param>
    /// <param name="state">The stringified JSON state data.</param>
    public async Task SendMatchStateAsync(long opCode, string state)
    {
        await socket.Value.SendMatchStateAsync(currentMatchId, opCode, state);
    }

    public async Task<bool> ReconnectToMatch()
    {
        if (currentMatchId == "") return false;
        bool isMatchActive = false;
        try
        {
            var rpcRespond = await socket.Value.RpcAsync("isMatchActive_rpc", currentMatchId);
            isMatchActive = bool.Parse(rpcRespond.Payload);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't send isMatchActive_rpc because of {ex.Message}");
            return false;
        }


        if (!isMatchActive)
        {
            Debug.LogError("Current Match Not Active");
            loadScene.Raise(homeScreen, true, true);
            return false;
        }

        try
        {
            var match = await socket.Value.JoinMatchAsync(currentMatchId);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't join match because of {ex.Message}");
            //loadScene.Raise(onlineGameScene, true, true);
            return false;
        }

        return true;
    }
}