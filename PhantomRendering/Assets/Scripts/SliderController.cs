using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum DataContentFormat
{
    UChar,
    Short,
    Float,
    Unknown
}

namespace UnityVolumeRendering
{
    public class SliderController : MonoBehaviour
    {
        private static int dimX = 224;
        private static int dimY = 208;
        private static int dimZ = 224;

        private static DataContentFormat dataFormat = DataContentFormat.Uint8;
        private static Endianness endianness = Endianness.LittleEndian;
        private static int bytesToSkip = 0;

        [SerializeField]
        private VolumeRenderedObject volumeRenderedObject;

        public Text valueText;
        private string smoothingValue;
        private Texture3D dataTexture;
        private Transform objectPosition;
        public GameObject existingObject;
        public string mhdFilePath;


        // Start is called before the first frame update
        void Start()
        {
            //volumeRenderedObject = GameObject.FindGameObjectWithTag("VolumeObject").GetComponent<VolumeRenderedObject>();
            //smoothingSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });

        }
        public void OnSliderChanged(float value)
        {
            switch (value)
            {

                case 1:
                    valueText.text = "Low";
                    break;

                case 2:
                    valueText.text = "Medium";
                    break;

                case 3:
                    valueText.text = "High";
                    break;

                default:
                    valueText.text = "Original";
                    break;
            }
             ApplySmoothing();
        }
        void ApplySmoothing()
        {
            string path = Application.dataPath + "/smoothed/test_02.raw";
            /* Read the contents of the .mhd file. 
             * Search for the line that specifies the ElementType.
             * Extract the ElementType value.
             * Convert the ElementType string to the appropriate data type or enum
             */
            mhdFilePath = Application.dataPath + "/smoothed/test_02.mhd";
            string[] fileContents = File.ReadAllLines(mhdFilePath);
            string elementTypeLine = System.Array.Find(fileContents, line => line.StartsWith("ElementType"));
            if (elementTypeLine != null)
            {
                string elementTypeString = elementTypeLine.Split('=')[1].Trim();
                dataFormat = ConvertElementType(elementTypeString);
                Debug.Log("ElementType: " + dataFormat);
            }
            else
            {
                Debug.LogError("Element Type not found in the .mhd file");
            }

            OnOpenRAWDatasetResult(path);

            /*   // Get the current dataset
               VolumeDataset dataset = volumeRenderedObject.dataset;
               // Get the pixel data from the dataset
               Texture3D dataTexture = dataset.GetDataTexture();
               Color[] pixels = dataTexture.GetPixels();
               // Apply filter to the pixel data with the specified smoothing value
               for (int i = 0; i < pixels.Length; i++)
               {
                   float pixelValue = pixels[i].r;
                   // Call Gaussian Filter
                   float smoothedValue = Smooth(pixelValue, smoothingValue, dataTexture);
                   pixels[i] = new Color(smoothedValue, smoothedValue, smoothedValue);
               }
               // Update the dataset with the modified pixel data
               dataTexture.SetPixels(pixels);
               dataTexture.Apply();
               // Update the volume rendered object with the modified dataset
               volumeRenderedObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());*/
        }
        //Todo: test it with original image 
        private void OnOpenRAWDatasetResult(string filePath)
        {
            // Read the position, scale, tag, rotation of existing volume object
            existingObject = GameObject.FindGameObjectWithTag("VolumeObject");
            objectPosition = existingObject.transform;
            Vector3 volumeScale = existingObject.transform.localScale;
            string volumeTag = existingObject.tag;
            Vector3 existingEulerAngles = existingObject.transform.eulerAngles;

            // We'll only allow one dataset at a time in the runtime GUI (for simplicity)
            DespawnAllDatasets();

            // Import the dataset
            RawDatasetImporter importer = new RawDatasetImporter(filePath, dimX, dimY, dimZ, dataFormat, endianness, bytesToSkip);
            VolumeDataset dataset = importer.Import();

            // Spawn the object
            if (dataset != null)
            {
                VolumeRenderedObject obj = VolumeObjectFactory.CreateObject(dataset);
                obj.tag = volumeTag;
                obj.transform.position = objectPosition.position;
                // Assign the scale to the target object
                obj.transform.localScale = volumeScale;
                // Assign the rotation to the target object
                obj.transform.eulerAngles = existingEulerAngles;
            }
        }
        private void DespawnAllDatasets()
        {
            VolumeRenderedObject[] volobjs = GameObject.FindObjectsOfType<VolumeRenderedObject>();
            foreach (VolumeRenderedObject volobj in volobjs)
            {
                GameObject.Destroy(volobj.gameObject);
            }
        }
        DataContentFormat ConvertElementType(string elementTypeString)
        {
            // Implement the conversion logic based on the specific ElementType representation in the code
            switch (elementTypeString)
            {
                case "MET_UCHAR":
                    return DataContentFormat.Uint8;
                case "MET_FLOAT":
                    return DataContentFormat.Float;
                // Add more cases for other element types as needed
                default:
                    return DataContentFormat.Uint8;
            }
        }

        /* float Smooth(float value, float smoothing, Texture3D dataTexture)
         {
             // Apply a simple Gaussian smoothing function
             //float weight = Mathf.Exp(-(smoothing * smoothing));
             float smoothedValue = 0.0f;
             float totalWeight = 0.0f;

             //GetPixel method, which takes an int index for the x, y, and z axes.
             for (int i = -2; i <= 2; i++)
             {
                 print(dataTexture.width);
                 float u = (value + i) / (float)dataTexture.width;
                 if (dataTexture != null && u >= 0f && u <= 1f)
                 {
                     int xIndex = Mathf.Clamp((int)(u + i), 0, dataTexture.width - 1);
                     float neighborValue = dataTexture.GetPixel(xIndex, 0, 0).r;
                     //neighborValue = 0.1f;
                     if (neighborValue != 0)
                     {
                         float neighborWeight = Mathf.Exp(-(i * i) / (2.0f * smoothing * smoothing));
                         smoothedValue += neighborValue * neighborWeight;
                         totalWeight += neighborWeight;
                     }
                     //float neighborValue = dataTexture.GetPixelBilinear(u, 0.5f, 0.5f).r;

                 }
             }
             return smoothedValue / totalWeight;
         }*/

    }
}
