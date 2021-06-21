using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class Dyzk : MonoBehaviour
    {
        public DyzkState dyzkState { get; set; }
        public Spinning spinning { get; private set; }

        public void SetHorizontaInput(float h)
        {
            dyzkState.control.x = h;
        }

        public void SetVerticalInput(float v)
        {
            dyzkState.control.y = v;
        }

        private void Start()
        {
            spinning = GetComponent<Spinning>();
        }

        private void Update()
        {            
            transform.position = new Vector3(dyzkState.position.x, 100, dyzkState.position.y);
            spinning.RPM = dyzkState.RPM;
        }        

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}