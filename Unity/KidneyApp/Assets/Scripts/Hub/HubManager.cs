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
    [SerializeField] private string tipEndpoint;

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
                Debug.Log(request.downloadHandler.text);
                Debug.Log(response.tip.es);
                //tipOfTheDay.text = response.tip;

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
}
