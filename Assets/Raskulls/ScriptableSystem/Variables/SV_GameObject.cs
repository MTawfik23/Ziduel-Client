using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "GameObject_Name", menuName = "Raskulls/Scriptable System/Variables/GameObject")]
    public class SV_GameObject : ScriptableVariableBase
    {
        public GameObject Value;
    }
}
