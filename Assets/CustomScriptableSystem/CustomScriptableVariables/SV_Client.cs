using Nakama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Client_Name", menuName = "Raskulls/Scriptable System/Variables/Client")]
    public class SV_Client : ScriptableVariableBase
    {
        public Client Value;
    }
}
