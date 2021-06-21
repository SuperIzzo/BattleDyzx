using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkPlayerController : MonoBehaviour
    {
        private Dyzk dyzk;
        
        void Start()
        {
            dyzk = GetComponent<Dyzk>();
        }

        void Update()
        {
            dyzk.SetHorizontaInput(Input.GetAxis("Horizontal"));
            dyzk.SetVerticalInput(Input.GetAxis("Vertical"));
        }
    }
}
