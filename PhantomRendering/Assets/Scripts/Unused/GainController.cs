using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityVolumeRendering
{
    public class GainController : MonoBehaviour
    {
        [SerializeField]
        private VolumeRenderedObject volumeRenderedObject;

        public Text valueText;
        private float scaleValue = 0.5f;
        private Texture3D dataTexture;

        // Start is called before the first frame update
        void Start()
        {
            volumeRenderedObject = GameObject.FindGameObjectWithTag("VolumeObject").GetComponent<VolumeRenderedObject>();
            // Get the current dataset
            VolumeDataset dataset = volumeRenderedObject.dataset;
            // Get the pixel data from the dataset
            Texture3D dataTexture = dataset.GetDataTexture();
        }

        public void OnGainSliderChanged(float value)
        {
            valueText.text = value.ToString();
            scaleValue = value;
            ApplyGain(scaleValue);
        }

        private void ApplyGain(float scaleValue)
        {
            // Adjust the volume rendering properties based on the scale value
            //volumeRenderedObject.opacity = scaleValue * 0.5f; // Adjust opacity 
            //volumeRenderedObject.intensity = scaleValue * 2.0f; // Adjust intensity 

            // Refresh or update the volume rendering to reflect the changes
            //volumeRenderedObject.Refresh();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
