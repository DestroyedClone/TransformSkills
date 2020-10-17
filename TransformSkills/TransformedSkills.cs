using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace MyNameSpace
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(
        "com.DestroyedClone.TransformedSkills",
        "Transformed Skills",
        "1.0.0")]
    [R2APISubmoduleDependency(nameof(LoadoutAPI), nameof(SurvivorAPI), nameof(LanguageAPI))]
    public class ExamplePlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            // myCharacter should either be
            // Resources.Load<GameObject>("prefabs/characterbodies/BanditBody");
            // or BodyCatalog.FindBodyIndex("BanditBody");
            var myCharacter = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");

            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use AssetPlus API for that
            LanguageAPI.Add("MYBANDIT_DESCRIPTION", "cum! cum! fuck!" + Environment.NewLine);

            var mySurvivorDef = new SurvivorDef
            {
                //We're finding the body prefab here,
                bodyPrefab = myCharacter,
                //Description
                descriptionToken = "MYBANDIT_DESCRIPTION",
                //Display 
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
                //Color on select screen
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                //Unlockable name
                unlockableName = "",
            };
            SurvivorAPI.AddSurvivor(mySurvivorDef);

            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use AssetPlus API for that
            LanguageAPI.Add("COMMANDO_PRIMARY_BURSTFIRE_NAME", "Burst Fire");
            LanguageAPI.Add("COMMANDO_PRIMARY_BURSTFIRE_DESCRIPTION", "Unload a burst of 3 bullets for 3x100% damage");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(MyNameSpace.MyEntityStates.Commando_Primary));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = true;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Resources.Load<Sprite>("NotAnActualPath");
            mySkillDef.skillDescriptionToken = "COMMANDO_PRIMARY_BURSTFIRE_DESCRIPTION";
            mySkillDef.skillName = "COMMANDO_PRIMARY_BURSTFIRE_NAME";
            mySkillDef.skillNameToken = "COMMANDO_PRIMARY_BURSTFIRE_NAME";

            LanguageAPI.Add("COMMANDO_SECONDARY_BACKUPSHIV_NAME", "Backup Shiv");
            LanguageAPI.Add("COMMANDO_SECONDARY_BACKUPSHIV_DESCRIPTION", "Slice for 220% damage. Recover 7% health on kill.");

            var commandoSecondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            commandoSecondarySkillDef.activationState = new SerializableEntityStateType(typeof(MyNameSpace.MyEntityStates.Commando_Secondary));
            commandoSecondarySkillDef.activationStateMachineName = "Weapon";
            commandoSecondarySkillDef.baseMaxStock = 1;
            commandoSecondarySkillDef.baseRechargeInterval = 2f;
            commandoSecondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            commandoSecondarySkillDef.canceledFromSprinting = false;
            commandoSecondarySkillDef.fullRestockOnAssign = false;
            commandoSecondarySkillDef.interruptPriority = InterruptPriority.Any;
            commandoSecondarySkillDef.isBullets = false;
            commandoSecondarySkillDef.isCombatSkill = true;
            commandoSecondarySkillDef.mustKeyPress = false;
            commandoSecondarySkillDef.noSprint = false;
            commandoSecondarySkillDef.rechargeStock = 1;
            commandoSecondarySkillDef.requiredStock = 1;
            //commandoSecondarySkillDef.shootDelay = 0.5f;
            commandoSecondarySkillDef.stockToConsume = 1;
            commandoSecondarySkillDef.skillDescriptionToken = "COMMANDO_SECONDARY_BACKUPSHIV_DESCRIPTION";
            commandoSecondarySkillDef.skillName = "COMMANDO_SECONDARY_BACKUPSHIV_NAME";
            commandoSecondarySkillDef.skillNameToken = "COMMANDO_SECONDARY_BACKUPSHIV_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);
            LoadoutAPI.AddSkillDef(commandoSecondarySkillDef);
            //This adds our skilldef. If you don't do this, the skill will not work.

            var skillLocator = myCharacter.GetComponent<SkillLocator>();

            //Note; you can change component.primary to component.secondary , component.utility and component.special
            var skillFamily = skillLocator.primary.skillFamily;
            var skillFamily2 = skillLocator.secondary.skillFamily;
            //var skillFamily3 = skillLocator.utility.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = commandoSecondarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(commandoSecondarySkillDef.skillNameToken, false, null)
            };/*
            Array.Resize(ref skillFamily3.variants, skillFamily3.variants.Length + 1);
            skillFamily.variants[skillFamily3.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = commandoSecondarySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(commandoSecondarySkillDef.skillNameToken, false, null)
            };*/
        }
    }
}