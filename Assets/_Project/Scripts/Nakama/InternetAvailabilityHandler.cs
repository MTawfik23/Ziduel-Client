using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raskulls.ScriptableSystem;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

public class InternetAvailabilityHandler : MonoBehaviour
{
    [SerializeField] private SV_Socket socket = null;
    [SerializeField] private SV_bool internetAvailable = null;
    [SerializeField] private SE_Bool internetAvailabilityChanged = null;
    [SerializeField] private float requestInterval = 5f;
    private bool previousInternetAvailable = false;
    private bool firstTime = true;
    private async void Start()
    {
        firstTime = true;
        await Task.Delay(1000);
        socket.closed += SocketClosed;
        socket.connected += SocketConected;
        StartCoroutine(InternetConnectivitySocket());
    }

    private void OnDisable()
    {
        socket.closed -= SocketClosed;
        socket.connected -= SocketConected;
    }
    private void SocketClosed()
    {
        internetAvailable.Value = false;
        if (firstTime || internetAvailable.Value != previousInternetAvailable)
        {
            firstTime = false;
            internetAvailabilityChanged.Raise(internetAvailable.Value);
        }
        previousInternetAvailable = internetAvailable.Value;
    }
    private void SocketConected()
    {
        internetAvailable.Value = true;
        if (firstTime || internetAvailable.Value != previousInternetAvailable)
        {
            firstTime = false;
            internetAvailabilityChanged.Raise(internetAvailable.Value);
        }
        previousInternetAvailable = internetAvailable.Value;
    }
    IEnumerator InternetConnectivitySocket()
    {
    RestartLoopSocket:
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("NetworkReachability.NotReachable");
            SocketClosed();
        }
        else
        {
            
            if (socket.Value.IsConnected)
            {
                //Debug.Log("Connected");
                SocketConected();
            }
            else if (!socket.Value.IsConnected && !socket.Value.IsConnecting)
            {
                Debug.Log("call connection");
                SocketClosed();

                var reconnectTask = socket.reconnect.Invoke();
                yield return new WaitUntil(() => reconnectTask.IsCompleted || reconnectTask.IsCanceled || reconnectTask.IsFaulted);
                if (socket.Value.IsConnected) goto RestartLoopSocket;
            }
            else 
            {
                Debug.Log("Socket Try to Connecting");
            }
        }

        yield return new WaitForSeconds(requestInterval);
        goto RestartLoopSocket;
    }
    public void OnInternetAvailabilityChanged(System.Boolean value)
    {
        Debug.Log($"Internet Availability Changed To {value}");
    }
}
