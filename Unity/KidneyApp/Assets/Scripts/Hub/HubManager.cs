using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.Networking;

public class HubManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipOfTheDay;
    [SerializeField] private string tipEndpoint = "http://127.0.0.1:12345/randomTip";

    private void Awake()
    {
        StartCoroutine(TryGetDailyTip());
    }

    public IEnumerator TryGetDailyTip()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Post(tipEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {

            TipResponse response = JsonUtility.FromJson<TipResponse>(request.downloadHandler.text);

            if (response.code == 0)
            {
                tipOfTheDay.text = response.tip.es;

            }
            else
            {
                Debug.Log("Request was not successful");
            }

        }
        else
        {

            Debug.Log("Request failed");
        }

        yield return null;
    }

    public void FillJSON() {
        GameManager.Instance.FillJSON();
    }

    public void ExitHub()
    {
        GameManager.Instance.ClearJSON();

        GameManager.Instance.ChangeScene(0); // return to login
    }

    public void goToScene(int scene)
    {
        GameManager.Instance.ChangeScene(scene);
    }

    public void testProgressAchiev(string name){
        GameManager.Instance.ProgressAchievment(name,1);
    }
}
