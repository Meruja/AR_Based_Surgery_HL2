
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UnityVolumeRendering
{

    public class VolumeRenderingController : MonoBehaviour {

        //[SerializeField] protected VolumeRendering volume;
        [SerializeField] protected Slider sliderXMin, sliderXMax, sliderYMin, sliderYMax, sliderZMin, sliderZMax;
        [SerializeField] protected Slider sliderThresholdMin, sliderThresholdMax;
        [SerializeField] protected Transform axis;
        [SerializeField] private VolumeRenderedObject volumeObject;

        [Range(0f, 1f)] public float intensity = 0.5f;
        [Range(0f, 1f)] public float thresholdMin = 0.0f, thresholdMax = 1.0f;
        [Range(0f, 1f)] public float sliceXMin = 0.0f, sliceXMax = 1.0f;
        [Range(0f, 1f)] public float sliceYMin = 0.0f, sliceYMax = 1.0f;
        [Range(0f, 1f)] public float sliceZMin = 0.0f, sliceZMax = 1.0f;

        private Vector3 initialPosition, yMinNewPosition, zNewPosition, xNewPosition, xMaxNewPosition, yMaxNewPosition, zMaxNewPosition;
        GameObject yMinPlane, yMaxPlane, zMinPlane, xMinPlane, xMaxPlane, zMaxPlane;
        private void Awake()
        {
            //volumeRenderedObject = GetComponent<VolumeRenderedObject>();
            //volumeObject = GameObject.FindGameObjectWithTag("VolumeObject").GetComponent<VolumeRenderedObject>();

        }
        void Start ()
        {
/*            const float threshold = 0.025f;

            sliderXMin.onValueChanged.AddListener((v) => {
                sliceXMin = sliderXMin.value = Mathf.Min(v, sliceXMax - threshold);
                float xPos = -0.31f * sliceXMin + 0.07f;
                xNewPosition = new Vector3(xPos, initialPosition.y, initialPosition.z);
            });
            sliderXMax.onValueChanged.AddListener((v) => {
                sliceXMax = sliderXMax.value = Mathf.Max(v, sliceXMin + threshold);
                float xPos = -0.31f * sliceXMax + 0.07f;
                xMaxNewPosition = new Vector3(xPos, initialPosition.y, initialPosition.z);
            });
            
            sliderYMin.onValueChanged.AddListener((v) => {
                sliceYMin = sliderYMin.value = Mathf.Min(v, sliceYMax - threshold);
                float yPos = -0.3f * sliceYMin + 0.15f;
                yMinNewPosition = new Vector3(initialPosition.x, yPos, initialPosition.z);
            });
            sliderYMax.onValueChanged.AddListener((v) => {
                sliceYMax = sliderYMax.value = Mathf.Max(v, sliceYMin + threshold);
                float yPos = -0.3f * sliceYMax + 0.15f;
                yMaxNewPosition = new Vector3(initialPosition.x, yPos, initialPosition.z);
            });

            sliderZMin.onValueChanged.AddListener((v) => {
                sliceZMin = sliderZMin.value = Mathf.Min(v, sliceZMax - threshold);
                float zPos = 0.25f * sliceZMin + 0.25f;
                zNewPosition = new Vector3(initialPosition.x, initialPosition.y, zPos);
            });
            sliderZMax.onValueChanged.AddListener((v) => {
                //volume.sliceZMax = sliderZMax.value = Mathf.Max(v, volume.sliceZMin + threshold);
                sliceZMax = sliderZMax.value = Mathf.Max(v, sliceZMin + threshold);
                float zPos = 0.25f * sliceZMax + 0.25f;
                Debug.Log("Slicer value Z " + sliceZMax);
                zMaxNewPosition = new Vector3(initialPosition.x, initialPosition.y, zPos);
                Debug.Log("new Position Position Z " + zPos);
            });

            yMinPlane = GameObject.FindGameObjectWithTag("SlicingPlane");
            yMaxPlane = GameObject.FindGameObjectWithTag("Ymax");
            //xMinPlane = GameObject.FindGameObjectWithTag("Xmin");
            //xMaxPlane = GameObject.FindGameObjectWithTag("Xmax");
            initialPosition = yMinPlane.transform.position;

            zMinPlane = GameObject.FindGameObjectWithTag("Zmin");
            zMaxPlane = GameObject.FindGameObjectWithTag("Zmax");*/
        }

        void Update()
        {
           /* volumeObject.axis = axis.rotation;

            // Apply the new position to the slicing plane
            yMinPlane.transform.position = yMinNewPosition;
            yMaxPlane.transform.position = yMaxNewPosition;
            //xMinPlane.transform.position = xNewPosition;
            //xMaxPlane.transform.position = xMaxNewPosition;
            zMinPlane.transform.position = zNewPosition;
            zMaxPlane.transform.position = zMaxNewPosition;*/
        }

        public void OnIntensity(float v)
        {
            VolumeDataset dataset = volumeObject.dataset;
            float minValue = Mathf.Max(v*20, dataset.GetMinDataValue());
            Debug.Log("slider value " + v);
            volumeObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.UpdateDataTexture(minValue));

        }


        public void Brightness()
        {
            VolumeDataset dataset = volumeObject.dataset;
            float intensityValue = dataset.GetMinDataValue();
            Debug.Log("value " + intensityValue);
            //volumeObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.UpdateDataTexture(minValue));

        }

        public void OnThreshold(float v)
        {
            //volumeObject.threshold = v;
            const float threshold = 0.025f;
            thresholdMin = sliderThresholdMin.value = Mathf.Min(v, thresholdMax - threshold);
            Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
            volumeObject.SetVisibilityWindow(thresholdMin, thresholdMax);

        }
        public void OnThresholdMax(float v)
        {
            //volumeObject.threshold = v;
            const float threshold = 0.025f;
            thresholdMax = sliderThresholdMax.value = Mathf.Max(v, thresholdMin + threshold);
            Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
            volumeObject.SetVisibilityWindow(thresholdMin, thresholdMax);

        }

    }

}


