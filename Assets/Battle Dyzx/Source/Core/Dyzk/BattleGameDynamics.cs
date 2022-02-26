using System.Collections.Generic;

namespace BattleDyzx
{
    public class BattleGameDynamics
    {
        public void Tick(BattleGameState state)
        {
            UpdateGameState(state);
        }

        public void UpdateGameState(BattleGameState state)
        {
            float dt = state.dynamicsTimeStep;

            // Update physics
            foreach (DyzkState dyzk in state.dyzx)
            {
                dyzk.normal = state.arena.SampleNormalScaled(dyzk.position.x, dyzk.position.y);
                dyzk.ground = state.arena.SampleElevationScaled(dyzk.position.x, dyzk.position.y);
                dyzk.gravity = state.gravity;

                UpdateDyzk(dyzk, dt);

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

        public void UpdateDyzk(DyzkState dyzk, float dt)
        {
            //=========================================================================
            // Constants
            //-------------------------
            const float LINEAR_FRICTION = 0.005f;
            const float ANGULAR_ACCELERATION = -0.1f;
            const float DISBALANCE_ANGULAR_ACCELERATION_FACTOR = 2.0f;

            //=========================================================================
            // Linear Integration
            //-------------------------
            dyzk.position += dyzk.velocity * dt;
            dyzk.velocity += dyzk.acceleration * dt;
            dyzk.velocity *= (1.0f - LINEAR_FRICTION);

            // TODO: Consider applying this only to dyzx on the ground
            Vector3D normalForce = dyzk.normal * dyzk.normal.Dot(dyzk.acceleration);
            dyzk.acceleration = dyzk.gravity;
            dyzk.acceleration -= normalForce;

            dyzk.position.z = Math.Max(dyzk.position.z, dyzk.ground);

            dyzk.acceleration += dyzk.control * dyzk.speed;

            //=========================================================================
            // Angular Integration
            //-------------------------
            float disbalanceDecelFactor = 1.0f + (1.0f - dyzk.balance) * (DISBALANCE_ANGULAR_ACCELERATION_FACTOR - 1.0f);
            dyzk.angle += dyzk.angularVelocity * dt;
            dyzk.angularVelocity += ANGULAR_ACCELERATION * disbalanceDecelFactor * dt;
        }

        public void HandleDyzkCollision(DyzkState dyzkA, DyzkState dyzkB)
        {
            //=========================================================================
            // Constants
            //-------------------------

            // Force distribution between the two dyzx,
            // i.e. how much does the velocity of one dyzk affect the other
            // 1.0 - means dyzx are independent (A knocks back B, and B knocks back A)
            // 0.5 - means velocities are shared half-half (half of A's velocity goes back to it)
            // 0.0 - means velocity only affects opponent 
            const float FORCE_DISTRIBUTION = 0.8f;

            // Likewise tangent force distribution is about how much tantial rotation force is shared
            const float TANGENT_FORCE_DISTRIBUTION = 0.8f;

            // Knockback negation is when dyzx are pushing towards the hit, part of the knockback can be negated.
            // Conversly pushing away from the hit increases knockback received from the other dyzk.
            // How is knockback negation distributed between players (1 - defender is sole negator, 0 - attacker is sole negator)
            // and what is the maximum scale (1 - fully negate, 0 - no negation)
            const float MAX_KNOCKBACK_CONTROL_SCALE = 0.5f;

            // How much does angular velocities factor into the knockback
            const float SPIN_KNOCKBACK_FACTOR = 0.01f;

            // How much tangent force to apply
            const float TANGENT_FACTOR = 0.1f;

            // A scale factor to increase or decrease RPM damage based on rotation
            const float RPM_DAMAGE_ROTATION_SCALE = 1.0f;

            // A scale factor to increase or decrease RPM damage based on speed
            const float RPM_DAMAGE_SPEED_SCALE = 1.0f;

            // A scale factor to increase or decrease RPM damage based on direction (tangent)
            const float RPM_DAMAGE_SLASH_SCALE = 1.0f;

            // How to distribute speed damage between attacker and defender
            const float SPEED_DAMAGE_DISTRIBUTION = 0.8f;

            // How to distribute saw damage between attacker and defender
            const float SAW_DAMAGE_DISTRIBUTION = 0.7f;
            

            //=========================================================================
            // Useful variables
            //-------------------------

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

            //=========================================================================
            // Knockback
            //-------------------------

            // How much dyzkA's momentum is affecting dyzkB relatively and vice-versa
            // This is a function of both if B is moving away, A exerts higher force
            float forceRateA = Math.Clamp01(hitMoveRateA * FORCE_DISTRIBUTION - hitMoveRateB * (1 - FORCE_DISTRIBUTION));
            float forceRateB = Math.Clamp01(hitMoveRateB * FORCE_DISTRIBUTION - hitMoveRateA * (1 - FORCE_DISTRIBUTION));

            // How much of the two dyzx momentum carries over
            float preservedAmountA = speedA * (1 - forceRateA);
            float preservedAmountB = speedB * (1 - forceRateB);
            Vector2D preservedForceA = directionA * preservedAmountA;
            Vector2D preservedForceB = directionB * preservedAmountB;

            // The direction of the knock-back (hit normal unless pushing away)
            Vector2D knockbackDirectionA;
            Vector2D knockbackDirectionB;

            if (hitControlRateA > 0)
            {
                knockbackDirectionA = -hitNormal2D;
            }
            else
            {
                knockbackDirectionA = controlA - hitNormal2D;
                knockbackDirectionA.Normalize();
            }

            if (hitControlRateB > 0)
            {
                knockbackDirectionB = hitNormal2D;
            }
            else
            {
                knockbackDirectionB = controlB + hitNormal2D;
                knockbackDirectionB.Normalize();
            }

            // Knock-back control factor
            float hitControlRateScale = (hitControlRateA * Math.Abs(hitControlRateA) - hitControlRateB * Math.Abs(hitControlRateB)) * 0.5f;
            float knockbackControlFactorA = Math.Max(0.0f, 1.0f - hitControlRateScale) * MAX_KNOCKBACK_CONTROL_SCALE;
            float knockbackControlFactorB = Math.Max(0.0f, 1.0f + hitControlRateScale) * MAX_KNOCKBACK_CONTROL_SCALE;

            // How much force is applied as knock-back onto the other dyzk
            float knockbackBalanceFactor = 2.0f - dyzkA.balance - dyzkB.balance;
            float dyzkSawFactor = dyzkA.saw * dyzkA.saw + dyzkA.saw * dyzkB.saw + dyzkB.saw * dyzkB.saw;
            float rotationalKnockback = (knockbackBalanceFactor + dyzkSawFactor) * Math.Abs(dyzkA.angularVelocity + dyzkB.angularVelocity) * SPIN_KNOCKBACK_FACTOR;
            float knockbackAmountA = (speedB * forceRateB + rotationalKnockback) * massRateB * knockbackControlFactorA;
            float knockbackAmountB = (speedA * forceRateA + rotationalKnockback) * massRateA * knockbackControlFactorB;
            Vector2D knockbackForceA = knockbackDirectionA * knockbackAmountA;
            Vector2D knockbackForceB = knockbackDirectionB * knockbackAmountB;

            // How much tangential force to apply (tangential force is generated from spinning)

            // L = m*v*r, but we factor in mass later as a proportion
            float dyzkAAngularMomentum = dyzkA.angularVelocity * dyzkA.maxRadius;
            float dyzkBAngularMomentum = dyzkB.angularVelocity * dyzkB.maxRadius;
            float tangentAmountA = massRateB * (dyzkBAngularMomentum * TANGENT_FORCE_DISTRIBUTION + dyzkAAngularMomentum * (1 - TANGENT_FORCE_DISTRIBUTION)) * TANGENT_FACTOR;
            float tangentAmountB = massRateA * (dyzkAAngularMomentum * TANGENT_FORCE_DISTRIBUTION + dyzkBAngularMomentum * (1 - TANGENT_FORCE_DISTRIBUTION)) * TANGENT_FACTOR;
            Vector2D tangentDirA = new Vector2D(-hitNormal2D.y, hitNormal2D.x);
            Vector2D tangentDirB = -tangentDirA;
            Vector2D tangentForceA = tangentDirA * tangentAmountA;
            Vector2D tangentForceB = tangentDirB * tangentAmountB;

            Vector2D finalForceA = preservedForceA + knockbackForceA + tangentForceA;
            Vector2D finalForceB = preservedForceB + knockbackForceB + tangentForceB;

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

            //=========================================================================
            // RPM Damage
            //-------------------------
            float speedBasedRPMDamageA = (speedB * SPEED_DAMAGE_DISTRIBUTION + speedA * (1 - SPEED_DAMAGE_DISTRIBUTION)) * RPM_DAMAGE_SPEED_SCALE;
            float speedBasedRPMDamageB = (speedA * SPEED_DAMAGE_DISTRIBUTION + speedB * (1 - SPEED_DAMAGE_DISTRIBUTION)) * RPM_DAMAGE_SPEED_SCALE;
            float rotationBasedRPMDamageA = (dyzkB.saw * SAW_DAMAGE_DISTRIBUTION + dyzkA.saw * (1.0f - SAW_DAMAGE_DISTRIBUTION)) * RPM_DAMAGE_ROTATION_SCALE;
            float rotationBasedRPMDamageB = (dyzkA.saw * SAW_DAMAGE_DISTRIBUTION + dyzkB.saw * (1.0f - SAW_DAMAGE_DISTRIBUTION)) * RPM_DAMAGE_ROTATION_SCALE;
            float slashDamageA = (1 - Math.Abs(hitMoveRateB)) * RPM_DAMAGE_SLASH_SCALE;
            float slashDamageB = (1 - Math.Abs(hitMoveRateA)) * RPM_DAMAGE_SLASH_SCALE;
            float finalAngularDamageA = massRateB * (speedBasedRPMDamageA + rotationBasedRPMDamageA + slashDamageA);
            float finalAngularDamageB = massRateA * (speedBasedRPMDamageB + rotationBasedRPMDamageB + slashDamageB);
            float spinA = Math.Sign(dyzkA.angularVelocity);
            float spinB = Math.Sign(dyzkB.angularVelocity);

            dyzkA.angularVelocity -= finalAngularDamageA * spinA;
            dyzkB.angularVelocity -= finalAngularDamageB * spinB;

            // If the dyzx have switched spins, keep them at 0
            if (dyzkA.angularVelocity * spinA < 0.0f) { dyzkA.angularVelocity = 0.0f; }
            if (dyzkB.angularVelocity * spinB < 0.0f) { dyzkB.angularVelocity = 0.0f; }

            //=========================================================================
            // Debug stuff
            //-------------------------            
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