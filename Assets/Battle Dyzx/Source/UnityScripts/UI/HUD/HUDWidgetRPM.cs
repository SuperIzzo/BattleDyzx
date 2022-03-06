using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    /// <summary> A widget that shows RPM as a bar. </summary>
    public class HUDWidgetRPM : HUDWidgetBase
    {
        [SerializeField]
        private Image _currentRPMBar;

        [SerializeField]
        private Image _damageRPMBar;

        [SerializeField]
        private float _damageResetTime = 1.0f;

        private float _damageTimer;
        private float _preDamageRPM;

        private void Update()
        {
            _currentRPMBar.color = playerColor;
            _damageRPMBar.color = Color.Lerp(_currentRPMBar.color, Color.black, 0.5f);

            if (dyzk)
            {
                _currentRPMBar.fillAmount = dyzk.dyzkState.RPM / dyzk.dyzkState.maxRPM;
                _damageRPMBar.fillAmount = _preDamageRPM / dyzk.dyzkState.maxRPM;
                _damageRPMBar.gameObject.SetActive(_damageTimer > 0.0f);
                UpdateDamageTimer();
            }            
        }

        private void UpdateDamageTimer()
        {
            if (dyzk.dyzkState.collisionData.numDyzkCollisions > 0)
            {
                if (_damageTimer <= 0.0f)
                {
                    _preDamageRPM = dyzk.dyzkState.RPM;
                }

                _damageTimer = _damageResetTime;
            }
            else if (_damageTimer > 0.0f)
            {
                _damageTimer -= Time.deltaTime;
                if (_damageTimer <= 0.0f)
                {
                    _damageTimer = 0.0f;
                }
            }
        }
    }
}