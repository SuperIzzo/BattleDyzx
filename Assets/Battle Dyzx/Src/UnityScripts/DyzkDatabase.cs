using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BDXData", menuName = "BDX/Create Dyzk Database", order = 1)]
public class DyzkDatabase : ScriptableObject
{
    [SerializeField]
    private List<Texture2D> _dyzkTextures;

    public List<Texture2D> dyzkTextures => _dyzkTextures;
}
