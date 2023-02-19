using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "AuthenticationScheme_Name", menuName = "Raskulls/Scriptable System/Variables/AuthenticationScheme")]
    public class SV_AuthenticationScheme : ScriptableVariableBase
    {
        public string Scheme;
        public string Host;
        public int Port;
        public string ServerKey;
    }
}
