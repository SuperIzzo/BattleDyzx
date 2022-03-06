using UnityEngine;

namespace BattleDyzx
{
    /// <summary>
    /// A singleton component that provides readonly access to various project configurations.
    /// </summary>
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField]
        BDXConfigFile _config;

        static ConfigManager _instance;
        static ConfigManager instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<ConfigManager>();
                }
                return _instance;
            }
        }

        public void Awake()
        {
            _instance = this;            
        }

        public static PlayerColors playerColors { get => instance._config.playerColors; }
        public static DyzkDatabase dyzkDatabase { get => instance._config.dyzkDatabase; }
    }
}