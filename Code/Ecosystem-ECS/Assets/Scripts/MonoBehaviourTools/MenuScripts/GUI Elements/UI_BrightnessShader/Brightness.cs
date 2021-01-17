using UnityEngine;

namespace MonoBehaviourTools.MenuScripts.GUI_Elements.UI_BrightnessShader
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Brightness")]
    public class Brightness : MonoBehaviour
    {

        /// Provides a shader property that is set in the inspector
        /// and a material instantiated from the shader
        public Shader shaderDerp;
        Material m_Material;

        [Range(0.5f, 2f)]
        public float brightness = 1f;

        void Start()
        {
            // Disable the image effect if the shader can't
            // run on the users graphics card
            if (!shaderDerp || !shaderDerp.isSupported)
                enabled = false;
        }


        Material Material
        {
            get
            {
                if (m_Material == null)
                {
                    m_Material = new Material(shaderDerp)
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                }
                return m_Material;
            }
        }


        void OnDisable()
        {
            if (m_Material)
            {
                DestroyImmediate(m_Material);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Material.SetFloat("_Brightness", brightness);
            Graphics.Blit(source, destination, Material);
        }
    }
}
