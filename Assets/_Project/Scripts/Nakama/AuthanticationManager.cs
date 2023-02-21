using Nakama;
using Nakama.TinyJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Raskulls.ScriptableSystem;
public class AuthanticationManager : MonoBehaviour
{
    [SerializeField] private SV_Client client = null;
    [SerializeField] private SV_Session session = null;
    [SerializeField] private SV_Socket socket = null;
    [SerializeField] private SV_Player myPlayer = null;
    [SerializeField] private SE authSuccess, authFaild = null;

    [Header("Authentication Settings")]
    [SerializeField] private bool UseLocalhostScheme = false;
    [SerializeField] private SV_AuthenticationScheme localhostScheme = null;
    [SerializeField] private SV_AuthenticationScheme nakamaScheme = null;

    //testing purposes
    [Header("Testing Purposes")]
    [SerializeField] private bool UseEditordeviceID = false;
    [SerializeField] private bool UseRandomdeviceID = false;
    //[SerializeField] [Tooltip("Leave Empty for a random ID")] private string deviceID = "QWREXAFWEEAWEWARAXFXCVXAQE";
    [SerializeField][Tooltip("Leave Empty for a random ID")] private SV_string deviceID = null;
    [SerializeField] private TMPro.TextMeshProUGUI text = null;

    [SerializeField] private bool isTryConnecting = false, isReconncetingSocket = false;

    private async void Awake()
    {
        // Connect to the Nakama server.
        if (UseLocalhostScheme)
            client.Value = new Client(localhostScheme.Scheme, localhostScheme.Host, localhostScheme.Port, localhostScheme.ServerKey);
        else
            client.Value = new Client(nakamaScheme.Scheme, nakamaScheme.Host, nakamaScheme.Port, nakamaScheme.ServerKey);

        socket.Value = client.Value.NewSocket();
        socket.Init();
        socket.reconnect += ReconnectSocket;
        //Debug.Log("Awak Call Connect");
        await Connect();
    }

    /// <summary>
    /// Connects to the Nakama server using device authentication and opens socket for realtime communication.
    /// </summary>
    public async Task Connect()
    {
        isTryConnecting = true;
        if (session.Value != null && !session.Value.IsExpired) return;
        try
        {
            await RegisterWithDeviceIDAsync();
            myPlayer.Value.id = session.Value.UserId;
            // Open a new Socket for realtime communication.
            await socket.Value.ConnectAsync(session.Value, true);
            Debug.Log($"Auth Success  UserId={session.Value.UserId} , username={session.Value.Username}");
            if (text) text.text = $"Auth Success UserId={session.Value.UserId} , username={session.Value.Username}";
            authSuccess.Raise();
        }
        catch (Exception ex)
        {
            authFaild.Raise();
            Debug.LogError($"Auth Faild With Message {ex.Message}");
            if (text) text.text = $"Auth Faild With Message {ex.Message}";
            socket.Value = client.Value.NewSocket();
            socket.Init();
        }
        isTryConnecting = false;
    }

    private async Task RegisterWithDeviceIDAsync()
    {
        try
        {
            // If we've reach this point, get the device's unique identifier or generate a unique one.
            string deviceId = SystemInfo.deviceUniqueIdentifier;

            if (deviceId == SystemInfo.unsupportedIdentifier)
                deviceId = System.Guid.NewGuid().ToString();
            if (UseEditordeviceID) deviceId = deviceID.Value;
            if (UseRandomdeviceID) deviceId = System.Guid.NewGuid().ToString();
            //text.text = deviceId;
            // Use Nakama Device authentication to create a new session using the device identifier.
            session.Value = await client.Value.AuthenticateDeviceAsync(deviceId, create: true);

            //playerData.currentPlayerData.Username = session.Value.Username;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't RegisterWithDeviceIDAsync because of {ex.Message}");
        }
    }
    private async Task ReconnectSocket()
    {
        if (isReconncetingSocket) return;

        isReconncetingSocket = true;
        try
        {
            if (socket.Value.IsConnecting)
            {
                isReconncetingSocket = false;
                return;
            }

            if (socket.Value.IsConnected)
            {
                isReconncetingSocket = false;
                return;
            }

            if (session.Value == null)
            {
                if (isTryConnecting)
                {
                    isReconncetingSocket = false;
                    return;
                }
                Debug.Log("await Connect();");
                await Connect();
                isReconncetingSocket = false;
                return;
            }

            if (session.Value.IsExpired)
            {
                try
                {
                    session.Value = await client.Value.SessionRefreshAsync(session.Value);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Can't SessionRefreshAsync because of {ex.Message}");
                    isReconncetingSocket = false;
                    return;
                }
            }

            if (!socket.Value.IsConnected)
            {
                try
                {
                    await socket.Value.ConnectAsync(session.Value, true);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Can't ConnectAsync Socket because of {ex.Message}");
                    await socket.Value.CloseAsync();
                    socket.Value = client.Value.NewSocket();
                    socket.Init();
                    isReconncetingSocket = false;
                    return;
                }
            }
            else Debug.Log("socket already connected");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't ReconnectSocket because of {ex.Message}");
        }
        isReconncetingSocket = false;
    }
    private async void OnApplicationQuit()
    {

        socket.Unsubscribe();
        socket.reconnect -= ReconnectSocket;
        await socket.Value.CloseAsync();
        await client.Value.SessionLogoutAsync(session.Value);
    }

    public async void OnInternetAvailabilityChanged(System.Boolean value)
    {
        try
        {
            if (!value) return;
            await ReconnectSocket();
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Can't OnInternetAvailabilityChanged because of {ex.Message}");
        }

    }
}
