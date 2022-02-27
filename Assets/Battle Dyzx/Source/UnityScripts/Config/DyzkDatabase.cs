using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    [CreateAssetMenu(fileName = "BDXData", menuName = "BDX/Dyzk Database", order = 2)]
    public class DyzkDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Texture2D> _dyzkTextures;

        public List<Texture2D> dyzkTextures => _dyzkTextures;
    }
}