using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
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