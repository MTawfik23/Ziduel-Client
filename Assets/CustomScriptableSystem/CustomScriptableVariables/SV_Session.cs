using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Session_Name", menuName = "Raskulls/Scriptable System/Variables/Session")]
    public class SV_Session : ScriptableVariableBase
    {
        public ISession Value=null;
    }
}
