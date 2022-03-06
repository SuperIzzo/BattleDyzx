using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkController : MonoBehaviour
    {        
        public int controllerId { get; set; }
        public string controllerName { get; set; }
        public Dyzk dyzk { get; private set; }

        void Awake()
        {
            dyzk = GetComponent<Dyzk>();
        }
    }
}
