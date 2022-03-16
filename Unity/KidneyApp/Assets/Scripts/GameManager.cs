using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{


    [SerializeField] private string addAchievmentEndpoint = "http://127.0.0.1:12345/account/addAchievment";

    
    private static GameManager instance = null;
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ChangeScene(int targetSceneId) {
        SceneManager.LoadScene(targetSceneId);
    } 


    public void AddAchievment(string name){

        StartCoroutine(tryAddAchievment(name));
    }

    private IEnumerator tryAddAchievment(string name){

        string userId = PlayerPrefs.GetString("userid");

        WWWForm form = new WWWForm();
        form.AddField("rUserId", userId);
        form.AddField("rAchievmentName", name);
    

        UnityWebRequest request = UnityWebRequest.Post(addAchievmentEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                break;
            }

            yield return null;
        }     

       
        yield return null;
    }

}
