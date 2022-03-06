using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    public class PlayerNameText : MonoBehaviour
    {
        [SerializeField]
        private int _playerId = -1;

        [SerializeField]
        private Text _playerLabel;

        public DyzkController dyzkController
        {
            get => BattleManager.instance?.GetDyzkController(_playerId);
            set { _playerId = value ? value.playerId : -1; }
        }

        private Color _originalOutlineColor;
        private Outline _playerLabelOutline;
        private Outline playerLabelOutline
        {
            get
            {
                if (!_playerLabelOutline && _playerLabel)
                {
                    _playerLabelOutline = _playerLabel.GetComponent<Outline>();
                    if (_playerLabelOutline)
                    {
                        _originalOutlineColor = _playerLabelOutline.effectColor;
                    }
                }
                return _playerLabelOutline;
            }
        }

        void Update()
        {
            if (!_playerLabel)
            {
                return;
            }

            if (dyzkController)
            {
                _playerLabel.text = dyzkController.playerName;
            }

            if (ConfigManager.playerColors && _playerId >= 0 && _playerId < ConfigManager.playerColors.Count)
            {
                Color col = ConfigManager.playerColors[_playerId];
                _playerLabel.color = new Color(col.r, col.g, col.b, _playerLabel.color.a);
                playerLabelOutline.effectColor = col * _originalOutlineColor;
            }
        }
    }
}