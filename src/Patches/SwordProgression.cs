﻿using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicArchipelago {
    public class SwordProgression {
        private static ManualLogSource Logger = TunicArchipelago.Logger;
        public static void UpgradeSword(int SwordLevel) {
            
            SaveFile.SetInt("randomizer sword progression level", SwordLevel);

            GameObject SwordPresentation = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0].transform.GetChild(9).gameObject;
            if (SwordLevel == 1) {
                //fownd ahn Itehm!
                Inventory.GetItemByName("Stick").Quantity = 1;
                Inventory.GetItemByName("Stick").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Stick").collectionMessage.text = $"fownd ahn Itehm! \"(<#8ddc6e>Lv. 1<#FFFFFF>)\"";

                ItemPresentation.PresentItem(Inventory.GetItemByName("Stick"));
            } else if (SwordLevel == 2) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                SwordPresentation.GetComponent<MeshFilter>().mesh = ModelSwaps.Items["Sword"].GetComponent<MeshFilter>().mesh;
                SwordPresentation.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Sword"].GetComponent<MeshRenderer>().materials;
                SwordPresentation.transform.localScale = new Vector3(1.447f, 1.447f, 1.447f);
                SwordPresentation.transform.localRotation = new Quaternion(-0.2071f, -0.1216f, 0.3247f, -0.9148f);

                Inventory.GetItemByName("Sword").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Sword").collectionMessage.text = $"fownd ahn Itehm! \"(<#e99d4c>Lv. 2<#FFFFFF>)\"";
                Inventory.GetItemByName("Sword").useAlreadyHaveOneMessage = false;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Sword"));
            } else if (SwordLevel == 3) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                SwordPresentation.GetComponent<MeshFilter>().mesh = ModelSwaps.SecondSword.GetComponent<MeshFilter>().mesh;
                SwordPresentation.GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                SwordPresentation.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);
                SwordPresentation.transform.localRotation = new Quaternion(-0.2071f, -0.1216f, 0.3247f, -0.9148f);

                Inventory.GetItemByName("Sword").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Sword").collectionMessage.text = $"\"        ? ? ? (<#ca7be4>Lv. 3<#FFFFFF>)\"";
                Inventory.GetItemByName("Sword").useAlreadyHaveOneMessage = false;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Sword"));
                Inventory.GetItemByName("Level Up - Attack").Quantity += 1;
                TunicArchipelago.Tracker.ImportantItems["Level Up - Attack"] = Inventory.GetItemByName("Level Up - Attack").Quantity;
                EnableSecondSword();
            } else if (SwordLevel >= 4) {
                Inventory.GetItemByName("Sword").Quantity = 1;
                SwordPresentation.GetComponent<MeshFilter>().mesh = ModelSwaps.ThirdSword.GetComponent<MeshFilter>().mesh;
                SwordPresentation.GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                SwordPresentation.transform.localScale = new Vector3(0.175f, 0.175f, 0.175f);
                SwordPresentation.transform.localRotation = new Quaternion(-0.6533f, 0.2706f, -0.2706f, 0.6533f);
                SwordPresentation.transform.localScale = new Vector3(0.175f, 0.175f, 0.175f);

                Inventory.GetItemByName("Sword").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Sword").collectionMessage.text = $"\"        ! ! ! (<#5de7cf>Lv. 4<#FFFFFF>)\"";
                Inventory.GetItemByName("Sword").useAlreadyHaveOneMessage = false;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Sword"));
                Inventory.GetItemByName("Level Up - Attack").Quantity += 1;
                TunicArchipelago.Tracker.ImportantItems["Level Up - Attack"] = Inventory.GetItemByName("Level Up - Attack").Quantity;
                EnableThirdSword();
            }
        }

        public static void ResetSword() {
            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
            GameObject SwordProxy = GameObject.Find(SwordPath);
            if (SwordProxy.GetComponent<MeshFilter>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
            }
            if (SwordProxy.GetComponent<MeshRenderer>() == null) {
                GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
            }
            SwordProxy.AddComponent<MeshFilter>().mesh = ModelSwaps.Items["Sword"].GetComponent<MeshFilter>().mesh;
            SwordProxy.AddComponent<MeshRenderer>().materials = ModelSwaps.Items["Sword"].GetComponent<MeshRenderer>().materials;
            SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 0.7653f, 0f);
            SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.33f, 1.25f, 1f);
            SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.2f, 1.32f, 0.2f);
            if (SwordProxy.transform.childCount == 5) {
                GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
            }
        }

        public static void EnableSecondSword() {
            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
            GameObject SwordProxy = GameObject.Find(SwordPath);
            if (SwordProxy != null) {
                if (SwordProxy.GetComponent<MeshFilter>() != null && SwordProxy.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
                    GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
                }
                if (SwordProxy.transform.childCount >= 3) {
                    SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 1.7f, 0f);
                    SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.66f, 4.16f, 2f);
                    SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.4f, 4.4f, 0.4f);
                }

                if (SwordProxy.transform.childCount == 5) {
                    GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
                }
                GameObject Sword = new GameObject("sword");
                Sword.AddComponent<MeshFilter>().mesh = ModelSwaps.SecondSword.GetComponent<MeshFilter>().mesh;
                Sword.AddComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                Sword.transform.parent = SwordProxy.transform;
                Sword.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
                Sword.transform.localRotation = Quaternion.identity;
                Sword.transform.localPosition = Vector3.zero;
                PlayerCharacterPatches.LoadSecondSword = false;
            } else {
                Logger.LogError("Could not find sword object to replace with Sword Lvl 3!");
            }
        }

        public static void EnableThirdSword() {

            string SwordPath = "_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/";
            GameObject SwordProxy = GameObject.Find(SwordPath);
            if (SwordProxy != null) {
                if(SwordProxy.GetComponent<MeshFilter>() != null && SwordProxy.GetComponent<MeshRenderer>() != null) {
                    GameObject.Destroy(SwordProxy.GetComponent<MeshFilter>());
                    GameObject.Destroy(SwordProxy.GetComponent<MeshRenderer>());
                }
                if (SwordProxy.transform.childCount >= 3) {
                    SwordProxy.transform.GetChild(0).localPosition = new Vector3(0f, 1.85f, 0f);
                    SwordProxy.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(0.66f, 4.36f, 2f);
                    SwordProxy.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(0.4f, 4.6f, 0.4f);
                    SwordProxy.transform.GetChild(1).GetComponent<HitTrigger>().unblockable = true;
                    SwordProxy.transform.GetChild(2).GetComponent<HitTrigger>().unblockable = true;
                }
                if (SwordProxy.transform.childCount == 5) {
                    GameObject.Destroy(SwordProxy.transform.GetChild(4).gameObject);
                }
                GameObject Sword = new GameObject("sword");
                Sword.AddComponent<MeshFilter>().mesh = ModelSwaps.ThirdSword.GetComponent<MeshFilter>().mesh;
                Sword.AddComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                Sword.transform.parent = SwordProxy.transform;
                Sword.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Sword.transform.localRotation = new Quaternion(0.7071f, 0f, 0f, -0.7071f);
                Sword.transform.localPosition = Vector3.zero;
                PlayerCharacterPatches.LoadThirdSword = false;
            } else {
                Logger.LogError("Could not find sword object to replace with Sword Lvl 4!");
            }
        }

        public static bool HitReceiver_ReceiveHit_PrefixPatch(HitReceiver __instance, ref HitType hitType, ref bool unblockable, ref bool isPlayerCharacterMelee) {

            if ((__instance.GetComponent<TuningForkBell>() != null || __instance.GetComponent<PowerSwitch>() != null) && isPlayerCharacterMelee) {
                unblockable = false;
            }

            return true;
        }
    }
}
