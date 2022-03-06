using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class DyzkAIController : DyzkController
    {
        private Dyzk targetDyzk;

        private float dyzkTargetTimer;
        private float dyzkTargetChangeTimeMin = 1;
        private float dyzkTargetChangeTimeMax = 10;

        private void Update()
        {
            dyzkTargetTimer -= Time.deltaTime;
            if(dyzkTargetTimer <= 0)
            {
                dyzkTargetTimer = Random.Range(dyzkTargetChangeTimeMin, dyzkTargetChangeTimeMax);
                Dyzk[] dyzx = FindObjectsOfType<Dyzk>();

                targetDyzk = null;
                while (dyzx.Length > 0 && (targetDyzk == null || targetDyzk == dyzk))
                {
                    targetDyzk = dyzx[Random.Range(0, dyzx.Length)];
                }
            }

            if (targetDyzk)
            {
                Vector3D direction = targetDyzk.dyzkState.position - dyzk.dyzkState.position;
                direction.z = 0;
                direction.Normalize();

                dyzk.SetHorizontaInput(direction.x);
                dyzk.SetVerticalInput(direction.y);
            }
        }
    }
}