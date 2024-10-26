using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
    [ExecuteInEditMode]
    [AddComponentMenu("Enviro 3/Volumetric Light")]
    public class EnviroVolumetricFogLight : MonoBehaviour
    {
        [Range(0f, 2f)] public float intensity = 1.0f;
        [Range(0f, 2f)] public float range = 1.0f;

        private Light myLight;

        private bool initialized = false;
        private CommandBuffer cascadeShadowCB;
        //private CommandBuffer shadowMatrixBuffer;


        public bool isOn
        {
            get
            {
                if (!isActiveAndEnabled)
                    return false;

                Init();

                return myLight.enabled;
            }

            private set { }
        }

        public new Light light
        {
            get
            {
                Init();
                return myLight;
            }
            private set { }
        }


        private void OnEnable()
        {
            Init();
            if (EnviroManager.instance != null && EnviroManager.instance.Fog != null)
                AddToLightManager();
        }

        private void OnDisable()
        {
            if (cascadeShadowCB != null && myLight != null && myLight.type == LightType.Directional)
                myLight.RemoveCommandBuffer(LightEvent.AfterShadowMap, cascadeShadowCB);

            // if(shadowMatrixBuffer != null && myLight != null && myLight.type == LightType.Directional)
            //    myLight.RemoveCommandBuffer(LightEvent.BeforeScreenspaceMask, shadowMatrixBuffer);

            if (EnviroManager.instance != null && EnviroManager.instance.Fog != null)
                RemoveFromLightManager();
        }

        private void AddToLightManager()
        {
            var addedToMgr = false;

            for (var i = 0; i < EnviroManager.instance.Fog.fogLights.Count; i++)
                if (EnviroManager.instance.Fog.fogLights[i] == this)
                {
                    addedToMgr = true;
                    break;
                }

            if (!addedToMgr)
                EnviroManager.instance.Fog.AddLight(this);
        }

        private void RemoveFromLightManager()
        {
            for (var i = 0; i < EnviroManager.instance.Fog.fogLights.Count; i++)
                if (EnviroManager.instance.Fog.fogLights[i] == this)
                {
                    EnviroManager.instance.Fog.RemoveLight(this);
                    initialized = false;
                }
        }


        private void Init()
        {
            if (initialized)
                return;

            myLight = GetComponent<Light>();

            if (myLight.type == LightType.Directional)
            {
                cascadeShadowCB = new CommandBuffer();
                cascadeShadowCB.name = "Dir Light Command Buffer";
                cascadeShadowCB.SetGlobalTexture("_CascadeShadowMapTexture",
                    new RenderTargetIdentifier(BuiltinRenderTextureType.CurrentActive));
                myLight.AddCommandBuffer(LightEvent.AfterShadowMap, cascadeShadowCB);

                //shadowMatrixBuffer = new CommandBuffer();  
                //shadowMatrixBuffer.name = "Extract Shadow Matrix Buffer";  
                //myLight.AddCommandBuffer(LightEvent.BeforeShadowMap, shadowMatrixBuffer); 
            }

            initialized = true;
        }
    }
}