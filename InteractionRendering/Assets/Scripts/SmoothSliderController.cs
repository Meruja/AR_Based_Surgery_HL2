using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityVolumeRendering
{
    public class SmoothSliderController : MonoBehaviour
    {
        [SerializeField]
        private VolumeRenderedObject volumeRenderedObject;

        [SerializeField]
        private Slider smoothingSlider;
        private float smoothingValue = 0.5f;
        private Texture3D texture;
        public Text valueText;

        // Start is called before the first frame update
        void Start()
        {
            volumeRenderedObject = GameObject.FindGameObjectWithTag("VolumeObject").GetComponent<VolumeRenderedObject>();
            smoothingSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });

        }

        void OnSliderValueChange()
        {
            valueText.text = valueText.ToString();
            smoothingValue = smoothingSlider.value;
            ApplySmoothing();
        }

        void ApplySmoothing()
        {
            // Get the current dataset
            VolumeDataset dataset = volumeRenderedObject.dataset;
            // Get the pixel data from the dataset
            Texture3D dataTexture = volumeRenderedObject.dataset.GetDataTexture();
            Color[] pixels = dataTexture.GetPixels();
            // Apply filter to the pixel data with the specified smoothing value
            for (int i = 0; i < pixels.Length; i++)
            {
                float value = pixels[i].r;
                // Call Gaussian Filter
                float smoothedValue = Smooth(value, smoothingValue);
                pixels[i] = new Color(smoothedValue, smoothedValue, smoothedValue);
            }
            // Update the dataset with the modified pixel data
            dataTexture.SetPixels(pixels);
            dataTexture.Apply();
            // Update the volume rendered object with the modified dataset
            volumeRenderedObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
        }

        /*     void OnGUI()
             {
                // Create a slider for the smoothing value
                smoothingValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), smoothingValue, 0.0f, 1.0f);

                // Apply the smoothing function to the pixel data
                Color[] pixels = texture.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                {
                    float value = pixels[i].r;
                    float smoothedValue = Smooth(value, smoothingValue);
                    pixels[i] = new Color(smoothedValue, smoothedValue, smoothedValue);
                }

                // Update the texture with the modified pixel data
                texture.SetPixels(pixels);
                texture.Apply();
             }*/

        float Smooth(float value, float smoothing)
        {
            // Apply a simple Gaussian smoothing function
            float weight = Mathf.Exp(-(smoothing * smoothing));
            float smoothedValue = 0.0f;
            float totalWeight = 0.0f;

            for (int i = -2; i <= 2; i++)
            {
                float neighborValue = texture.GetPixelBilinear(value + i, 0.5f, 0.5f).r;
                float neighborWeight = Mathf.Exp(-(i * i) / (2.0f * smoothing * smoothing));

                smoothedValue += neighborValue * neighborWeight;
                totalWeight += neighborWeight;
            }

            return smoothedValue / totalWeight;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
