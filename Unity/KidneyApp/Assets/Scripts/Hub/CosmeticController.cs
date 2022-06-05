using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CosmeticController : MonoBehaviour
{
    [SerializeField] private Image hat, face, body;
    [SerializeField] private Sprite[] hats, faces, bodies;
    [SerializeField] private int[] currentCosmetics = new int[3] {0,0,0}; //Hat, face and body
    [SerializeField] private string createCosmeticsEndponint = "http://127.0.0.1:12345/cosmetics/createUserCosmetics";
    [SerializeField] private string saveCosmeticsEndponint = "http://127.0.0.1:12345/cosmetics/setUserCosmetics";

    private void Start() {
        currentCosmetics[0] = PlayerPrefs.GetInt("hat");
        currentCosmetics[1] = PlayerPrefs.GetInt("face");
        currentCosmetics[2] = PlayerPrefs.GetInt("body");
        
        setCosmetics(currentCosmetics);
    }

    public void setHat(Sprite spr_hat) {
        hat.sprite = spr_hat;
    }

    public void setFace(Sprite spr_face) {
        face.sprite = spr_face;
    }

    public void setBody(Sprite spr_body) {
        body.sprite = spr_body;
    }

    public void toggleHat(bool positive) {
        if(positive) {
            if(currentCosmetics[0] +1 < hats.Length) {
                currentCosmetics[0] += 1;
                hat.sprite = hats[currentCosmetics[0]];
            } else {
                currentCosmetics[0] = 0;
                hat.sprite = hats[currentCosmetics[0]];
            }
        } else {
            if(currentCosmetics[0] -1 >= 0) {
                currentCosmetics[0] -= 1;
                hat.sprite = hats[currentCosmetics[0]];
            } else {
                currentCosmetics[0] = hats.Length-1;
                hat.sprite = hats[currentCosmetics[0]];
            }
        }
    }

    public void toggleFace(bool positive) {
        if(positive) {
            if(currentCosmetics[1] +1 < faces.Length) {
                currentCosmetics[1] += 1;
                face.sprite = faces[currentCosmetics[1]];
            } else {
                currentCosmetics[1] = 0;
                face.sprite = faces[currentCosmetics[1]];
            }
        } else {
            if(currentCosmetics[1] -1 >= 0) {
                currentCosmetics[1] -= 1;
                face.sprite = faces[currentCosmetics[1]];
            } else {
                currentCosmetics[1] = faces.Length-1;
                face.sprite = faces[currentCosmetics[1]];
            }
        }
    }

    public void toggleBody(bool positive) {
        if(positive) {
            if(currentCosmetics[2] +1 < bodies.Length) {
                currentCosmetics[2] += 1;
                body.sprite = bodies[currentCosmetics[2]];
            } else {
                currentCosmetics[2] = 0;
                body.sprite = bodies[currentCosmetics[2]];
            }
        } else {
            if(currentCosmetics[2] -1 >= 0) {
                currentCosmetics[2] -= 1;
                body.sprite = bodies[currentCosmetics[2]];
            } else {
                currentCosmetics[2] = bodies.Length-1;
                body.sprite = bodies[currentCosmetics[2]];
            }
        }
    }

    public void setCosmetics(int[] cosmeticArray) {
        if(cosmeticArray[0] < hats.Length) {
            setHat(hats[cosmeticArray[0]]);
        }
        if(cosmeticArray[1] < faces.Length) {
            setFace(faces[cosmeticArray[1]]);
        }
        if(cosmeticArray[2] < bodies.Length) {
            setBody(bodies[cosmeticArray[2]]);
        }
    }

    public void goToHub() {
        StartCoroutine(trySaveCosmetics());
    }

    public IEnumerator tryInitializeCosmetics() {
        WWWForm form = new WWWForm();
        form.AddField("rUserId", PlayerPrefs.GetString("userId"));        
        UnityWebRequest request = UnityWebRequest.Post(createCosmeticsEndponint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if(request.result != UnityWebRequest.Result.Success) {

            Debug.Log("Request failed");

        }

        yield return null;
    }

    public IEnumerator trySaveCosmetics() {

        WWWForm form = new WWWForm();
        form.AddField("rUserId", PlayerPrefs.GetString("userId"));
        form.AddField("rHat", currentCosmetics[0]);     
        form.AddField("rFace", currentCosmetics[1]);   
        form.AddField("rBody", currentCosmetics[2]);  
        PlayerPrefs.SetInt("hat", currentCosmetics[0]); 
        PlayerPrefs.SetInt("face", currentCosmetics[1]);
        PlayerPrefs.SetInt("body", currentCosmetics[2]);   
        UnityWebRequest request = UnityWebRequest.Post(saveCosmeticsEndponint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Timed out");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {


            GameManager.Instance.ChangeScene(2);

        } else {
            Debug.Log("Request failed");
        }

        yield return null;
    }
}
