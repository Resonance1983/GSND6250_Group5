using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Enviro
{
    [Serializable]
    public class EnviroLightning
    {
        public Lightning prefab;
        public bool lightningStorm = false;
        [Range(1f, 60f)] public float randomLightingDelay = 10.0f;
        [Range(0f, 10000f)] public float randomSpawnRange = 5000.0f;
        [Range(0f, 10000f)] public float randomTargetRange = 5000.0f;
    }

    [Serializable]
    public class EnviroLightningModule : EnviroModule
    {
        public EnviroLightning Settings;
        public EnviroLightningModule preset;
        public bool showLightningControls;
        private bool spawned = false;

        // Update Method
        public override void UpdateModule()
        {
            if (!active)
                return;

            if (Application.isPlaying && Settings.lightningStorm && Settings.prefab != null) CastLightningBoltRandom();
        }

        public void CastLightningBolt(Vector3 from, Vector3 to)
        {
            if (Settings.prefab != null)
            {
                var lightn = (Lightning)Instantiate(Settings.prefab, from, Quaternion.identity);
                lightn.target = to;

                //Play Thunder SFX with delay if Audio module is used.
                if (EnviroManager.instance.Audio != null) EnviroManager.instance.StartCoroutine(PlayThunderSFX(0.05f));
            }
            else
            {
                Debug.Log("Please assign a lightning prefab in your Enviro Ligthning module!");
            }
        }

        public void CastLightningBoltRandom()
        {
            if (!spawned)
            {
                //Calculate some random spawn and target locations.
                var circlSpawn = UnityEngine.Random.insideUnitCircle * Settings.randomSpawnRange;
                var circlTarget = UnityEngine.Random.insideUnitCircle * Settings.randomTargetRange;
                var spawnPosition = new Vector3(circlSpawn.x + EnviroManager.instance.transform.position.x, 2500f,
                    circlSpawn.y + EnviroManager.instance.transform.position.z);
                var targetPosition = new Vector3(circlTarget.x + spawnPosition.x, 0f, circlTarget.y + spawnPosition.z);
                EnviroManager.instance.StartCoroutine(LightningStorm(spawnPosition, targetPosition));
            }
        }

        private IEnumerator LightningStorm(Vector3 spwn, Vector3 targ)
        {
            spawned = true;
            yield return new WaitForSeconds(Settings.randomLightingDelay);
            CastLightningBolt(spwn, targ);
            spawned = false;
        }

        private IEnumerator PlayThunderSFX(float delay)
        {
            yield return new WaitForSeconds(delay);
            EnviroManager.instance.Audio.PlayRandomThunderSFX();
        }

        //Save and Load
        public void LoadModuleValues()
        {
            if (preset != null)
                Settings = JsonUtility.FromJson<EnviroLightning>(JsonUtility.ToJson(preset.Settings));
            else
                Debug.Log("Please assign a saved module to load from!");
        }

        public void SaveModuleValues()
        {
#if UNITY_EDITOR
            var t = CreateInstance<EnviroLightningModule>();
            t.name = "Lightning Preset";
            t.Settings = JsonUtility.FromJson<EnviroLightning>(JsonUtility.ToJson(Settings));

            var assetPathAndName =
                UnityEditor.AssetDatabase.GenerateUniqueAssetPath(EnviroHelper.assetPath + "/New " + t.name + ".asset");
            UnityEditor.AssetDatabase.CreateAsset(t, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public void SaveModuleValues(EnviroLightningModule module)
        {
            module.Settings = JsonUtility.FromJson<EnviroLightning>(JsonUtility.ToJson(Settings));

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(module);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}