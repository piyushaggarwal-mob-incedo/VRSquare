

namespace GVRSample {
  using UnityEngine;

  /// <summary>
  /// Auto play video.
  /// </summary>
  /// <remarks>This script exposes a delay value in seconds to start playing the TexturePlayer
  /// component on the same object.
  /// </remarks>

  [RequireComponent(typeof(GvrVideoPlayerTexture))]
  public class AutoPlayVideo : MonoBehaviour {
    private bool done;
    private float t;
    private GvrVideoPlayerTexture player;

    public float delay = 2f;
    public bool loop = false;

    void Start() {
      t = 0;
      done = false;
      player = GetComponent<GvrVideoPlayerTexture>();
      if (player != null) {
        player.Init();
      }
    }

    void Update() {
      if (player == null) {
        return;
      } else if (player.PlayerState == GvrVideoPlayerTexture.VideoPlayerState.Ended && done && loop) {
        player.Pause();
        player.CurrentPosition = 0;
        done = false;
        t = 0f;
        return;
      }
      if (done) {
        return;
      }

      t += Time.deltaTime;
      if (t >= delay && player != null) {
        player.Play();
        done = true;
      }
    }
  }
}
