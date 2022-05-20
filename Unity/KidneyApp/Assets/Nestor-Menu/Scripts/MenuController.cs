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
    private string persistentPath;

    public Transform scrollRectContent;

    public Button aliment;

    FoodDataResponse data;

    [SerializeField] private string foodUpdateEndpoint = "http://127.0.0.1:80/account/updateClientFood";


    Values values;

    //Hardcoded af

    public int IMC;

    public bool creatingMenu;

    public RecipeData dummyMenu;

    void Start()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "Data/alimentsJSON.json";

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Data/alimentsJSON.json";

        data = LoadData();

    }


    public void testJSON(){

        FoodDataResponse data = LoadData();

    }

    public void PopulateAliments(){

        for (int i = 0; i < data.foodData.Length; i++){

                Button listAliment = Instantiate(aliment, new Vector3(0,0,0), Quaternion.identity);
                listAliment.transform.SetParent(scrollRectContent.transform,false);
                listAliment.GetComponentInChildren<TextMeshProUGUI>().text = data.foodData[i].category + " - "  + data.foodData[i].name;
                if(SelecFromIMC(data,i, IMC)==0) listAliment.GetComponent<Image>().color = Color.red;
                else if(SelecFromIMC(data,i, IMC)==1) listAliment.GetComponent<Image>().color = Color.yellow;
                if(SelecFromIMC(data,i, IMC)==2) listAliment.GetComponent<Image>().color = Color.green;

        }
    }

    private void ChangeButtonColor(Button button){
    }

     public void SaveData(string data)
    {
        FileInfo fi = new FileInfo(persistentPath);
        if(!fi.Directory.Exists) System.IO.Directory.CreateDirectory(fi.DirectoryName);

        using StreamWriter writer = new StreamWriter(persistentPath);
        
        writer.Write(data);
    }

     public FoodDataResponse LoadData()
    {
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();

        return JsonUtility.FromJson<FoodDataResponse>(json);
    }

    public void ClearJSON()
    {
        Debug.Log("Clearing JSON data");
        FileInfo fi = new FileInfo(path);
        fi.Directory.Delete(true);
    }

    public int SelecFromIMC(FoodDataResponse data, int position, int IMC){

        if(IMC == 1) return data.foodData[position].values.C1;
        else if (IMC == 2) return data.foodData[position].values.C2;
        else if (IMC == 3) return data.foodData[position].values.C3;
        else if (IMC == 4) return data.foodData[position].values.C4;
        else if (IMC == 5) return data.foodData[position].values.C5;
        else if (IMC == 6) return data.foodData[position].values.C6;
        else if (IMC == 7) return data.foodData[position].values.C7;
        else if (IMC == 8) return data.foodData[position].values.C8;
        else if (IMC == 9) return data.foodData[position].values.C9;
        else if (IMC == 10) return data.foodData[position].values.C10;
        else if (IMC == 11) return data.foodData[position].values.C11;
        else if (IMC == 12) return data.foodData[position].values.C12;
        else if (IMC == 13) return data.foodData[position].values.C13;
        else if (IMC == 14) return data.foodData[position].values.C14;
        else if (IMC == 15) return data.foodData[position].values.C15;
        else if (IMC == 16) return data.foodData[position].values.C16;
        else if (IMC == 17) return data.foodData[position].values.C17;
        else if (IMC == 18) return data.foodData[position].values.C18;
        else if (IMC == 19) return data.foodData[position].values.C19;
        else if (IMC == 20) return data.foodData[position].values.C20;
        else if (IMC == 21) return data.foodData[position].values.C21;
        else if (IMC == 22) return data.foodData[position].values.C22;
        else if (IMC == 23) return data.foodData[position].values.C23;
        else if (IMC == 24) return data.foodData[position].values.C24;

        else return 0;
    }

    public void testUpdate(){
        StartCoroutine(UpdateLocalFoodJSON());
    }


    public void CreateMenu(){


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

            FoodDataResponse alimentData = JsonUtility.FromJson<FoodDataResponse>("{\"foodData\":" + request.downloadHandler.text + "}");

            SaveData("{\"foodData\":" + request.downloadHandler.text + "}");
            
            Debug.Log(alimentData.foodData[0].name);
        }             
        yield return null;
    }
}
