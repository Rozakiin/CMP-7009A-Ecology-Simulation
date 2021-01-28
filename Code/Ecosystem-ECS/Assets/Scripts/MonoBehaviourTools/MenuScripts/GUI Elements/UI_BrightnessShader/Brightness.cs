using UnityEngine;

namespace MonoBehaviourTools.MenuScripts.GUI_Elements.UI_BrightnessShader
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Brightness")]
    public class Brightness : MonoBehaviour
    {

        /// Provides a shader property that is set in the inspector
        /// and a material instantiated from the shader
        public Shader ShaderDerp;

        private Material _material;

        [Range(0.5f, 2f)]
        public float brightness = 1f;

        private void Start()
        {
            // Disable the image effect if the shader can't
            // run on the users graphics card
            if (!ShaderDerp || !ShaderDerp.isSupported)
                enabled = false;
        }


        private Material Material
        {
            get
            {
                if (_material == null)
                {
                    _material = new Material(ShaderDerp)
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                }
                return _material;
            }
        }


        private void OnDisable()
        {
            if (_material)
            {
                DestroyImmediate(_material);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Material.SetFloat("_Brightness", brightness);
            Graphics.Blit(source, destination, Material);
        }
    }
}
