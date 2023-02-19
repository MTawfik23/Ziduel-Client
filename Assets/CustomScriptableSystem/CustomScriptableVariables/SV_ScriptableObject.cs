using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "ScriptableObject_Name", menuName = "Raskulls/Scriptable System/Variables/ScriptableObject")]
    public class SV_ScriptableObject : ScriptableObject
    {
        public ScriptableObject value = null;
    }
}
