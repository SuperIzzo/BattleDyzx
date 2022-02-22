using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace BattleDyzx.Test
{
    public class CombatKnockback_TEST
    {
        private Vector3D hitPoint = Vector3D.zero;
        private Vector3D hitNormal = Vector3D.right;
        private BattleGameDynamics battleDynamics = new BattleGameDynamics();

        private DyzkState CreateDefaultDyzk()
        {
            DyzkState dyzk = new DyzkState
            {
                dyzkData = new DyzkData
                {
                    id = -1,            // invalid
                    maxRadius = 0.02f,  // 2cm
                    mass = 0.001f,      // ~10g     
                    saw = 0.1f,         // 10%
                    balance = 1.0f,     // 100%
                },

                angularVelocity = 1000,
            };

            return dyzk;
        }

        protected DyzkState CreateDyzkA()
        {
            DyzkState dyzkA = CreateDefaultDyzk();
            dyzkA.position = hitPoint + Vector3D.left * dyzkA.maxRadius;

            return dyzkA;
        }

        protected DyzkState CreateDyzkB()
        {
            DyzkState dyzkA = CreateDefaultDyzk();
            dyzkA.position = hitPoint + Vector3D.right * dyzkA.maxRadius;

            return dyzkA;
        }

        [Test]
        public void KnockbackShouldBeOppositeToTheHit()
        {
            DyzkState dyzkAttackA = CreateDyzkA();
            DyzkState dyzkAttackB = CreateDyzkB();
            DyzkState dyzkDefenceA = CreateDyzkA();
            DyzkState dyzkDefenceB = CreateDyzkB();

            dyzkAttackA.velocity = Vector3D.right;
            dyzkAttackB.velocity = Vector3D.left;

            battleDynamics.HandleDyzkCollision(dyzkAttackA, dyzkDefenceB);
            battleDynamics.HandleDyzkCollision(dyzkDefenceA, dyzkAttackB);

            float dyzkDefenceADot = dyzkDefenceA.velocity.Dot(Vector3D.left);
            float dyzkDefenceBDot = dyzkDefenceB.velocity.Dot(Vector3D.right);

            Assert.Positive(dyzkDefenceADot, "Defending dyzx A should be knocked back to the left.");
            Assert.Positive(dyzkDefenceBDot, "Defending dyzx B should be knocked back to the right.");
        }

        [Test]
        public void HeavierDyzxApplyMoreKnockback()
        {
            DyzkState dyzkAttackHeavy = CreateDyzkA();
            DyzkState dyzkAttackLight = CreateDyzkA();
            DyzkState dyzkDefenceVsHeavy = CreateDyzkB();
            DyzkState dyzkDefenceVsLight = CreateDyzkB();

            dyzkAttackHeavy.velocity = Vector3D.right;
            dyzkAttackLight.velocity = Vector3D.right;

            dyzkAttackHeavy.mass = dyzkAttackLight.mass * 2.0f; // Twice as heavy as the default            

            battleDynamics.HandleDyzkCollision(dyzkAttackHeavy, dyzkDefenceVsHeavy);
            battleDynamics.HandleDyzkCollision(dyzkAttackLight, dyzkDefenceVsLight);

            float heavyAppliedKnockback = dyzkDefenceVsHeavy.velocity.length;
            float lightAppliedKnockback = dyzkDefenceVsLight.velocity.length;

            Assert.Greater(heavyAppliedKnockback, lightAppliedKnockback, "Heavy dyzx should apply more knockback.");
        }

        [Test]
        public void HeavierDyzxReceiveLessKnockback()
        {
            DyzkState dyzkAttackOnHeavy = CreateDyzkA();
            DyzkState dyzkAttackOnLight = CreateDyzkA();
            DyzkState dyzkDefenceHeavy = CreateDyzkB();
            DyzkState dyzkDefenceLight = CreateDyzkB();

            dyzkAttackOnHeavy.velocity = Vector3D.right;
            dyzkAttackOnLight.velocity = Vector3D.right;

            dyzkDefenceHeavy.mass = dyzkDefenceLight.mass * 2.0f; // Twice as heavy as the default            

            battleDynamics.HandleDyzkCollision(dyzkAttackOnHeavy, dyzkDefenceHeavy);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnLight, dyzkDefenceLight);

            float heavyReceivedKnockback = dyzkDefenceHeavy.velocity.length;
            float lightReceivedKnockback = dyzkDefenceLight.velocity.length;

            Assert.Less(heavyReceivedKnockback, lightReceivedKnockback, "Heavy dyzx should receive less knockback.");
        }

        [Test]
        public void FasterDyzxApplyMoreKnockback()
        {
            DyzkState dyzkAttackFast = CreateDyzkA();
            DyzkState dyzkAttackSlow = CreateDyzkA();
            DyzkState dyzkDefenceVsFast = CreateDyzkB();
            DyzkState dyzkDefenceVsSlow = CreateDyzkB();

            dyzkAttackFast.velocity = Vector3D.right * 2.0f;
            dyzkAttackSlow.velocity = Vector3D.right;

            battleDynamics.HandleDyzkCollision(dyzkAttackFast, dyzkDefenceVsFast);
            battleDynamics.HandleDyzkCollision(dyzkAttackSlow, dyzkDefenceVsSlow);

            float fastAppliedKnockback = dyzkDefenceVsFast.velocity.length;
            float slowAppliedKnockback = dyzkDefenceVsSlow.velocity.length;

            Assert.Greater(fastAppliedKnockback, slowAppliedKnockback, "Fast dyzx should apply more knockback.");
        }

        [Test]
        public void ImbalancedDyzxApplyMoreKnockback()
        {
            DyzkState dyzkAttackImbalanced = CreateDyzkA();
            DyzkState dyzkAttackBalanced = CreateDyzkA();
            DyzkState dyzkDefenceVsImbalanced = CreateDyzkB();
            DyzkState dyzkDefenceVsBalanced = CreateDyzkB();

            dyzkAttackImbalanced.velocity = Vector3D.right;
            dyzkAttackBalanced.velocity = Vector3D.right;

            dyzkAttackImbalanced.balance = 0.5f;
            dyzkAttackBalanced.balance = 1.0f;

            battleDynamics.HandleDyzkCollision(dyzkAttackImbalanced, dyzkDefenceVsImbalanced);
            battleDynamics.HandleDyzkCollision(dyzkAttackBalanced, dyzkDefenceVsBalanced);

            float imbalancedAppliedKnockback = dyzkDefenceVsImbalanced.velocity.length;
            float balancedAppliedKnockback = dyzkDefenceVsBalanced.velocity.length;

            Assert.Greater(imbalancedAppliedKnockback, balancedAppliedKnockback, "Imbalanced dyzx should apply more knockback.");
        }


        [Test]
        public void ImbalancedDyzxReceiveMoreKnockback()
        {
            DyzkState dyzkAttackOnImbalanced = CreateDyzkA();
            DyzkState dyzkAttackOnBalanced = CreateDyzkA();
            DyzkState dyzkDefenceImbalanced = CreateDyzkB();
            DyzkState dyzkDefenceBalanced = CreateDyzkB();

            dyzkAttackOnImbalanced.velocity = Vector3D.right;
            dyzkAttackOnBalanced.velocity = Vector3D.right;

            dyzkDefenceImbalanced.balance = 0.5f;
            dyzkDefenceBalanced.balance = 1.0f;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnImbalanced, dyzkDefenceImbalanced);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnBalanced, dyzkDefenceBalanced);

            float imbalancedReceivedKnockback = dyzkDefenceImbalanced.velocity.length;
            float balancedReceivedKnockback = dyzkDefenceBalanced.velocity.length;

            Assert.Greater(imbalancedReceivedKnockback, balancedReceivedKnockback, "Imbalanced dyzx should receive more knockback.");
        }

        [Test]
        public void DyzxApplyMoreKnockbackTheMoreTheyMoveTowardsHit()
        {
            DyzkState dyzkAttackMovingTowards = CreateDyzkA();
            DyzkState dyzkAttackMovingSemiTowards = CreateDyzkA();
            DyzkState dyzkAttackMovingPerpendicular = CreateDyzkA();
            DyzkState dyzkDefenceVsMovingTowards = CreateDyzkB();
            DyzkState dyzkDefenceVsMovingSemiTowards = CreateDyzkB();
            DyzkState dyzkDefenceVsMovingPerpendicular = CreateDyzkB();

            dyzkAttackMovingTowards.velocity = Vector3D.right;
            dyzkAttackMovingSemiTowards.velocity = (Vector3D.right + Vector3D.up).normal;
            dyzkAttackMovingPerpendicular.velocity = Vector3D.up;

            battleDynamics.HandleDyzkCollision(dyzkAttackMovingTowards, dyzkDefenceVsMovingTowards);
            battleDynamics.HandleDyzkCollision(dyzkAttackMovingSemiTowards, dyzkDefenceVsMovingSemiTowards);
            battleDynamics.HandleDyzkCollision(dyzkAttackMovingPerpendicular, dyzkDefenceVsMovingPerpendicular);

            float movingTowardsReceivedKnockback = dyzkDefenceVsMovingTowards.velocity.length;
            float movingSemiTowardsReceivedKnockback = dyzkDefenceVsMovingSemiTowards.velocity.length;
            float movingPerpendicularReceivedKnockback = dyzkDefenceVsMovingPerpendicular.velocity.length;

            Assert.Greater(movingSemiTowardsReceivedKnockback, movingPerpendicularReceivedKnockback, "Dyzx somewhat towards hit should apply knockback than perpendicular.");
            Assert.Greater(movingTowardsReceivedKnockback, movingSemiTowardsReceivedKnockback, "Dyzx moving directly towards hit should apply most knockback.");
        }

        [Test]
        public void DyzxMovingTowardsHitReceiveLessKnockbackAndViceVersa()
        {
            DyzkState dyzkAttackOnMovingTowards = CreateDyzkA();
            DyzkState dyzkAttackOnMovingAway = CreateDyzkA();
            DyzkState dyzkAttackOnStill = CreateDyzkA();
            DyzkState dyzkDefenceMovingTowards = CreateDyzkB();
            DyzkState dyzkDefenceMovingAway = CreateDyzkB();
            DyzkState dyzkDefenceStill = CreateDyzkB();

            dyzkAttackOnMovingTowards.velocity = Vector3D.right;
            dyzkAttackOnMovingAway.velocity = Vector3D.right;
            dyzkAttackOnStill.velocity = Vector3D.right;

            dyzkDefenceMovingTowards.velocity = Vector3D.left * 0.5f;
            dyzkDefenceMovingAway.velocity = Vector3D.right * 0.5f;
            dyzkDefenceStill.velocity = Vector3D.zero;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnMovingTowards, dyzkDefenceMovingTowards);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnMovingAway, dyzkDefenceMovingAway);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnStill, dyzkDefenceStill);

            float movingTowardsReceivedKnockback = dyzkDefenceMovingTowards.velocity.length;
            float movingAwayReceivedKnockback = dyzkDefenceMovingAway.velocity.length;
            float stillReceivedKnockback = dyzkDefenceStill.velocity.length;

            Assert.Less(movingTowardsReceivedKnockback, stillReceivedKnockback, "Dyzx moving towards hit should receive less knockback.");
            Assert.Greater(movingAwayReceivedKnockback, stillReceivedKnockback, "Dyzx moving away from hit should receive more knockback.");
        }

        [Test]
        public void DyzxControllingTowardsHitReceiveLessKnockbackAndViceVersa()
        {
            DyzkState dyzkAttackOnControlTowards = CreateDyzkA();
            DyzkState dyzkAttackOnControlAway = CreateDyzkA();
            DyzkState dyzkAttackOnNoControl = CreateDyzkA();
            DyzkState dyzkDefenceControlTowards = CreateDyzkB();
            DyzkState dyzkDefenceControlAway = CreateDyzkB();
            DyzkState dyzkDefenceNoControl = CreateDyzkB();

            dyzkAttackOnControlTowards.velocity = Vector3D.right;
            dyzkAttackOnControlAway.velocity = Vector3D.right;
            dyzkAttackOnNoControl.velocity = Vector3D.right;

            dyzkDefenceControlTowards.control = Vector3D.left;
            dyzkDefenceControlAway.control = Vector3D.right;
            dyzkDefenceNoControl.control = Vector3D.zero;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnControlTowards, dyzkDefenceControlTowards);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnControlAway, dyzkDefenceControlAway);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnNoControl, dyzkDefenceNoControl);

            float controlTowardsReceivedKnockback = dyzkDefenceControlTowards.velocity.length;
            float controlAwayReceivedKnockback = dyzkDefenceControlAway.velocity.length;
            float noControlReceivedKnockback = dyzkDefenceNoControl.velocity.length;

            Assert.Less(controlTowardsReceivedKnockback, noControlReceivedKnockback, "Dyzx pushing control towards hit should receive less knockback.");
            Assert.Greater(controlAwayReceivedKnockback, noControlReceivedKnockback, "Dyzx pushing control away from hit should receive more knockback.");
        }

        [Test]
        public void DyzxControllingAwayFromHitControlKnockbackDirection()
        {
            DyzkState dyzkAttackOnControlTowards = CreateDyzkA();
            DyzkState dyzkAttackOnControlAway = CreateDyzkA();
            DyzkState dyzkAttackOnNoControl = CreateDyzkA();
            DyzkState dyzkDefenceControlTowards = CreateDyzkB();
            DyzkState dyzkDefenceControlAway = CreateDyzkB();
            DyzkState dyzkDefenceNoControl = CreateDyzkB();

            dyzkAttackOnControlTowards.velocity = Vector3D.right;
            dyzkAttackOnControlAway.velocity = Vector3D.right;
            dyzkAttackOnNoControl.velocity = Vector3D.right;

            // Set rotation to 0, to ignore the effects of tangential forces
            // because this is easier than factoring it (tangential force is small)
            dyzkAttackOnControlTowards.RPM = 0;
            dyzkAttackOnControlAway.RPM = 0;
            dyzkAttackOnNoControl.RPM = 0;
            dyzkDefenceControlTowards.RPM = 0;
            dyzkDefenceControlAway.RPM = 0;
            dyzkDefenceNoControl.RPM = 0;

            Vector3D controlUpTowards = (Vector3D.left + Vector3D.up).normal;
            Vector3D controlUpAway = (Vector3D.right + Vector3D.up).normal;
            dyzkDefenceControlTowards.control = controlUpTowards;
            dyzkDefenceControlAway.control = controlUpAway;
            dyzkDefenceNoControl.control = Vector3D.zero;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnControlTowards, dyzkDefenceControlTowards);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnControlAway, dyzkDefenceControlAway);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnNoControl, dyzkDefenceNoControl);

            float controlTowardsDot = dyzkDefenceControlTowards.velocity.normal.Dot(Vector3D.up);
            float controlAwayDot = dyzkDefenceControlAway.velocity.normal.Dot(Vector3D.up);
            float noControlDot = dyzkDefenceNoControl.velocity.normal.Dot(Vector3D.up);

            Assert.Greater(controlAwayDot, controlTowardsDot, "Dyzx pushing control away from hit should control the knockback direction more than pushing control at it.");
            Assert.Greater(controlAwayDot, noControlDot, "Dyzx pushing control away from hit should control the knockback direction more than not pushing.");
            Assert.AreEqual(controlTowardsDot, noControlDot, "Not pushing control away from hit should not have much effect on direction");
        }

        [Test]
        public void SameSpinDyzxReceiveMoreKnockbackThanOppositeSpinDyzx()
        {
            DyzkState dyzkAttackCWOnCW = CreateDyzkA();
            DyzkState dyzkAttackCWOnCCW = CreateDyzkA();
            DyzkState dyzkDefenceCW = CreateDyzkB();
            DyzkState dyzkDefenceCCW = CreateDyzkB();

            dyzkAttackCWOnCW.velocity = Vector3D.right;
            dyzkAttackCWOnCCW.velocity = Vector3D.right;

            dyzkAttackCWOnCW.RPM = 1000;
            dyzkAttackCWOnCCW.RPM = 1000;
            dyzkDefenceCW.RPM = 1000;
            dyzkDefenceCCW.RPM = -1000;

            battleDynamics.HandleDyzkCollision(dyzkAttackCWOnCW, dyzkDefenceCW);
            battleDynamics.HandleDyzkCollision(dyzkAttackCWOnCCW, dyzkDefenceCCW);

            // Note for attackers we need to factor in direction as they have prior velocity (preserved momentum)
            float attackerSameSpinReceivedKnockback = dyzkAttackCWOnCW.velocity.length * dyzkAttackCWOnCW.velocity.normal.Dot(Vector3D.left);
            float attackerDifferentSpinReceivedKnockback = dyzkAttackCWOnCCW.velocity.length * dyzkAttackCWOnCCW.velocity.normal.Dot(Vector3D.left);
            float defenderSameSpinKnockback = dyzkDefenceCW.velocity.length;
            float defenderDifferentSpinReceivedKnockback = dyzkDefenceCCW.velocity.length;

            Assert.Greater(attackerSameSpinReceivedKnockback, attackerDifferentSpinReceivedKnockback, "Same spin dyzx should receive more knockback (attacking).");
            Assert.Greater(defenderSameSpinKnockback, defenderDifferentSpinReceivedKnockback, "Same spin dyzx should receive more knockback when (defending).");
        }

        [Test]
        public void HigherSawFactorIncreasesKnockbackForBothDyzx()
        {
            // H - high saw; L - low saw;  LH - low saw attack on high saw defence
            DyzkState dyzkAttackLL = CreateDyzkA();
            DyzkState dyzkAttackLH = CreateDyzkA();
            DyzkState dyzkAttackHL = CreateDyzkA();
            DyzkState dyzkAttackHH = CreateDyzkA();

            DyzkState dyzkDefenceLL = CreateDyzkB();
            DyzkState dyzkDefenceLH = CreateDyzkB();
            DyzkState dyzkDefenceHL = CreateDyzkB();
            DyzkState dyzkDefenceHH = CreateDyzkB();

            dyzkAttackLL.velocity = Vector3D.right;
            dyzkAttackLH.velocity = Vector3D.right;
            dyzkAttackHL.velocity = Vector3D.right;
            dyzkAttackHH.velocity = Vector3D.right;

            dyzkAttackLL.saw = 0.1f; dyzkDefenceLL.saw = 0.1f;
            dyzkAttackLH.saw = 0.1f; dyzkDefenceLH.saw = 0.5f;
            dyzkAttackHL.saw = 0.5f; dyzkDefenceHL.saw = 0.1f;
            dyzkAttackHH.saw = 0.5f; dyzkDefenceHH.saw = 0.5f;

            battleDynamics.HandleDyzkCollision(dyzkAttackLL, dyzkDefenceLL);
            battleDynamics.HandleDyzkCollision(dyzkAttackLH, dyzkDefenceLH);
            battleDynamics.HandleDyzkCollision(dyzkAttackHL, dyzkDefenceHL);
            battleDynamics.HandleDyzkCollision(dyzkAttackHH, dyzkDefenceHH);

            // Note for attackers we need to factor in direction as they have prior velocity (preserved momentum)
            float attackKnockbackLL = dyzkAttackLL.velocity.length * dyzkAttackLL.velocity.normal.Dot(Vector3D.left);
            float attackKnockbackLH = dyzkAttackLH.velocity.length * dyzkAttackLH.velocity.normal.Dot(Vector3D.left);
            float attackKnockbackHL = dyzkAttackHL.velocity.length * dyzkAttackHL.velocity.normal.Dot(Vector3D.left);
            float attackKnockbackHH = dyzkAttackHH.velocity.length * dyzkAttackHH.velocity.normal.Dot(Vector3D.left);

            float defenceKnockbackLL = dyzkDefenceLL.velocity.length;
            float defenceKnockbackLH = dyzkDefenceLH.velocity.length;
            float defenceKnockbackHL = dyzkDefenceHL.velocity.length;
            float defenceKnockbackHH = dyzkDefenceHH.velocity.length;

            Assert.Greater(attackKnockbackLH, attackKnockbackLL, "High saw factor defence increases applied knockback to the attacker.");
            Assert.Greater(defenceKnockbackLH, defenceKnockbackLL, "High saw factor defence increases received knockback for the defender.");
            Assert.Greater(attackKnockbackHL, attackKnockbackLL, "High saw factor attack increases received knockback for the attacker.");
            Assert.Greater(defenceKnockbackHL, defenceKnockbackLL, "High saw factor attack increases applied knockback to the defender.");

            Assert.Greater(attackKnockbackHH, attackKnockbackLH, "High saw factor attack and defence increases received knockback for the attacker (LH).");
            Assert.Greater(attackKnockbackHH, attackKnockbackHL, "High saw factor attack and defence increases received knockback for the attacker (HL).");
            Assert.Greater(defenceKnockbackHH, defenceKnockbackLH, "High saw factor attack and defence increases applied knockback to the defender (LH).");
            Assert.Greater(defenceKnockbackHH, defenceKnockbackHL, "High saw factor attack and defence increases applied knockback to the defender (HL).");
        }

        [Test]
        public void SpinDyzxApplyTangentialForce()
        {
            DyzkState dyzkAttackSpinningCW = CreateDyzkA();
            DyzkState dyzkAttackSpinningCCW = CreateDyzkA();
            DyzkState dyzkAttackNotSpinning = CreateDyzkA();

            DyzkState dyzkDefenceVsSpinningCW = CreateDyzkB();
            DyzkState dyzkDefenceVsSpinningCCW = CreateDyzkB();
            DyzkState dyzkDefenceVsNotSpinning = CreateDyzkB();

            dyzkAttackSpinningCW.velocity = Vector3D.right;
            dyzkAttackSpinningCCW.velocity = Vector3D.right;
            dyzkAttackNotSpinning.velocity = Vector3D.right;

            dyzkAttackSpinningCW.RPM = 2000;
            dyzkAttackSpinningCCW.RPM = -2000;
            dyzkAttackNotSpinning.RPM = 0;

            dyzkDefenceVsSpinningCW.RPM = 0;
            dyzkDefenceVsSpinningCCW.RPM = 0;
            dyzkDefenceVsNotSpinning.RPM = 0;

            battleDynamics.HandleDyzkCollision(dyzkAttackSpinningCW, dyzkDefenceVsSpinningCW);
            battleDynamics.HandleDyzkCollision(dyzkAttackSpinningCCW, dyzkDefenceVsSpinningCCW);
            battleDynamics.HandleDyzkCollision(dyzkAttackNotSpinning, dyzkDefenceVsNotSpinning);

            float cwDownTangentKnockback = dyzkAttackSpinningCW.velocity.Dot(Vector3D.down);
            float noSpinDownTangentKnockback = dyzkAttackNotSpinning.velocity.Dot(Vector3D.down);

            float ccwUpTangentKnockback = dyzkAttackSpinningCCW.velocity.Dot(Vector3D.up);
            float noSpinUpTangentKnockback = dyzkAttackNotSpinning.velocity.Dot(Vector3D.up);

            Assert.Greater(cwDownTangentKnockback, noSpinDownTangentKnockback, "CW spinning dyzk should apply a bit of CW knockback when attacking.");
            Assert.Greater(ccwUpTangentKnockback, noSpinUpTangentKnockback, "CCW spinning dyzk should apply a bit of CCW knockback when attacking.");
        }

        [Test]
        public void FastSpinDyzxApplyMoreTangentialForce()
        {
            DyzkState dyzkAttackSpinning3000RPM = CreateDyzkA();
            DyzkState dyzkAttackSpinning1000RPM = CreateDyzkA();
            DyzkState dyzkDefenceVs3000RPM = CreateDyzkB();
            DyzkState dyzkDefenceVs1000RPM = CreateDyzkB();

            dyzkAttackSpinning3000RPM.velocity = Vector3D.right;
            dyzkAttackSpinning1000RPM.velocity = Vector3D.right;

            dyzkAttackSpinning3000RPM.RPM = 3000;
            dyzkAttackSpinning1000RPM.RPM = 1000;
            dyzkDefenceVs3000RPM.RPM = 0;
            dyzkDefenceVs1000RPM.RPM = 0;

            battleDynamics.HandleDyzkCollision(dyzkAttackSpinning3000RPM, dyzkDefenceVs3000RPM);
            battleDynamics.HandleDyzkCollision(dyzkAttackSpinning1000RPM, dyzkDefenceVs1000RPM);

            float speed3000RPM = dyzkDefenceVs3000RPM.velocity.length;
            float speed1000RPM = dyzkDefenceVs1000RPM.velocity.length;

            Assert.Greater(speed3000RPM, speed1000RPM, "Faster spinning dyzk should apply more tangent force.");
        }
    }
}