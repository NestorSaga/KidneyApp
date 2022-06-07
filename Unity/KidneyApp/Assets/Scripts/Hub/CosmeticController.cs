using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TMP_Text descriptionText, unlockedStatusText;
    [SerializeField] private bool isHub = true;

    private void Start() {
        currentCosmetics[0] = PlayerPrefs.GetInt("hat");
        currentCosmetics[1] = PlayerPrefs.GetInt("face");
        currentCosmetics[2] = PlayerPrefs.GetInt("body");

        if(!isHub) setDescription(currentCosmetics[0]);
        
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
        setDescription(currentCosmetics[0]);
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
        setDescription(currentCosmetics[1]);
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
        setDescription(currentCosmetics[2]);
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

        checCosmeticsUnlocked();

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

    public void setDescription(int objectId) {
        switch(objectId) {
            case 0:
                descriptionText.text = "Sin ropa\nDesbloquea más cosméticos al completar retos.";
                unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                break;
            case 1:
                descriptionText.text = "Ropa casual\nDesbloqueado al acceder tres veces a la aplicación!";
                if(PlayerPrefs.GetInt("loginCount") > 3) {
                    unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                } else {
                    unlockedStatusText.text = "<color=red>BLOQUEADO</color>";
                }
                break;
            case 2:
                descriptionText.text = "Atuendo de cineasta\nDesbloqueado al ver tres vídeos!";
                if(PlayerPrefs.GetInt("videoCount") > 3) {
                    unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                } else {
                    unlockedStatusText.text = "<color=red>BLOQUEADO</color>";
                }
                break;
            case 3:
                descriptionText.text = "Atuendo de chef\nDesbloqueado al crear tres menús!";
                if(PlayerPrefs.GetInt("menuCount") > 3) {
                    unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                } else {
                    unlockedStatusText.text = "<color=red>BLOQUEADO</color>";
                }
                break;
            case 4:
                descriptionText.text = "Atuendo de estudiante\nDesbloqueado al responder a tres cuestionarios!";
                if(PlayerPrefs.GetInt("quizCount") > 3) {
                    unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                } else {
                    unlockedStatusText.text = "<color=red>BLOQUEADO</color>";
                }
                break;
            default:
                descriptionText.text = "Sin ropa\nDesbloquea más cosméticos al completar retos.";
                unlockedStatusText.text = "<color=green>DESBLOQUEADO</color>";
                break;
        }
    }

    public void checCosmeticsUnlocked() {
        for(int i = 0; i < currentCosmetics.Length; i++) {
            switch(currentCosmetics[i]) {
            case 0:
                break;
            case 1:
                if(PlayerPrefs.GetInt("loginCount") < 3) currentCosmetics[i] = 0;
                break;
            case 2:
                if(PlayerPrefs.GetInt("videoCount") < 3) currentCosmetics[i] = 0;
                break;
            case 3:
                if(PlayerPrefs.GetInt("menuCount") < 3) currentCosmetics[i] = 0;
                break;
            case 4:
                if(PlayerPrefs.GetInt("quizCount") < 3) currentCosmetics[i] = 0;
                break;
            default:
                break;
            }
        }
    }
}
