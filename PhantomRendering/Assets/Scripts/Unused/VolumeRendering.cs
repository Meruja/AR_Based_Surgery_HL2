
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace UnityVolumeRendering
{

    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class VolumeRendering : MonoBehaviour
    {

        [SerializeField] protected Shader shader;
        protected Material material;

        [SerializeField] Color color = Color.white;
        [Range(0f, 1f)] public float threshold = 0.5f;
        [Range(0.5f, 5f)] public float intensity = 1.5f;
        [Range(0f, 1f)] public float sliceXMin = 0.0f, sliceXMax = 1.0f;
        [Range(0f, 1f)] public float sliceYMin = 0.0f, sliceYMax = 1.0f;
        [Range(0f, 1f)] public float sliceZMin = 0.0f, sliceZMax = 1.0f;
        public Quaternion axis = Quaternion.identity;
        public Texture volume;
        [SerializeField]
        private VolumeRenderedObject volumeRenderedObject;


        // Define the movement parameters
        public float speed = 1f; // Adjust the speed of movement
        public float amplitude = 1f; // Adjust the distance of movement

        private float elapsedTime = 0f;
        private Vector3 initialPosition;
        GameObject slicingPlane;

        private void Awake()
        {
            //volumeRenderedObject = GetComponent<VolumeRenderedObject>();
            volumeRenderedObject = GameObject.FindGameObjectWithTag("VolumeObject").GetComponent<VolumeRenderedObject>();

        }

        protected virtual void Start()
        {
            material = new Material(shader);
            GetComponent<MeshFilter>().sharedMesh = Build();
            GetComponent<MeshRenderer>().sharedMaterial = material;


/*            slicingPlane = GameObject.FindGameObjectWithTag("SlicingPlane");
            initialPosition = slicingPlane.transform.position;*/
        }

        protected void Update()
        {
            material.SetTexture("_Volume", volume);
            material.SetColor("_Color", color);
            material.SetFloat("_Threshold", threshold);
            material.SetFloat("_Intensity", intensity);
            material.SetVector("_SliceMin", new Vector3(sliceXMin, sliceYMin, sliceZMin));
            material.SetVector("_SliceMax", new Vector3(sliceXMax, sliceYMax, sliceZMax));
            material.SetMatrix("_AxisRotationMatrix", Matrix4x4.Rotate(axis));

           /* Material mat = volumeRenderedObject.meshRenderer.sharedMaterial;
            mat.SetFloat("_Threshold", threshold);
            mat.SetFloat("_Intensity", intensity);
            mat.SetVector("_SliceMin", new Vector3(sliceXMin, sliceYMin, sliceZMin));
            mat.SetVector("_SliceMax", new Vector3(sliceXMax, sliceYMax, sliceZMax));
            mat.SetMatrix("_AxisRotationMatrix", Matrix4x4.Rotate(axis));*/
            //volumeRenderedObject.UpdateMaterialProperties();
/*
            elapsedTime += Time.deltaTime;

            // Calculate the new position using a sine wave pattern
            //float yPos = initialPosition.y + Mathf.Sin(elapsedTime * speed) * amplitude;
            //Debug.Log("initialPosition PositionX " + initialPosition.x);
            Debug.Log("X value " + sliceXMin);
            //Debug.Log("initialPosition Position Y " + initialPosition.y);
            //Debug.Log("initialPosition Position Z " + initialPosition.z);
            float yPos = initialPosition.y + Mathf.Sin(elapsedTime * speed) * amplitude;
            //Debug.Log(elapsedTime * speed);
            //Debug.Log(Mathf.Sin(elapsedTime * speed) * amplitude);
            //Debug.Log("New Position Position Y " + yPos);
            Vector3 newPosition = new Vector3(initialPosition.x, yPos, initialPosition.z);

            // Apply the new position to the slicing plane
            slicingPlane.transform.position = newPosition;*/
        }

        Mesh Build()
        {
            var vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f, -0.5f,  0.5f),
                new Vector3 (-0.5f, -0.5f,  0.5f),
            };
            var triangles = new int[] {
                0, 2, 1,
                0, 3, 2,
                2, 3, 4,
                2, 4, 5,
                1, 2, 5,
                1, 5, 6,
                0, 7, 4,
                0, 4, 3,
                5, 4, 7,
                5, 7, 6,
                0, 6, 7,
                0, 1, 6
            };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            return mesh;
        }

        void OnValidate()
        {
            Constrain(ref sliceXMin, ref sliceXMax);
            Constrain(ref sliceYMin, ref sliceYMax);
            Constrain(ref sliceZMin, ref sliceZMax);
        }

        void Constrain(ref float min, ref float max)
        {
            const float threshold = 0.025f;
            if (min > max - threshold)
            {
                min = max - threshold;
            }
            else if (max < min + threshold)
            {
                max = min + threshold;
            }
        }

        void OnDestroy()
        {
            Destroy(material);
        }
    }

}


