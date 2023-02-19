using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Transform_Name", menuName = "Raskulls/Scriptable System/Variables/Transform")]
    public class SV_Transform : ScriptableVariableBase
    {
        public Transform Value;
    }
}
