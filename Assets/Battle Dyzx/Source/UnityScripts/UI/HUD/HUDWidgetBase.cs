using UnityEngine;

namespace BattleDyzx
{
    /// <summary> A base for HUD widgets, each widget is associated with dyzk controller. </summary>
    public class HUDWidgetBase : MonoBehaviour
    {
        private DyzkControllerReference _controllerReference;
        protected DyzkControllerReference controllerReference
        {
            get
            {
                if (!_controllerReference)
                {
                    _controllerReference = GetComponentInParent<DyzkControllerReference>();
                }
                return _controllerReference;
            }
        }

        public int dyzkControllerId => controllerReference ? controllerReference.dyzkControllerId : -1;

        public DyzkController dyzkController => controllerReference?.dyzkController;

        public Dyzk dyzk => controllerReference?.dyzk;

        public Color playerColor
        {
            get
            {
                if (ConfigManager.playerColors && dyzkControllerId >= 0 && dyzkControllerId < ConfigManager.playerColors.Count)
                {
                    return ConfigManager.playerColors[dyzkControllerId];
                }
                else
                {
                    return Color.white;
                }
            }
        }
    }
}