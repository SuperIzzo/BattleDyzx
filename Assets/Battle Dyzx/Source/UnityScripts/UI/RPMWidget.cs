using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    public class RPMWidget : MonoBehaviour
    {
        [SerializeField]
        private Image _currentRPMBar;

        [SerializeField]
        private Image _damageRPMBar;

        [SerializeField]
        private int _playerId = -1;

        [SerializeField]
        float _damageResetTime = 1.0f;

        public DyzkController dyzkController
        {
            get => BattleManager.instance?.GetDyzkController(_playerId);
            set { _playerId = value ? value.playerId : -1; }
        }

        public Dyzk dyzk { get => dyzkController?.dyzk; }

        float _damageTimer;

        float _preDamageRPM;

        private void Update()
        {
            if (ConfigManager.playerColors && _playerId >= 0 && _playerId < ConfigManager.playerColors.Count)
            {
                _currentRPMBar.color = ConfigManager.playerColors[_playerId];
                _damageRPMBar.color = Color.Lerp(_currentRPMBar.color, Color.black, 0.5f);
            }

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