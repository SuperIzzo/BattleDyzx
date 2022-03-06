using UnityEngine;

namespace BattleDyzx
{
    /// <summary> A collection of all BDX configurations in one place. </summary>
    [CreateAssetMenu(fileName = "BDXConfig", menuName = "BDX/BDX Config", order = 100)]
    public class BDXConfigFile : ScriptableObject
    {
        [SerializeField]
        private PlayerColors _playerColors;
        public PlayerColors playerColors => _playerColors;

        [SerializeField]
        private DyzkDatabase _dyzkDatabase;
        public DyzkDatabase dyzkDatabase => _dyzkDatabase;
    }
}