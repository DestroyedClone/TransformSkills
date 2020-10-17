using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.Croco;

namespace MyNameSpace.MyEntityStates
{
    public class Commando_SecondaryDisabled : BasicMeleeAttack
    {
        //public float baseDuration = 0.5f;
        //private float duration;
        //public GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/Swing");
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterDirection.forward = base.GetAimRay().direction;
            this.duration = this.baseDuration / base.attackSpeedStat;

            //base.StartAimMode(aimRay, 2f, false);
            if (base.isAuthority)
            {
                this.hitBoxGroup = base.FindHitBoxGroup(this.hitBoxGroupName);
                if (this.hitBoxGroup)
                {
                    new OverlapAttack
                    {
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        damage = base.characterBody.damage * 2.2f,
                        damageType = DamageType.Generic,
                        hitBoxGroup = base.hitBoxGroup,
                        procCoefficient = 1f,
                        //hitEffectPrefab = this.hitEffectPrefab,
                        isCrit = base.RollCrit(),
                        teamIndex = base.GetTeam(),
                    }.Fire();
                }
            }
        }
        public override void OnExit()
        {
            if (base.isAuthority)
            {
                if (!this.outer.destroying && !this.authorityHasFiredAtAll && this.allowExitFire)
                {
                    this.BeginMeleeAttackEffect();
                }
                if (this.authorityInHitPause)
                {
                    this.AuthorityExitHitPause();
                }
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (string.IsNullOrEmpty(this.mecanimHitboxActiveParameter))
            {
                this.BeginMeleeAttackEffect();
            }
            if (base.isAuthority)
            {
                this.AuthorityFixedUpdate();
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            Chat.AddMessage("Second Priority");
            return InterruptPriority.Skill;
        }

    }
}
