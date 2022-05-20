using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.Networking;

public class MenuController : MonoBehaviour
{

    private string path;

    public Transform scrollRectContent;

    public Button aliment;

    AlimentData data;

    [SerializeField] private string foodUpdateEndpoint = "http://127.0.0.1:80/account/updateClientFood";



    //Hardcoded af

    public int IMC;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "Nestor-menu/Data/alimentsJSON.json";

        data = LoadData();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void testJSON(){

        AlimentData data = LoadData();
        Debug.Log(data.meat[5].Aliment);
        Debug.Log(data.fish[0].Aliment);
        Debug.Log(data.deliMeats[1].Aliment);
        Debug.Log(data.dairy[1].Aliment);

    }

    public void PopulateAliments(){

        

        for (int i = 0; i < data.meat.Length; i++){
                Button listAliment = Instantiate(aliment, new Vector3(0,0,0), Quaternion.identity);
                listAliment.transform.SetParent(scrollRectContent.transform,false);
                listAliment.GetComponentInChildren<TextMeshProUGUI>().text = data.meat[i].Aliment;
                if(SelecFromIMC(data,i,"meat", IMC)==0) listAliment.GetComponent<Image>().color = Color.red;
                else if(SelecFromIMC(data,i,"meat", IMC)==1) listAliment.GetComponent<Image>().color = Color.yellow;
                if(SelecFromIMC(data,i,"meat", IMC)==2) listAliment.GetComponent<Image>().color = Color.green;

        }
    }


    private void ChangeButtonColor(Button button){
    }

     public AlimentData LoadData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        return JsonUtility.FromJson<AlimentData>(json);
    }

    public void ClearJSON()
    {
        Debug.Log("Clearing JSON data");
        FileInfo fi = new FileInfo(path);
        fi.Directory.Delete(true);
    }

    public int SelecFromIMC(AlimentData data, int position, string alimentFamily, int IMC){

        switch(alimentFamily){

            case "meat":
                if(IMC==1) return data.meat[position].C1;
                else if(IMC==2) return data.meat[position].C2;
                else return 1;
            break;

            case "fish":
            if(IMC==1) return data.fish[position].C1;
            else return 1;
            break;

            default: 
                return 1;
        }
    }

    public void testUpdate(){
        StartCoroutine(UpdateLocalFoodJSON());
    }

    public IEnumerator UpdateLocalFoodJSON(){

        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Post(foodUpdateEndpoint, form);

        var handler = request.SendWebRequest();

        float startTime= 0.0f;
        while (!handler.isDone){
            startTime += Time.deltaTime;

            if(startTime > 10.0f) {
                Debug.Log("Connection timeout at login");
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success) {


            Debug.Log(request.downloadHandler.text);

            FoodDataResponse alimentData = JsonUtility.FromJson<AlimentData>(value2);
            
            Debug.Log(alimentData);
        }             
        yield return null;
    }
}
