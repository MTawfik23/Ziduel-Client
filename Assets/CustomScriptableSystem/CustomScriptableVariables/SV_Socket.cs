using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Socket_Name", menuName = "Raskulls/Scriptable System/Variables/Socket")]
    public class SV_Socket : ScriptableVariableBase
    {
        public ISocket Value = null;
        public Action connected = null;
        public Action closed = null;
        public Action<IMatchmakerMatched> receivedMatchmakerMatched = null;
        public Action<IMatchPresenceEvent> receivedMatchPresence = null;
        public Action<IMatchState> receivedMatchState = null;
        public Action<IStatusPresenceEvent> receivedStatusPresence = null;
        public Action<IApiNotification> receivedNotification = null;
        public Action<IParty> receivedParty = null;
        public Action<IPartyLeader> receivedPartyLeader = null;
        public Action<IPartyPresenceEvent> receivedPartyPresence = null;
        public Action<IPartyMatchmakerTicket> receivedPartyMatchmakerTicket = null;
        public Func<Task> reconnect = null;

        public void Init()
        {
            Value.Connected += OnConnected;
            Value.Closed += OnClosed;

            Value.ReceivedStatusPresence += ReceivedStatusPresence;
            Value.ReceivedNotification += ReceivedNotification;

            Value.ReceivedParty += ReceivedParty;
            Value.ReceivedPartyLeader += ReceivedPartyLeader;
            Value.ReceivedPartyPresence += ReceivedPartyPresence;
            
            SubscribeMatchEvents();
        }

        private void ReceivedPartyPresence(IPartyPresenceEvent obj)
        {
            receivedPartyPresence?.Invoke(obj);
        }

        private void ReceivedPartyLeader(IPartyLeader partyLeader)
        {
            receivedPartyLeader?.Invoke(partyLeader);
        }

        private void ReceivedParty(IParty party)
        {
            receivedParty?.Invoke(party);
        }

        public void SubscribeMatchEvents()
        {
            Value.ReceivedMatchmakerMatched += ReceivedMatchmakerMatched;
            Value.ReceivedMatchPresence += ReceivedMatchPresence;
            Value.ReceivedMatchState += ReceivedMatchState;
            Value.ReceivedPartyMatchmakerTicket += ReceivedPartyMatchmakerTicket;
        }

        private void ReceivedPartyMatchmakerTicket(IPartyMatchmakerTicket partyMatchmakerTicket)
        {
            receivedPartyMatchmakerTicket?.Invoke(partyMatchmakerTicket);
        }

        public void UnsubscribeMatchEvents()
        {
            Value.ReceivedMatchmakerMatched -= ReceivedMatchmakerMatched;
            Value.ReceivedMatchPresence -= ReceivedMatchPresence;
            Value.ReceivedMatchState -= ReceivedMatchState;
        }

        private void OnConnected()
        {
            //Debug.Log("Connected To The Server");
            connected?.Invoke();
        }
        private void OnClosed()
        {
            //Debug.Log("DisConnected From The Server");
            closed?.Invoke();
        }

        private void ReceivedMatchmakerMatched(IMatchmakerMatched obj)
        {
            receivedMatchmakerMatched?.Invoke(obj);
        }
        private void ReceivedMatchPresence(IMatchPresenceEvent obj)
        {
            receivedMatchPresence?.Invoke(obj);
        }
        private void ReceivedMatchState(IMatchState obj)
        {
            receivedMatchState?.Invoke(obj);
        }

        private void ReceivedStatusPresence(IStatusPresenceEvent obj)
        {
            receivedStatusPresence?.Invoke(obj);
        }
        private void ReceivedNotification(IApiNotification obj)
        {
            receivedNotification?.Invoke(obj);
        }

        public void Unsubscribe()
        {
            Value.Connected -= OnConnected;
            Value.Closed -= OnClosed;

            Value.ReceivedStatusPresence -= ReceivedStatusPresence;
            Value.ReceivedNotification -= ReceivedNotification;

            Value.ReceivedParty -= ReceivedParty;
            Value.ReceivedPartyLeader -= ReceivedPartyLeader;
            Value.ReceivedPartyPresence -= ReceivedPartyPresence;

            UnsubscribeMatchEvents();
        }

    }
}
