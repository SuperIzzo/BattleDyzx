using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    /// <summary> Static database of dyzk images. </summary>
    [CreateAssetMenu(fileName = "BDXData", menuName = "BDX/Dyzk Database", order = 2)]
    public class DyzkDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Texture2D> _dyzkTextures;

        public List<Texture2D> dyzkTextures => _dyzkTextures;
    }
}