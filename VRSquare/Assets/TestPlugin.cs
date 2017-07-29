using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TestPlugin : MonoBehaviour {

	AndroidJavaClass jc;

	// Use this for initialization
	void Start () {  


	}

	public void OnButtonClick()
	{
		var go = EventSystem.current.currentSelectedGameObject;
		if (go != null) {
			Debug.Log ("Clicked on : " + go.name);
		#if UNITY_ANDROID
			jc = new AndroidJavaClass("com.example.speechassist.Assist");
			jc.CallStatic("promptSpeechInput");
		#endif
		} else {
			Debug.Log ("currentSelectedGameObject is null");
		}
	}

	void onActivityResult(string Translation){
	
		Debug.Log (Translation);
		GameObject.Find ("TextRecognition").GetComponent<UnityEngine.UI.Text> ().text = Translation;
        SceneManager.LoadScene("MedScene", LoadSceneMode.Single);



    }

    // Update is called once per frame
    void Update () {
	
	}
}
