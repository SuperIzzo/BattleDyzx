using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkPlayerController : MonoBehaviour
    {        
        public int controllerId
        {
            get => _controllerId;
            set
            {
                _controllerId = value;
                UpdateControls();
            }
        }
        private int _controllerId = 0;

        private Dyzk dyzk;
        private string horizontalAxis;
        private string verticalAxis;

        void Start()
        {
            dyzk = GetComponent<Dyzk>();
            UpdateControls();
        }

        void UpdateControls()
        {
            string playerPrefix = "P" + (controllerId + 1) + "_";
            horizontalAxis = playerPrefix + "Horizontal";
            verticalAxis = playerPrefix + "Vertical";
        }

        void Update()
        {
            if (controllerId >= 0)
            {
                dyzk.SetHorizontaInput(Input.GetAxis(horizontalAxis));
                dyzk.SetVerticalInput(Input.GetAxis(verticalAxis));
            }
        }
    }
}
