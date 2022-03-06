using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    /// <summary> Configuration of player colors. </summary>
    [CreateAssetMenu(fileName = "BDXPlayerColors", menuName = "BDX/Player Colors Config", order = 1)]
    public class PlayerColors : ScriptableObject
    {
        [SerializeField]
        List<Color> _playerColors;

        public Color this[int index] { get => _playerColors[index]; }
        public int Count { get => _playerColors.Count; }
    }
}