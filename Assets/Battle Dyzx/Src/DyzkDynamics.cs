using System.Collections.Generic;

namespace BattleDyzx
{
    public class DyzkDynamics
    {        
        public void Tick( ModelState state )
        {
            UpdateDyzx( state );
        }

        private void UpdateDyzx( ModelState state )
        {
            float dt = state.dynamicsTimeStep;

            foreach( Dyzk dyzk in state.dyzx )
            {
                dyzk.angle += dyzk.angularVelocity * dt;

                dyzk.position += dyzk.velocity * dt;
                dyzk.velocity += dyzk.acceleration * dt;
            }            
        }
    }
}