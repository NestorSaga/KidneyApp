using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.IO;

public class GameManager : MonoBehaviour
{


    [SerializeField] private string addAchievmentEndpoint = "http://127.0.0.1:12345/account/addAchievment";
    [SerializeField] private string dataEndpoint = "http://127.0.0.1:12345/account/retrieveData";


    [SerializeField] public string JSONfile;


    private string path = "";
    private string persistentPath = "";


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

        path = Application.dataPath + Path.AltDirectorySeparatorChar + "Data/data.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Data/data.json";
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
        form.AddField("rCompletion", 2);
    
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


    public IEnumerator LoadToJSON()
    {
        WWWForm form = new WWWForm();

        form.AddField("rUserId", PlayerPrefs.GetString("userId"));

        UnityWebRequest request = UnityWebRequest.Post(dataEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }


        if (request.result == UnityWebRequest.Result.Success)
        {

            JSONdata response = JsonUtility.FromJson<JSONdata>(request.downloadHandler.text);

            string jsonString = JsonUtility.ToJson(response);

            SaveData(jsonString);

            Debug.Log(jsonString);

        }
        else
        {
            Debug.Log("conectadont");
        }

        yield return null;
    }


    public void FillJSON()
    {
        //JSONfile = Application.persistentDataPath + "/Assets/Data/data.json";

        StartCoroutine(LoadToJSON());



    }


    public void SaveData(string data)
    {
        string savePath = persistentPath;

        //string json = JsonUtility.ToJson(data);

        using StreamWriter writer = new StreamWriter(savePath);
        
        writer.Write(data);

        
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();

        JSONdata data = JsonUtility.FromJson<JSONdata>(json);
        Debug.Log(data.ToString());
    }

}
