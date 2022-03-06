using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    /// <summary> A widget to render the dyzk controller name. </summary>
    public class HUDWidgetControllerName : HUDWidgetBase
    {
        [SerializeField]
        private Text _playerLabel;

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

        private void Update()
        {
            if (!_playerLabel)
            {
                return;
            }

            if (dyzkController)
            {
                _playerLabel.text = dyzkController.controllerName;
            }

            Color col = playerColor;
            _playerLabel.color = new Color(col.r, col.g, col.b, _playerLabel.color.a);
            playerLabelOutline.effectColor = col * _originalOutlineColor;
        }
    }
}