using System.Collections.Generic;

namespace BattleDyzx
{
    public class BattleGameDynamics
    {
        public void Tick(BattleGameState state)
        {
            UpdateDyzx(state);
        }

        private void UpdateDyzx(BattleGameState state)
        {
            float dt = state.dynamicsTimeStep;

            // Update physics
            foreach (DyzkState dyzk in state.dyzx)
            {
                dyzk.angle += dyzk.angularVelocity * dt;

                dyzk.position += dyzk.velocity * dt;
                dyzk.velocity += dyzk.acceleration * dt;
                dyzk.velocity *= 0.995f; // friction

                dyzk.normal = state.arena.SampleNormalScaled(dyzk.position.x, dyzk.position.y);
                dyzk.ground = state.arena.SampleElevationScaled(dyzk.position.x, dyzk.position.y);

                // TODO: Consider applying this only to dyzx on the ground
                Vector3D normalForce = dyzk.normal * dyzk.normal.Dot(dyzk.acceleration);
                dyzk.acceleration = state.gravity;
                dyzk.acceleration -= normalForce;

                dyzk.position.z = Math.Max(dyzk.position.z, dyzk.ground);

                dyzk.acceleration += dyzk.control * dyzk.speed;

                dyzk.collisionDebug.isInCollision = false;
            }

            // Detect Collisions
            for (int i = 0; i < state.dyzx.Count; i++)
            {
                for (int j = i + 1; j < state.dyzx.Count; j++)
                {
                    DyzkState dyzkA = state.dyzx[i];
                    DyzkState dyzkB = state.dyzx[j];

                    // In the first phase we just need to check
                    // circle x circle collision on the XY plane
                    float xDist = dyzkA.position.x - dyzkB.position.x;
                    float yDist = dyzkA.position.y - dyzkB.position.y;
                    float distSquared = xDist * xDist + yDist * yDist;

                    float radiiSquared = dyzkA.dyzkData.maxRadius + dyzkB.dyzkData.maxRadius;
                    radiiSquared *= radiiSquared;

                    if (distSquared > radiiSquared)
                    {
                        // No collision the dyzx are far apart
                        continue;
                    }

                    HandleDyzkCollision(dyzkA, dyzkB);
                }
            }
        }

        void HandleDyzkCollision(DyzkState dyzkA, DyzkState dyzkB)
        {
            // Force distribution between the two dyzx,
            // i.e. how much does the velocity of one dyzk affect the other
            // 1.0 - means dyzx are independent (A knocks back B, and B knocks back A)
            // 0.5 - means velocities are shared half-half (half of A's velocity goes back to it)
            // 0.0 - means velocity only affects opponent 
            const float FORCE_DISTRIBUTION = 0.8f;

            // Likewise tangent force distribution is about how much tantial rotation force is shared
            const float TANGENT_FORCE_DISTRIBUTION = 0.8f;

            float radDistance = dyzkA.maxRadius + dyzkB.maxRadius;
            float radRatio = dyzkA.maxRadius / radDistance;
            
            Vector3D hitPoint = dyzkA.position * (1 - radRatio) + dyzkB.position * radRatio;
            Vector3D hitNormal = dyzkB.position - dyzkA.position;

            // We handle collisions mostly in 2D space, it makes things simple and easier to understand for players
            // The third dimensions is mostly there as a way to avoid collision and for cosmetic purposes
            float distance;
            Vector2D hitNormal2D = hitNormal.xy;
            hitNormal2D.Normalize(out distance);

            Vector2D directionA = dyzkA.velocity.xy;
            Vector2D directionB = dyzkB.velocity.xy;

            float speedA, speedB;
            directionA.Normalize(out speedA);
            directionB.Normalize(out speedB);

            Vector2D controlA = dyzkA.control.xy;
            Vector2D controlB = dyzkB.control.xy;

            // How much dyzx are moving towards the collision (1 towards, -1 away)
            float hitMoveRateA = directionA.Dot(hitNormal2D);
            float hitMoveRateB = -directionB.Dot(hitNormal2D);

            // How much the dyzx are pushing control towards the collision (1 towards, -1 away)
            float hitControlRateA = controlA.Dot(hitNormal2D);
            float hitControlRateB = -controlB.Dot(hitNormal2D);

            // Calculate masses
            float totalMass = dyzkA.mass + dyzkB.mass;
            float massRateA = dyzkA.mass / totalMass;
            float massRateB = 1.0f - massRateA;

            // How much dyzkA's momentum is affecting dyzkB relatively and vice-versa
            // This is a function of both if B is moving away, A exerts higher force
            float forceRateA = Math.Clamp01(hitMoveRateA * FORCE_DISTRIBUTION - hitMoveRateB * (1-FORCE_DISTRIBUTION));
            float forceRateB = Math.Clamp01(hitMoveRateB * FORCE_DISTRIBUTION - hitMoveRateA * (1-FORCE_DISTRIBUTION));

            // How much of the two dyzx momentum carries over
            float preservedAmountA = speedA * (1 - forceRateA);
            float preservedAmountB = speedB * (1 - forceRateB);
            Vector2D preservedForceA = directionA * preservedAmountA;
            Vector2D preservedForceB = directionB * preservedAmountB;

            // How much force is applied as knock-back onto the other dyzk
            float knockbackSawSpeed = (dyzkA.saw*dyzkA.saw + dyzkA.saw * dyzkB.saw + dyzkB.saw * dyzkB.saw) * (dyzkA.angularVelocity + dyzkA.angularVelocity) * 0.01f;
            float knockbackAmountA = (speedA + knockbackSawSpeed) * forceRateA * massRateA;
            float knockbackAmountB = (speedB + knockbackSawSpeed) * forceRateB * massRateB;
            Vector2D knockbackForceA = hitNormal2D * knockbackAmountA;
            Vector2D knockbackForceB = hitNormal2D * -knockbackAmountB;

            // How much tangential force to apply (tangential force is generated from spinning)
            // TODO: Tangential direction should be relative to the spin direction
            // TODO: It should also be relative to the radii (converting torque to linear)
            float tangentTerm = (dyzkA.saw + dyzkB.saw) * 0.0001f;
            float tangentAmountA = tangentTerm * massRateA * (dyzkA.angularVelocity * TANGENT_FORCE_DISTRIBUTION + dyzkB.angularVelocity * (1 - TANGENT_FORCE_DISTRIBUTION));
            float tangentAmountB = tangentTerm * massRateB * (dyzkB.angularVelocity * TANGENT_FORCE_DISTRIBUTION + dyzkA.angularVelocity * (1 - TANGENT_FORCE_DISTRIBUTION));
            
            Vector2D tangentDirA = new Vector2D(-hitNormal2D.y, hitNormal2D.x); 
            Vector2D tangentDirB = new Vector2D(hitNormal2D.y, -hitNormal2D.x);
            Vector2D tangentForceA = (tangentDirA * 0.7f - hitNormal2D * 0.3f) * tangentAmountA;
            Vector2D tangentForceB = (tangentDirB * 0.7f + hitNormal2D * 0.3f) * tangentAmountB;

            Vector2D finalForceA = preservedForceA + knockbackForceB + tangentForceB;
            Vector2D finalForceB = preservedForceB + knockbackForceA + tangentForceA;

            dyzkA.velocity = finalForceA;
            dyzkB.velocity = finalForceB;

            float intersectionAmount = radDistance - distance;
            if (intersectionAmount > 0)
            {
                dyzkA.position.x = dyzkA.position.x - hitNormal.x * intersectionAmount;
                dyzkA.position.y = dyzkA.position.y - hitNormal.y * intersectionAmount;
                dyzkB.position.x = dyzkB.position.x + hitNormal.x * intersectionAmount;
                dyzkB.position.y = dyzkB.position.y + hitNormal.y * intersectionAmount;
            }


            // Debug stuff
            if (dyzkA.collisionDebug.isInCollision)
            {                
                dyzkA.collisionDebug.preservedForce += preservedForceA;
                dyzkA.collisionDebug.knockbackForce += knockbackForceB;
                dyzkA.collisionDebug.tangentForce += tangentForceB;
                dyzkA.collisionDebug.finalForce += finalForceA;
            }
            else
            {
                dyzkA.collisionDebug.preservedForce = preservedForceA;
                dyzkA.collisionDebug.knockbackForce = knockbackForceB;
                dyzkA.collisionDebug.tangentForce = tangentForceB;
                dyzkA.collisionDebug.finalForce = finalForceA;
            }

            if (dyzkB.collisionDebug.isInCollision)
            {
                dyzkB.collisionDebug.preservedForce += preservedForceB;
                dyzkB.collisionDebug.knockbackForce += knockbackForceA;
                dyzkB.collisionDebug.tangentForce += tangentForceA;
                dyzkB.collisionDebug.finalForce += finalForceB;
            }
            else
            {
                dyzkB.collisionDebug.preservedForce = preservedForceB;
                dyzkB.collisionDebug.knockbackForce = knockbackForceA;
                dyzkB.collisionDebug.tangentForce = tangentForceA;
                dyzkB.collisionDebug.finalForce = finalForceB;
            }

            dyzkA.collisionDebug.isInCollision = true;
            dyzkB.collisionDebug.isInCollision = true;
        }
    }
}