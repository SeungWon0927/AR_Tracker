using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System;

public class AugmentedImageController : MonoBehaviour
{

    public AugmentedImageVisualizer augmentedImageVisualizer;
    public GameObject augmentedImageVisualizerPrefab;
    private Dictionary<int, AugmentedImageVisualizer> visualizers
        = new Dictionary<int, AugmentedImageVisualizer>();
    private List<AugmentedImage> images = new List<AugmentedImage>();

    // Update is called once per frame
    void Update()
    {
        if(Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        Session.GetTrackables<AugmentedImage>(images, TrackableQueryFilter.Updated);
        VisualizeTrackables();
    }

    private void VisualizeTrackables()
    {
        foreach (var image in images)
        {
            var visualizer = GetVisualizer(image);

            if(image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                AddVisualizer(image);
            } else if(image.TrackingState != TrackingState.Tracking && visualizer != null)
            {
                RemoveVisualizer(image, visualizer);
            }
        }
    }

    AugmentedImageVisualizer GetVisualizer(AugmentedImage image) {
        AugmentedImageVisualizer visualizer;
        visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
        return visualizer;
    }


    private void RemoveVisualizer(AugmentedImage image, AugmentedImageVisualizer visualizer)
    {
        _ShowAndroidToastMessage("Removing visualizer");
        visualizers.Remove(image.DatabaseIndex);
        Destroy(visualizer);
    }

    private void AddVisualizer(AugmentedImage image)
    {
        _ShowAndroidToastMessage("Adding visualizer");
        var anchor = image.CreateAnchor(image.CenterPose);
        var visualizer = Instantiate(augmentedImageVisualizer, anchor.transform);
        visualizer.image = image;
        visualizers.Add(image.DatabaseIndex, visualizer);
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

}
