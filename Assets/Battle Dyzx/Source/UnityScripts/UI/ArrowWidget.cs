using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    public class ArrowWidget : MonoBehaviour
    {
        [SerializeField]
        private int _playerId = -1;

        [SerializeField]
        private Image _indicatorArrow;

        [SerializeField]
        private Image _controlArrow;

        public DyzkController dyzkController
        {
            get => BattleManager.instance?.GetDyzkController(_playerId);
            set { _playerId = value ? value.playerId : -1; }
        }

        public Dyzk dyzk { get => dyzkController?.dyzk; }

        void Update()
        {
            if (dyzk)
            {
                if (ConfigManager.playerColors && _playerId >= 0 && _playerId < ConfigManager.playerColors.Count)
                {
                    Color col = ConfigManager.playerColors[_playerId];
                    _indicatorArrow.color = new Color(col.r, col.g, col.b, _indicatorArrow.color.a);
                    _controlArrow.color = new Color(col.r, col.g, col.b, _controlArrow.color.a);
                }

                UpdateIndicatorArrow();
                UpdateControlArrow();
            }
        }

        private void UpdateIndicatorArrow()
        {
            Vector3 arrowWorldPos = dyzk.transform.position + Vector3.forward * dyzk.dyzkState.maxRadius * 2.0f;
            Vector3 arrowScreenPoint = Camera.main.WorldToScreenPoint(arrowWorldPos);

            Vector3 screenClampOffset = GetOffsetToScreenClampPosition(arrowScreenPoint);
            arrowScreenPoint += screenClampOffset;

            _indicatorArrow.transform.position = arrowScreenPoint;
        }

        private void UpdateControlArrow()
        {
            Vector3D controlDir3D = dyzk.dyzkState.control;

            float controlStrength;
            controlDir3D.Normalize(out controlStrength);

            bool showControlArrow = controlStrength > 0.0f;
            _controlArrow.gameObject.SetActive(showControlArrow);

            if (!showControlArrow)
            {
                // We're done here (arrow's invisible)
                return;
            }

            const float MIN_CONTROL_ARROW_SCALE = 0.3f;

            Vector3 controlDir = controlDir3D.ToUnityVector();
            Vector3 arrowWorldPos = dyzk.transform.position + controlDir * dyzk.dyzkState.maxRadius * 2.0f;
            Vector3 arrowScreenPoint = Camera.main.WorldToScreenPoint(arrowWorldPos);

            Vector3 screenClampOffset = GetOffsetToScreenClampPosition(arrowScreenPoint);
            arrowScreenPoint += screenClampOffset;

            float rotation = Vector3.SignedAngle(Vector3.forward, controlDir, Vector3.down);            
            float controlArrowScale = controlStrength * (1.0f - MIN_CONTROL_ARROW_SCALE) + MIN_CONTROL_ARROW_SCALE;

            _controlArrow.transform.position = arrowScreenPoint;
            _controlArrow.transform.rotation = Quaternion.Euler(0, 0, rotation);
            _controlArrow.transform.localScale = new Vector3(controlArrowScale, controlArrowScale, controlArrowScale);

            // Change control arrow alpha based on how unaligned it is with velocity
            const float MAX_ARROW_ALPHA = 0.8f;
            const float MIN_ARROW_ALPHA = 0.2f;

            Vector2D control2D = dyzk.dyzkState.control.xy.normal;
            Vector2D clampedVelocity2D = dyzk.dyzkState.velocity.xy;
            float speed;
            clampedVelocity2D.Normalize(out speed);
            if (speed > 1.0f)
            {
                clampedVelocity2D /= speed;
            }

            float controlVelAlignment = control2D.Dot(clampedVelocity2D);
            float alpha = Mathf.Clamp01(MAX_ARROW_ALPHA - controlVelAlignment * (MAX_ARROW_ALPHA - MIN_ARROW_ALPHA));

            Color col = _controlArrow.color;
            col.a = alpha;
            _controlArrow.color = col;
        }

        /// Returns the offset needed to clamp a screen position within a predefined margin
        private Vector3 GetOffsetToScreenClampPosition(Vector3 screenPos)
        {
            const float PAD_FACTOR = 0.05f;
            float hPadding = Screen.width * PAD_FACTOR;
            float vPadding = Screen.height * PAD_FACTOR;
            hPadding = Mathf.Min(hPadding, vPadding);
            vPadding = hPadding;

            Vector3 clampedArrowPoint = new Vector3(
                Mathf.Clamp(screenPos.x, hPadding, Screen.width - hPadding),
                Mathf.Clamp(screenPos.y, vPadding, Screen.height - vPadding),
                screenPos.z);

            return clampedArrowPoint - screenPos;
        }
    }
}