﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityVolumeRendering
{
    public class TimeseriesFullImageSize : MonoBehaviour
    {
        private static int dimX = 224;
        private static int dimY = 208;
        private static int dimZ = 208;

        /*private static int dimX = 64;
        private static int dimY = 64;
        private static int dimZ = 64;*/
        private static DataContentFormat dataFormat = DataContentFormat.Uint8;
        private static Endianness endianness = Endianness.LittleEndian;
        private static int bytesToSkip = 0;
        [SerializeField]
        private VolumeRenderedObject volumeRenderedObject;
        [SerializeField]
        private List<VolumeDataset> datasets;
        private float accumulatedTime = 0.0f;
        private float framesPerSecond = 5.0f;
        private float time;
        private int frameCount;
        private float poolingTime = 1f;
        private float rrInterval = 800;

        private int currentIndex = 0;
        public int HeartRate = 75;
        public float waitTime;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Volume Rendering/Load dataset/Load raw time series full image")]
        private static void ImportTimeSeries()
        {
            string directory = UnityEditor.EditorUtility.OpenFolderPanel("Select a folder", "DataFiles", "");
            if (Directory.Exists(directory))
            {
                IEnumerable<string> fileCandidates = Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(p => p.EndsWith(".raw", StringComparison.InvariantCultureIgnoreCase)).OrderBy(f => f);

                List<VolumeDataset> datasets = new List<VolumeDataset>();
                foreach (String file in fileCandidates)
                {
                    RawDatasetImporter importer = new RawDatasetImporter(file, dimX, dimY, dimZ, dataFormat, endianness, bytesToSkip);
                    VolumeDataset dataset = importer.Import();
                    if (dataset != null)
                        datasets.Add(dataset);
                }

                if (datasets.Count > 0)
                {
                    VolumeRenderedObject volObj = VolumeObjectFactory.CreateObject(datasets[0]);
                    TimeseriesFullImageSize timeSeriesManager = volObj.gameObject.AddComponent<TimeseriesFullImageSize>();
                    timeSeriesManager.volumeRenderedObject = volObj;
                    timeSeriesManager.datasets = datasets;
                }
            }
        }
#endif

        private void Start()
        {
            // Create all dataset textures first
            foreach (VolumeDataset dataset in datasets)
                dataset.GetDataTexture();
        }

        /*        private void Update()
                {
                    accumulatedTime += Time.deltaTime;
                    time += Time.deltaTime;
                    frameCount++;
                    //accumulatedTime += Time.deltaTime;
                    //int framesPerSecond = Mathf.RoundToInt(frameCount / time);

                    if (time >= poolingTime)
                    {
                        //framesPerSecond = Mathf.RoundToInt(frameCount / time);
                        framesPerSecond = (int)(rrInterval / frameCount);
                        Debug.Log("framesPerSecond " + framesPerSecond);
                        //WriteToFile("FPS: " + framesPerSecond.ToString("F2") + "\n");
                        // Instead of setting time 0, we did this because they may have been more time elapsed.
                        time -= poolingTime;
                        frameCount = 0;
                    }
                    int index = (int)(accumulatedTime * framesPerSecond) % datasets.Count;
                    VolumeDataset dataset = datasets[index];
                    volumeRenderedObject.dataset = dataset;
                    volumeRenderedObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
                }*/
        /*private void Update()
        {
            accumulatedTime += Time.deltaTime;
            double framesPerSecond = (60f / 75) + (rrInterval / 1000f);
            //Debug.Log(framesPerSecond);
            int index = (int)(accumulatedTime * framesPerSecond) % datasets.Count;
            VolumeDataset dataset = datasets[index];
            volumeRenderedObject.dataset = dataset;
            volumeRenderedObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
        }*/
        private void Update()
        {
            StartCoroutine(PlaySequence());
        }
        private IEnumerator PlaySequence()
        {
            DisplayFrame(currentIndex);
            waitTime = rrInterval / (1000* datasets.Count);
            Debug.Log((60 / datasets.Count) + waitTime);
            yield return new WaitForSeconds(waitTime);                
            currentIndex = (currentIndex + 1) % datasets.Count;
            Debug.Log(currentIndex);

        }
        private void DisplayFrame(int currentIndex)
        {
            VolumeDataset dataset = datasets[currentIndex];
            volumeRenderedObject.dataset = dataset;
            volumeRenderedObject.meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
        }
    }
}