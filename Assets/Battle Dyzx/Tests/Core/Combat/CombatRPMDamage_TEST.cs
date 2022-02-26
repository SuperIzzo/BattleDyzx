using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace BattleDyzx.Test
{
    public class CombatRPMDamage_TEST : CombatBase_TEST
    {
        [Test]
        public void HighSawFactorDealsMoreDamage()
        {
            DyzkState dyzkAttackHighSaw = CreateDyzkA();
            DyzkState dyzkAttackLowSaw = CreateDyzkA();
            DyzkState dyzkDefenceVsHighSaw = CreateDyzkB();
            DyzkState dyzkDefenceVsLowSaw = CreateDyzkB();

            dyzkAttackHighSaw.velocity = Vector3D.right;
            dyzkAttackLowSaw.velocity = Vector3D.right;

            dyzkAttackHighSaw.saw = 0.5f;
            dyzkAttackLowSaw.saw = 0.1f;

            battleDynamics.HandleDyzkCollision(dyzkAttackHighSaw, dyzkDefenceVsHighSaw);
            battleDynamics.HandleDyzkCollision(dyzkAttackLowSaw, dyzkDefenceVsLowSaw);

            float defenceRPMVsHighSaw = Math.Abs(dyzkDefenceVsHighSaw.RPM);
            float defenceRPMVsLowSaw = Math.Abs(dyzkDefenceVsLowSaw.RPM);

            Assert.Less(defenceRPMVsHighSaw, defenceRPMVsLowSaw, "High saw factor should deal more RPM damage.");
        }

        [Test]
        public void LowSawFactorReceivesLessDamage()
        {
            DyzkState dyzkAttackOnHighSaw = CreateDyzkA();
            DyzkState dyzkAttackOnLowSaw = CreateDyzkA();
            DyzkState dyzkDefenceHighSaw = CreateDyzkB();
            DyzkState dyzkDefenceLowSaw = CreateDyzkB();

            dyzkAttackOnHighSaw.velocity = Vector3D.right;
            dyzkAttackOnLowSaw.velocity = Vector3D.right;

            dyzkDefenceHighSaw.saw = 0.5f;
            dyzkDefenceLowSaw.saw = 0.1f;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnHighSaw, dyzkDefenceHighSaw);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnLowSaw, dyzkDefenceLowSaw);

            float defenceRPMHighSaw = Math.Abs(dyzkDefenceHighSaw.RPM);
            float defenceRPMLowSaw = Math.Abs(dyzkDefenceLowSaw.RPM);

            Assert.Less(defenceRPMHighSaw, defenceRPMLowSaw, "High saw factor dyzx should receive more RPM damage.");
        }

        [Test]
        public void HighSawFactorDealsMoreDamageThanItReceives()
        {
            DyzkState dyzkAttack = CreateDyzkA();
            DyzkState dyzkDefence = CreateDyzkB();

            dyzkAttack.velocity = Vector3D.right;
            dyzkDefence.velocity = Vector3D.left;

            dyzkAttack.saw = 0.5f;
            dyzkDefence.saw = 0.1f;

            battleDynamics.HandleDyzkCollision(dyzkAttack, dyzkDefence);

            float attackRPM = Math.Abs(dyzkAttack.RPM);
            float defenceRPM = Math.Abs(dyzkDefence.RPM);

            Assert.Greater(attackRPM, defenceRPM, "High saw dyzk should not receive back less damage than it deals.");
        }

        [Test]
        public void DamageIsDeductedCorrectlyForDyzxWithCCWSpin()
        {
            DyzkState dyzkAttackHighSaw = CreateDyzkA();
            DyzkState dyzkAttackLowSaw = CreateDyzkA();
            DyzkState dyzkDefenceVsHighSaw = CreateDyzkB();
            DyzkState dyzkDefenceVsLowSaw = CreateDyzkB();

            dyzkAttackHighSaw.velocity = Vector3D.right;
            dyzkAttackLowSaw.velocity = Vector3D.right;

            dyzkAttackHighSaw.saw = 1.6f;
            dyzkAttackLowSaw.saw = 0.2f;

            dyzkDefenceVsHighSaw.RPM = -1230;
            dyzkDefenceVsLowSaw.RPM = -1230;

            battleDynamics.HandleDyzkCollision(dyzkAttackHighSaw, dyzkDefenceVsHighSaw);
            battleDynamics.HandleDyzkCollision(dyzkAttackLowSaw, dyzkDefenceVsLowSaw);

            float defenceRPMVsHighSaw = Math.Abs(dyzkDefenceVsHighSaw.RPM);
            float defenceRPMVsLowSaw = Math.Abs(dyzkDefenceVsLowSaw.RPM);

            Assert.Less(defenceRPMVsHighSaw, defenceRPMVsLowSaw, "RPM damage should 'increase' RPM of dyzx with CCW (negative) spin.");
        }

        [Test]
        public void HeavierDyzxApplyMoreDamage()
        {
            DyzkState dyzkAttackHeavy = CreateDyzkA();
            DyzkState dyzkAttackLight = CreateDyzkA();
            DyzkState dyzkDefenceVsHeavy = CreateDyzkB();
            DyzkState dyzkDefenceVsLight = CreateDyzkB();

            dyzkAttackHeavy.velocity = Vector3D.right;
            dyzkAttackLight.velocity = Vector3D.right;

            dyzkAttackHeavy.mass = dyzkAttackLight.mass * 2.0f;

            battleDynamics.HandleDyzkCollision(dyzkAttackHeavy, dyzkDefenceVsHeavy);
            battleDynamics.HandleDyzkCollision(dyzkAttackLight, dyzkDefenceVsLight);

            float defenceRPMVsHeavy = Math.Abs(dyzkDefenceVsHeavy.RPM);
            float defenceRPMVsLight = Math.Abs(dyzkDefenceVsLight.RPM);

            Assert.Less(defenceRPMVsHeavy, defenceRPMVsLight, "Heavy dyzx should deal more RPM damage.");
        }

        [Test]
        public void FasterMovingDyzxApplyMoreDamage()
        {
            DyzkState dyzkAttackFast = CreateDyzkA();
            DyzkState dyzkAttackSlow = CreateDyzkA();
            DyzkState dyzkDefenceVsFast = CreateDyzkB();
            DyzkState dyzkDefenceVsSlow = CreateDyzkB();

            dyzkAttackFast.velocity = Vector3D.right * 3.0f;
            dyzkAttackSlow.velocity = Vector3D.right;

            battleDynamics.HandleDyzkCollision(dyzkAttackFast, dyzkDefenceVsFast);
            battleDynamics.HandleDyzkCollision(dyzkAttackSlow, dyzkDefenceVsSlow);

            float defenceRPMVsFast = Math.Abs(dyzkDefenceVsFast.RPM);
            float defenceRPMVsSlow = Math.Abs(dyzkDefenceVsSlow.RPM);

            Assert.Less(defenceRPMVsFast, defenceRPMVsSlow, "Fast dyzx should deal more RPM damage.");
        }

        [Test]
        public void TangentHitDealsMoreDamageThanHeadOnHit()
        {
            DyzkState dyzkAttackTangent = CreateDyzkA();
            DyzkState dyzkAttackHeadOn = CreateDyzkA();
            DyzkState dyzkDefenceVsTangent = CreateDyzkB();
            DyzkState dyzkDefenceVsHeadOn = CreateDyzkB();

            dyzkAttackTangent.velocity = Vector3D.up;
            dyzkAttackHeadOn.velocity = Vector3D.right;

            battleDynamics.HandleDyzkCollision(dyzkAttackTangent, dyzkDefenceVsTangent);
            battleDynamics.HandleDyzkCollision(dyzkAttackHeadOn, dyzkDefenceVsHeadOn);

            float defenceRPMVsTangent = Math.Abs(dyzkDefenceVsTangent.RPM);
            float defenceRPMVsHeadOn = Math.Abs(dyzkDefenceVsHeadOn.RPM);

            Assert.Less(defenceRPMVsTangent, defenceRPMVsHeadOn, "Tangent hitting dyzx should deal more RPM damage than direct hit.");
        }

        [Test]
        public void RPMDamageDoesNotOvershootZero()
        {
            DyzkState dyzkAttackOnCW = CreateDyzkA();
            DyzkState dyzkAttackOnCCW = CreateDyzkA();
            DyzkState dyzkDefenceCW = CreateDyzkB();
            DyzkState dyzkDefenceCCW = CreateDyzkB();

            dyzkAttackOnCW.velocity = Vector3D.right;
            dyzkAttackOnCCW.velocity = Vector3D.right;

            dyzkDefenceCW.RPM = 1;
            dyzkDefenceCCW.RPM = -1;

            battleDynamics.HandleDyzkCollision(dyzkAttackOnCW, dyzkDefenceCW);
            battleDynamics.HandleDyzkCollision(dyzkAttackOnCCW, dyzkDefenceCCW);

            float defenceRPMCW = dyzkDefenceCW.RPM;
            float defenceRPMCCW = dyzkDefenceCCW.RPM;

            Assert.AreEqual(0.0f, defenceRPMCW, "Enough damage on CW spinning dyzk should bring it down to 0 and not overshoot.");
            Assert.AreEqual(0.0f, defenceRPMCCW, "Enough damage on CCW spinning dyzk should bring it up to 0 and not overshoot.");
        }

        [Test]
        public void RPMReducesOverTime()
        {
            DyzkState dyzk = CreateDyzkA();

            float preUpdateRPM = Math.Abs(dyzk.RPM);
            battleDynamics.UpdateDyzk(dyzk, 1.0f);
            float postUpdateRPM = Math.Abs(dyzk.RPM);

            Assert.Less(postUpdateRPM, preUpdateRPM, "Dyzk RPM should reduce over time.");
        }

        [Test]
        public void RPMReducesMoreOverMoreTime()
        {
            DyzkState dyzk1Step1 = CreateDyzkA();
            DyzkState dyzk1Step2 = CreateDyzkA();
            DyzkState dyzk2Step1 = CreateDyzkA();

            battleDynamics.UpdateDyzk(dyzk1Step1, 1.0f);
            battleDynamics.UpdateDyzk(dyzk1Step2, 2.0f);
            battleDynamics.UpdateDyzk(dyzk2Step1, 1.0f);
            battleDynamics.UpdateDyzk(dyzk2Step1, 1.0f);

            float dyzk1Step1PostUpdateRPM = Math.Abs(dyzk1Step1.RPM);
            float dyzk1Step2PostUpdateRPM = Math.Abs(dyzk1Step2.RPM);
            float dyzk2Step1PostUpdateRPM = Math.Abs(dyzk2Step1.RPM);

            Assert.Less(dyzk1Step2PostUpdateRPM, dyzk1Step1PostUpdateRPM, "Dyzk RPM should reduce more over longer timestep.");
            Assert.Less(dyzk2Step1PostUpdateRPM, dyzk1Step1PostUpdateRPM, "Dyzk RPM should reduce more over more timesteps.");
        }

        [Test]
        public void RPMReducesTheSameOverTheSameAmountOfTimeOverDifferentNumberOfTimesteps()
        {
            DyzkState dyzk1Step2 = CreateDyzkA();
            DyzkState dyzk2Step1 = CreateDyzkA();            

            battleDynamics.UpdateDyzk(dyzk1Step2, 2.0f);
            battleDynamics.UpdateDyzk(dyzk2Step1, 1.0f);
            battleDynamics.UpdateDyzk(dyzk2Step1, 1.0f);

            float dyzk1Step2PostUpdateRPM = Math.Abs(dyzk1Step2.RPM);
            float dyzk2Step1PostUpdateRPM = Math.Abs(dyzk2Step1.RPM);

            Assert.AreEqual(dyzk1Step2PostUpdateRPM, dyzk2Step1PostUpdateRPM, "Dyzk RPM should reduce equally over the same total amount of time.");
        }

        [Test]
        public void DisbalancedDyzxLoseMoreRPMOverTime()
        {
            DyzkState dyzkDisbalanced = CreateDyzkA();
            DyzkState dyzkBalanced = CreateDyzkA();

            dyzkDisbalanced.balance = 0.8f;
            dyzkBalanced.balance = 1.0f;

            battleDynamics.UpdateDyzk(dyzkDisbalanced, 1.0f);
            battleDynamics.UpdateDyzk(dyzkBalanced, 1.0f);

            float dyzkDisbalancedPostUpdateRPM = Math.Abs(dyzkDisbalanced.RPM);
            float dyzkBalancedPostUpdateRPM = Math.Abs(dyzkBalanced.RPM);

            Assert.Less(dyzkDisbalancedPostUpdateRPM, dyzkBalancedPostUpdateRPM, "Disbalanced dyzx should lose RPM faster.");
        }
    }
}