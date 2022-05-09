using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MenuController : MonoBehaviour
{

    private string path;

    public Transform scrollRectContent;

    public Button aliment;

    AlimentData data;



    //Hardcoded af

    public int IMC = 11;

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

}
