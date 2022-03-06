using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkPlayerController : DyzkController
    {        
        public int gamepadId
        {
            get => _gamepadId;
            set
            {
                _gamepadId = value;
                UpdateInputControls();
            }
        }
        private int _gamepadId = -1;
        private string horizontalAxis;
        private string verticalAxis;

        void Start()
        {
            UpdateInputControls();
        }

        void UpdateInputControls()
        {
            string playerPrefix = "P" + (gamepadId + 1) + "_";
            horizontalAxis = playerPrefix + "Horizontal";
            verticalAxis = playerPrefix + "Vertical";
        }

        void Update()
        {
            if (gamepadId >= 0)
            {
                dyzk.SetHorizontaInput(Input.GetAxis(horizontalAxis));
                dyzk.SetVerticalInput(Input.GetAxis(verticalAxis));
            }
        }
    }
}
