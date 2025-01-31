﻿using BepInEx.Logging;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicArchipelago.Hints;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class InteractionPatches {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static bool InteractionTrigger_Interact_PrefixPatch(Item item, InteractionTrigger __instance) {
            string InteractionLocation = SceneLoaderPatches.SceneName + " " + __instance.transform.position;

            if (__instance.gameObject.GetComponent<NPC>() != null) {
                if (SceneManager.GetActiveScene().name == "g_elements") {
                    if (Inventory.GetItemByName("Dath Stone").Quantity == 0) {
                        __instance.gameObject.GetComponent<NPC>().script.text = $"I lawst mI mahjik stOn ahnd kahnt gO hOm...---if yoo fInd it, kahn yoo bri^ it too mE?\nit louks lIk #is: [dath]";
                    } else {
                        __instance.gameObject.GetComponent<NPC>().script.text = $"I lawst mI mahjik stOn [dath] ahnd kahnt gO hOm...---... wAt, yoo fownd it! plEz, yooz it now!";
                    }
                }

                if (GhostHints.HintGhosts.ContainsKey(__instance.name) && GhostHints.HexQuestHintLookup.ContainsKey(GhostHints.HintGhosts[__instance.name].Hint)) {
                    SaveFile.SetInt($"randomizer hex quest read {GhostHints.HexQuestHintLookup[GhostHints.HintGhosts[__instance.name].Hint]} hint", 1);
                    ItemStatsHUD.UpdateAbilitySection();
                }

                if (TunicArchipelago.Settings.SendHintsToServer) {
                    GhostHints.CheckForServerHint(__instance.name);
                }
            }
            if (Hints.HintLocations.ContainsKey(InteractionLocation) && Hints.HintMessages.ContainsKey(Hints.HintLocations[InteractionLocation]) && TunicArchipelago.Settings.HeroPathHintsEnabled) {
                LanguageLine Hint = ScriptableObject.CreateInstance<LanguageLine>();
                Hint.text = Hints.HintMessages[Hints.HintLocations[InteractionLocation]];

                GenericMessage.ShowMessage(Hint);
                return false;
            }
            if (__instance.GetComponentInParent<HeroGraveToggle>() != null && TunicArchipelago.Settings.HeroPathHintsEnabled) {
                bool showRelicHint = StateVariable.GetStateVariableByName("randomizer got all 6 grave items").BoolValue;
                HeroGraveHint hint = __instance.GetComponentInParent<HeroGraveToggle>().heroGravehint;

                GenericMessage.ShowMessage(showRelicHint ? hint.RelicHint : hint.PathHint);
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.4, 46.9, 3.0)" && TunicArchipelago.Tracker.ImportantItems["Fairies"] < 10) {
                GenericMessage.ShowMessage($"\"Locked. (10\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.5, 45.0, -0.5)" && TunicArchipelago.Tracker.ImportantItems["Fairies"] < 20) {
                GenericMessage.ShowMessage($"\"Locked. (20\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Overworld Interiors" && __instance.transform.position.ToString() == "(-26.4, 28.9, -46.2)") {
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && (PlayerCharacterPatches.TimeWhenLastChangedDayNight + 3.0f < Time.fixedTime)) {
                    GenericPrompt.ShowPrompt(CycleController.IsNight ? $"wAk fruhm #is drEm?" : $"rEtirn too yor drEm?", (Il2CppSystem.Action)ChangeDayNightHourglass, null);
                }
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Overworld Redux" && __instance.transform.position.ToString() == "(-38.0, 29.0, -55.0)") {
                PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent<MeshRenderer>().materials;
            }
            if ((SceneLoaderPatches.SceneName == "Overworld Redux" && __instance.transform.position.ToString() == "(21.0, 20.0, -122.0)") || 
                (SceneLoaderPatches.SceneName == "Atoll Redux") && __instance.transform.position.ToString() == "(64.0, 4.0, 0.0)") {
                PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key"].GetComponent<MeshRenderer>().materials;
            }
            if (SceneManager.GetActiveScene().name == "frog cave main" && __instance.transform.position.ToString() == "(20.5, 10.1, -32.1)" && !StateVariable.GetStateVariableByName("Granted Cape").BoolValue) {
                GenericMessage.ShowMessage(
                    $"[arrow_down] [arrow_left] [arrow_down] [arrow_right] [arrow_up]\n" +
                    $"[arrow_left] [arrow_up] [arrow_right] [arrow_down] [arrow_left]\n" +
                    $"[arrow_down] [arrow_right] [arrow_up] [arrow_left] [arrow_up]"
                    );
                return false;
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                if (__instance.transform.position.ToString() == "(0.0, 0.0, 0.0)" && SceneLoaderPatches.SceneName == "Spirit Arena" && SaveFile.GetInt(GoldHexagonQuantity) < SaveFile.GetInt(HexagonQuestGoal)) {
                    GenericMessage.ShowMessage($"\"<#EAA615>Sealed Forever.\"");
                    return false;
                }
                if (__instance.transform.position.ToString() == "(2.0, 46.0, 0.0)" && SceneLoaderPatches.SceneName == "Overworld Redux" && !(StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue && StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue)) {
                    GenericMessage.ShowMessage($"\"Sealed Forever.\"");
                    return false;
                }
            }

            return true;
        }

        private static void ChangeDayNightHourglass() {
            PlayerCharacterPatches.TimeWhenLastChangedDayNight = Time.fixedTime;
            bool isNight = CycleController.IsNight;
            if (isNight) {
                CycleController.AnimateSunrise();
            } else {
                CycleController.AnimateSunset();
            }
            CycleController.IsNight = !isNight;
            CycleController.nightStateVar.BoolValue = !isNight;
            GameObject.Find("day night hourglass/rotation/hourglass").GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
        }

        public static void SetupDayNightHourglass() {
            GameObject DayNightSwitch = GameObject.Instantiate(GameObject.Find("Trophy Stuff/TROPHY POINT (1)/"));
            DayNightSwitch.name = "day night hourglass";
            DayNightSwitch.GetComponent<GoldenTrophyRoom>().item = null;
            DayNightSwitch.transform.GetChild(0).gameObject.SetActive(true);
            GameObject Hourglass = DayNightSwitch.transform.GetChild(0).GetChild(0).gameObject;
            Hourglass.GetComponent<MeshFilter>().mesh = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshFilter>().mesh;
            Hourglass.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshRenderer>().materials;
            Hourglass.GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
            Hourglass.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Hourglass.transform.localPosition = Vector3.zero;
            Hourglass.name = "hourglass";
            Hourglass.SetActive(true);
            GameObject GlowEffect = GameObject.Instantiate(ModelSwaps.GlowEffect);
            GlowEffect.transform.parent = DayNightSwitch.transform;
            GlowEffect.transform.GetChild(0).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(1).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(2).gameObject.SetActive(false);
            GlowEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            GlowEffect.transform.localPosition = new Vector3(-0.5f, -1f, -0.1f);
            GlowEffect.SetActive(true);
            DayNightSwitch.transform.position = new Vector3(-26.3723f, 28.9452f, -46.1847f);
            DayNightSwitch.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        }

        public static bool BloodstainChest_IInteractionReceiver_Interact_PrefixPatch(Item i, BloodstainChest __instance) {
            if (SceneLoaderPatches.SceneName == "Changing Room") {
                CoinSpawner.SpawnCoins(20, __instance.transform.position);
                __instance.doPushbackBlast();
                return false;
            }

            return true;
        }

        public static void SpawnMoreSkulls() {
            if (TunicArchipelago.Settings.MoreSkulls && SceneManager.GetActiveScene().name == "Swamp Redux 2") {
                GameObject Skull = GameObject.Find("skull (gold) (0)");
                if (Skull != null) {
                    GameObject.Instantiate(Skull, new Vector3(69.3966f, 4.0462f, -64.6f), Quaternion.EulerRotation(new Vector3(0f, 230f, 0f)), Skull.transform.parent).name = "skull (gold) (4)";
                    GameObject.Instantiate(Skull, new Vector3(37.6797f, 0.0178f, -90.9843f), Quaternion.EulerRotation(new Vector3(0f, 26f, 0f)), Skull.transform.parent).name = "skull (gold) (5)";
                    GameObject.Instantiate(Skull, new Vector3(104f, -0.0654f, -79.2f), Quaternion.EulerRotation(new Vector3(0f, 324f, 0f)), Skull.transform.parent).name = "skull (gold) (6)";
                    GameObject.Instantiate(Skull, new Vector3(31.6891f, -0.0654f, -70.3681f), Quaternion.EulerRotation(new Vector3(0f, 90f, 0f)), Skull.transform.parent).name = "skull (gold) (7)";
                }
            }
        }

        public static bool SpecialSwampTrigger_OnTriggerEnter_PrefixPatch(SpecialSwampTrigger __instance, Collider c) {

            if (TunicArchipelago.Settings.MoreSkulls && c.GetComponent<SpecialSwampRB>() != null) {
                __instance.list.Add(c.GetComponent<SpecialSwampRB>());
                c.gameObject.SetActive(false);

                if (__instance.list.Count == 8) {
                    __instance.enableWhenSatisfied.BoolValue = true;
                }
                return false;
            }

            return true;
        }
    }
}
