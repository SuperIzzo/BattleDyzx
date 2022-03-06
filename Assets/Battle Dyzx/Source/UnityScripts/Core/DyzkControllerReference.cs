using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkControllerReference : MonoBehaviour
    {
        [SerializeField]
        private int _dyzkControllerId = -1;

        public int dyzkControllerId
        {
            get => _dyzkControllerId;
            set { _dyzkControllerId = value; }
        }

        public DyzkController dyzkController 
        { 
            get => BattleManager.instance?.GetDyzkController(_dyzkControllerId);
            set { _dyzkControllerId = value ? value.controllerId : -1; }
        }

        public Dyzk dyzk => dyzkController?.dyzk;
    }
}