using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchVideos : MonoBehaviour {
  public GameObject localVideoSample;
  public GameObject dashVideoSample;
  public GameObject panoVideoSample;

  private GameObject[] videoSamples;

  public Text missingLibText;

  public void Awake() {
    videoSamples = new GameObject[3];
    videoSamples[0] = localVideoSample;
    videoSamples[1] = dashVideoSample;
    videoSamples[2] = panoVideoSample;

    string NATIVE_LIBS_MISSING_MESSAGE = "Video Support libraries not found or could not be loaded!\n" +
          "Please add the <b>GVRVideoPlayer.unitypackage</b>\n to this project";

    if (missingLibText != null) {
      try {
        IntPtr ptr = GvrVideoPlayerTexture.CreateVideoPlayer();
        if (ptr != IntPtr.Zero) {
          GvrVideoPlayerTexture.DestroyVideoPlayer(ptr);
          missingLibText.enabled = false;
        } else {
          missingLibText.text = NATIVE_LIBS_MISSING_MESSAGE;
          missingLibText.enabled = true;
        }
      } catch (Exception e) {
        Debug.LogError(e);
        missingLibText.text = NATIVE_LIBS_MISSING_MESSAGE;
        missingLibText.enabled = true;
      }
    }
  }

  public void ShowMainMenu() {
    ShowSample(-1);
  }

  public void OnFlatLocal() {
    ShowSample(0);
  }

  public void OnDash() {
    ShowSample(1);
  }

  public void On360Video() {
    ShowSample(2);
  }

  private void ShowSample(int index) {
    // If the libs are missing, always show the main menu.
    if (missingLibText != null && missingLibText.enabled) {
      index = -1;
    }

    for (int i = 0; i < videoSamples.Length; i++) {
      if (videoSamples[i] != null) {

        if (i != index) {
          if (videoSamples[i].activeSelf) {
            videoSamples[i].GetComponentInChildren<GvrVideoPlayerTexture>().CleanupVideo();
          }
        } else {
            videoSamples[i].GetComponentInChildren<GvrVideoPlayerTexture>().ReInitializeVideo();
        }
        // GvrVideoPlayerTexture needs an additional frame after CleanupVideo() to finish
        // cleanup and allow its coroutine to exit, otherwise it gets permenantly stuck
        // if it is deactivated too soon.
        StartCoroutine(SetActiveDelayed(videoSamples[i], i == index));
      }
    }
    GetComponent<Canvas>().enabled = index == -1;
  }

  private IEnumerator SetActiveDelayed(GameObject go, bool state) {
    yield return new WaitForEndOfFrame();
    go.SetActive(state);
  }
}
