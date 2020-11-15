using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Video;
using UnityEngine.UI;

public class AugmentedImageVisualizer : MonoBehaviour
{

    public VideoClip[] videoClips;
    private VideoPlayer videoPlayer;
    public AugmentedImage image;

    public Button a;
    public Button b;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnStop;
        Button btn = a.GetComponent<Button>();
        Button btn2 = b.GetComponent<Button>();
        btn.onClick.AddListener(videoQuit);
        btn2.onClick.AddListener(videoQuit2);

    }

    void OnStop(VideoPlayer video)
    {
        gameObject.SetActive(false);
    }

    void videoQuit()
    {
        Application.Quit();
        videoPlayer.Stop();
        OnStop(videoPlayer);
        videoPlayer.clip = videoClips[1];
        videoPlayer.Play();
 

    }
    void videoQuit2()
    {
        Application.Quit();
        videoPlayer.Stop();
        OnStop(videoPlayer);
        videoPlayer.clip = videoClips[0];
        videoPlayer.Play();
    }
    // Update is called once per frame
    void Update()
    {

        if(image == null || image.TrackingState != TrackingState.Tracking)
        {
            return;
        }

        if(!videoPlayer.isPlaying)
        {
            videoPlayer.clip = videoClips[image.DatabaseIndex];
            videoPlayer.Play();
        }

        transform.localScale = new Vector3(image.ExtentX, image.ExtentZ, 1.0f);
    }
}