using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace UnityVolumeRendering
{
    public class KeywordManager : MonoBehaviour
    {
        KeywordRecognizer keywordRecognizer = null;
        Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

        public BrightnessOverrideComponent BrightnessOverride;
        public GameObject Content;
        [SerializeField] private VolumeRenderedObject volumeObject;


        // Use this for initialization
        void Start()
        {
            keywords.Add("Position", () =>
            {
                this.Content.GetComponent<NearInteractionGrabbable>().enabled = !this.Content.GetComponent<NearInteractionGrabbable>().enabled;
                this.Content.GetComponent<ObjectManipulator>().enabled = !this.Content.GetComponent<ObjectManipulator>().enabled;
                //this.BrightnessOverride.Brightness = 0f;
            });

            keywords.Add("Remove Minimum", () =>
            {
                const float threshold = 0.035f;
                Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
                visibilityWindow.x = Mathf.Min(visibilityWindow.x + 0.1f, visibilityWindow.y - threshold);
                volumeObject.SetVisibilityWindow(visibilityWindow.x, visibilityWindow.y);

            });

            keywords.Add("Remove Maximum", () =>
            {
                const float threshold = 0.035f;
                Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
                visibilityWindow.y = Mathf.Min(visibilityWindow.y - 0.1f, visibilityWindow.y + threshold);
                volumeObject.SetVisibilityWindow(visibilityWindow.x, visibilityWindow.y);
            });

            keywords.Add("Add Minimum", () =>
            {
                const float threshold = 0.035f;
                Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
                visibilityWindow.x = Mathf.Min(visibilityWindow.x - 0.1f, visibilityWindow.y - threshold);
                volumeObject.SetVisibilityWindow(visibilityWindow.x, visibilityWindow.y);

            });

            keywords.Add("Add Maximum", () =>
            {
                const float threshold = 0.035f;
                Vector2 visibilityWindow = volumeObject.GetVisibilityWindow();
                visibilityWindow.y = Mathf.Min(visibilityWindow.y + 0.1f, visibilityWindow.y + threshold);
                volumeObject.SetVisibilityWindow(visibilityWindow.x, visibilityWindow.y);
            });

            keywords.Add("Off", () =>
            {
                this.BrightnessOverride.Brightness = 0f;
            });

            keywords.Add("On", () =>
            {
                this.BrightnessOverride.Brightness = 1.0f;
            });

            keywords.Add("Darker", () =>
            {
                this.BrightnessOverride.Brightness -= 0.1f;
            });

            keywords.Add("Brighter", () =>
            {
                this.BrightnessOverride.Brightness += 0.1f;
            });

            keywords.Add("Hide", () =>
            {
                //this.Content.SetActive(false);
            });

            keywords.Add("Show", () =>
            {
                this.Content.SetActive(true);
            });

            keywords.Add("Quit", () =>
            {
                Application.Quit();
            });

            // Tell the KeywordRecognizer about our keywords.
            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

            // Register a callback for the KeywordRecognizer and start recognizing!
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Start();
        }

        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action keywordAction;
            if (keywords.TryGetValue(args.text, out keywordAction))
            {
                keywordAction.Invoke();
            }
        }
    }
}