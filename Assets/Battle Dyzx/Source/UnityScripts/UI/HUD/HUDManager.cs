using UnityEngine;

namespace BattleDyzx
{
    /// <summary> Responsible for instantiating and managing HUD widget. </summary>
    public class HUDManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _rpmWidgetHolder;

        [SerializeField]
        private Transform _arrowWidgetHolder;

        [SerializeField]
        private DyzkControllerReference _rpmWidgetPrefab;

        [SerializeField]
        private DyzkControllerReference _arrowWidgetPrefab;

        private void Awake()
        {
            BattleManager.instance.OnDyzkControllerAdded += OnDyzkControllerAdded;
        }

        private void OnDyzkControllerAdded(DyzkController dyzkController)
        {
            DyzkControllerReference rpmWidget = Instantiate(_rpmWidgetPrefab, _rpmWidgetHolder);
            DyzkControllerReference arrowWidget = Instantiate(_arrowWidgetPrefab, _arrowWidgetHolder);
            rpmWidget.dyzkController = dyzkController;
            arrowWidget.dyzkController = dyzkController;
        }
    }
}
