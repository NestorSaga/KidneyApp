using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;


public class MenuController : MonoBehaviour
{

    public enum CurrenState{
        LookingAliments, CreatingMenu,  LookingMenu, EditingMenu
    }

    public CurrenState currenState;

    private string alimentPath;
    private string alimentPersistentPath;

    public Transform scrollRectAlimentList;
    public Transform scrollRectMenuAlimentList;
    public Transform scrollRectNewMenuList;
    public Transform scrollRectAllMenuList;

    public Transform scrollRectAlimentList_Content;
    public Transform scrollRectMenuAlimentList_Content;
    public Transform scrollRectNewMenuList_Content;
    public Transform scrollRectAllMenuList_Content;



    int menuMaxCapacity = 50;

    public Button newMenuButton;

    public TMP_Text currentStateText;
    public TMP_Text menuTutorialText;

    public TMP_Text newMenuName;
    public TMP_Text newMenuDescription;


    public Button aliment;

    FoodDataResponse foodData;

    FoodDataResponse menuAllAliments;

    FoodData menuSingleAliment;

    FoodData [] menuAlimentList;

    public MenuData  newMenu;

    MenuDataResponse myAllMenus;



    MenuDataResponse myOwnMenus;


    JSONNode nestedJSON;

    



    [SerializeField] private string foodUpdateEndpoint = "http://127.0.0.1:80/account/updateClientFood";

    [SerializeField] private string getAllValidRecipesEndpoint = "http://127.0.0.1:80/account/getAllValidRecipes";


    Values values;

    //Hardcoded af

    public int IMC;

    private int currentAlimentInRecipeCounter;
    private int currentRecipeInListCounter;

    public bool creatingMenu;

    public MenuData dummyMenu;


    private static MenuController instance = null;
    public static MenuController Instance {
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

        alimentPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Data/alimentsJSON.json";

        alimentPersistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Data/alimentsJSON.json";
    }
    void Start()
    {
        

        InicializeMenuScreen();

        myAllMenus = new MenuDataResponse();

        //foodData = LoadFoodData();

        myAllMenus = new MenuDataResponse(){
                menuData = new MenuData[menuMaxCapacity]
            };

    }

    public void ChangeState(string newState){

        switch(newState){


            //Display Aliment List and disable others
            case "LookingAliments":
                currenState = CurrenState.LookingAliments;
                currentStateText.text = "Aliment List";
                scrollRectAlimentList.gameObject.SetActive(true);
                scrollRectMenuAlimentList.gameObject.SetActive(false);
                scrollRectNewMenuList.gameObject.SetActive(false);
                scrollRectAllMenuList.gameObject.SetActive(false);
                LookingAliments();

                
                break;

            //Display Menu List and Aliment List disable others
            case "LookingMenu":
                currenState = CurrenState.LookingMenu;
                currentStateText.text = "Menu List";
                
                break;
            
            //Display Menu List and Aliment List disable others
            case "EditingMenu":
                currenState = CurrenState.EditingMenu;
                 currentStateText.text = "Editing Menu";
                
                break;
            
            //Display Aliment List and newMenu List disable others
            case "CreatingMenu":
                currenState = CurrenState.CreatingMenu;
                currentStateText.text = "New Menu";
                scrollRectAlimentList.gameObject.SetActive(true);
                scrollRectMenuAlimentList.gameObject.SetActive(false);
                scrollRectNewMenuList.gameObject.SetActive(true);
                scrollRectAllMenuList.gameObject.SetActive(false);

                newMenu = new MenuData();
                
                break;
            
        }

    }


    public void testJSON(){

        FoodDataResponse foodData = LoadFoodData();

    }

    public void InicializeMenuScreen(){

        foodData = LoadFoodData();

        if(foodData.foodData[0]._id==null) UpdateAlimentJSON();

    }

    //Populate initial Aliment list based on AlimentJSON
    public void PopulateAliments(){

        foreach (Transform child in scrollRectAlimentList_Content.transform){
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < foodData.foodData.Length; i++){

                Button listAliment = Instantiate(aliment, new Vector3(0,0,0), Quaternion.identity);
                listAliment.transform.SetParent(scrollRectAlimentList_Content.transform,false);
                listAliment.GetComponentInChildren<TextMeshProUGUI>().text = foodData.foodData[i].category + " - "  + foodData.foodData[i].name;
                if(SelecFromIMC(foodData.foodData[i], IMC)==0) listAliment.GetComponent<Image>().color = Color.red;
                else if(SelecFromIMC(foodData.foodData[i], IMC)==1) listAliment.GetComponent<Image>().color = Color.yellow;
                if(SelecFromIMC(foodData.foodData[i], IMC)==2) listAliment.GetComponent<Image>().color = Color.green;

                listAliment.GetComponent<AlimentButton>().identity = foodData.foodData[i];
                //menuAlimentList[0] = foodData.foodData[i];

        }
    }

    

    public void LookingAliments(){
        PopulateAliments();
    }

    //Select IMC from Aliment List
    public int SelecFromIMC(FoodData data, int IMC){


        if(IMC == 1) return data.values.C1;
        else if (IMC == 2) return data.values.C2;
        else if (IMC == 3) return data.values.C3;
        else if (IMC == 4) return data.values.C4;
        else if (IMC == 5) return data.values.C5;
        else if (IMC == 6) return data.values.C6;
        else if (IMC == 7) return data.values.C7;
        else if (IMC == 8) return data.values.C8;
        else if (IMC == 9) return data.values.C9;
        else if (IMC == 10) return data.values.C10;
        else if (IMC == 11) return data.values.C11;
        else if (IMC == 12) return data.values.C12;
        else if (IMC == 13) return data.values.C13;
        else if (IMC == 14) return data.values.C14;
        else if (IMC == 15) return data.values.C15;
        else if (IMC == 16) return data.values.C16;
        else if (IMC == 17) return data.values.C17;
        else if (IMC == 18) return data.values.C18;
        else if (IMC == 19) return data.values.C19;
        else if (IMC == 20) return data.values.C20;
        else if (IMC == 21) return data.values.C21;
        else if (IMC == 22) return data.values.C22;
        else if (IMC == 23) return data.values.C23;
        else if (IMC == 24) return data.values.C24;

        else return 0;
    }

    public void OnAlimentClick(FoodData aliment){


        if(currenState==CurrenState.CreatingMenu){


            foreach(Transform child in scrollRectNewMenuList_Content.transform){
                    GameObject.Destroy(child.gameObject);
            }

            List<FoodData> list;list = new List<FoodData>();

            if(newMenu.aliments[0]!=null){//is NOT empty

                Debug.Log("Not empty");

                foreach(FoodData food in newMenu.aliments){
            
                 if(food.name!=aliment.name){
                     list.Add(food);  
                     Debug.Log(food.name + " added");
                 } else{
                     list.Remove(aliment);
                     
                     //RemoveAliment(newMenu,aliment, scrollRectNewMenuList_Content);
                     
                     Debug.Log("Removed " + aliment.name);
                 }
                    list.Add(aliment); 
                }

                
            }else{
                Debug.Log("Add");
                list.Add(aliment);
            }           

            MenuData dummy = new MenuData();
            dummy.aliments = list.ToArray();
            newMenu = dummy;

            foreach(FoodData data in newMenu.aliments)
                Debug.Log(data.name);

            CreateMenu(newMenu);

        }

        //Si no está en la lista, añadir
        //else, quitar

    }

    public void CreateMenu(MenuData currentMenu){

        

        for(int i=0;i<newMenu.aliments.Length;i++){

        Debug.Log("Hay " + i + " alimentos en el newMenu");
                                
        Button listAliment = Instantiate(aliment, new Vector3(0,0,0), Quaternion.identity);
        listAliment.transform.SetParent(scrollRectNewMenuList_Content.transform,false);
        listAliment.GetComponentInChildren<TextMeshProUGUI>().text = currentMenu.aliments[i].category + " - "  + currentMenu.aliments[i].name;
        if(SelecFromIMC(currentMenu.aliments[i], IMC)==0) listAliment.GetComponent<Image>().color = Color.red;
        else if(SelecFromIMC(currentMenu.aliments[i], IMC)==1) listAliment.GetComponent<Image>().color = Color.yellow;
        if(SelecFromIMC(currentMenu.aliments[i], IMC)==2) listAliment.GetComponent<Image>().color = Color.green;

        }
    }


    public void EndNewMenuCreation(){

        newMenu.author = "Carmen";
        newMenu.name = newMenuName.text;
        newMenu.description = newMenuDescription.text;

        List<MenuData> list = new List<MenuData>();

        if(myAllMenus.menuData[0]!=null){ //is NOT empty

            foreach(MenuData menu in myAllMenus.menuData){
                list.Add(menu);
            }

        }else{
            list.Add(newMenu);
        }

        MenuDataResponse dummy = new MenuDataResponse();
        dummy.menuData = list.ToArray();

        myAllMenus = dummy;

        newMenuName.text = "";
        newMenuDescription.text = "";

        //ChangeState(CurrenState.LookingMenu);


    }

     public void SaveData(string data)
    {
        FileInfo fi = new FileInfo(alimentPersistentPath);
        if(!fi.Directory.Exists) System.IO.Directory.CreateDirectory(fi.DirectoryName);

        using StreamWriter writer = new StreamWriter(alimentPersistentPath);
        
        writer.Write(data);
    }

     public FoodDataResponse LoadFoodData()
    {
        using StreamReader reader = new StreamReader(alimentPersistentPath);
        string json = reader.ReadToEnd();

        return JsonUtility.FromJson<FoodDataResponse>(json);
    }

    public void ClearJSON()
    {
        Debug.Log("Clearing JSON data");
        FileInfo fi = new FileInfo(alimentPersistentPath);
        fi.Directory.Delete(true);
    }


    public void UpdateAlimentJSON(){
        StartCoroutine(UpdateLocalFoodJSON());
    }


    public void testSimpleJSON(){


        StartCoroutine(GetAllValidRecipes());


    }

    public void FromMenusToJSON(MenuDataResponse menus){

        var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(myAllMenus);

    }

    public void FromJSONtoMenu(string json){

         nestedJSON = JSON.Parse(json);


            

            Debug.Log(nestedJSON);
            for(int i = 0; i<nestedJSON.Count; i++){

                
                string idJSON = nestedJSON[i]["_id"].Value;
                string authorJSON = nestedJSON[i]["author"].Value;
                string nameJSON = nestedJSON[i]["name"].Value;
                string descJSON = nestedJSON[i]["description"].Value;
                int IMCJSON = nestedJSON[i]["IMCValue"].AsInt;
                int scoreJSON = nestedJSON[i]["score"].AsInt;
                

                int numberOfAliments = nestedJSON[i]["aliments"].Count;

                Debug.Log(nestedJSON.Count + " numero de recetas en total");

                myAllMenus.menuData[i] = new MenuData(){
                    _id = idJSON,
                    author = authorJSON,
                    name = nameJSON,
                    description = descJSON,
                    IMCValue = IMCJSON,
                    score = scoreJSON,
                    aliments = new FoodData[numberOfAliments]
                };
         
                for(int j=0; j<numberOfAliments; j++){
                        
                    string idJSONA = nestedJSON[i]["aliments"][i]["_id"].Value;
                    string nameJSONA = nestedJSON[i]["aliments"][i]["name"].Value;
                    string categoryJSONA = nestedJSON[i]["aliments"][i]["category"].Value;
                    string languageJSONA = nestedJSON[i]["aliments"][i]["language"].Value;

                    Debug.Log(numberOfAliments + " Alimentos en " + nestedJSON[i]["name"].Value);


                    string quotation = "\"";
                    string allValues ="";

                    int counter = 1;
             
                    foreach (JSONNode item in nestedJSON[i]["aliments"][j]["values"].Children)
                    {
                    string toAdd = quotation + "C" + counter + quotation + ":" + item + ",";
                    allValues += toAdd;
                    counter++;
                    }

                    string result = allValues.Remove(allValues.Length-1);
                    string final = "{" + quotation + "values" + quotation + ":{" + result + "}";

                    //Values valuesJSON = JsonUtility.FromJson<Values>(final+"}");
                    Values valuesJSON = Newtonsoft.Json.JsonConvert.DeserializeObject<Values>("{" + result + "}");

                    myAllMenus.menuData[i].aliments[j] = new FoodData(){
                        _id = idJSONA,
                        name = nameJSONA,
                        category = categoryJSONA,
                        language = languageJSONA,
                        values = valuesJSON
                        
                    };

                }
                
            }


    }

    public IEnumerator GetAllValidRecipes(){
        

        WWWForm form = new WWWForm();
        form.AddField("rIMC", IMC);

        UnityWebRequest request = UnityWebRequest.Post(getAllValidRecipesEndpoint, form);

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


           FromJSONtoMenu(request.downloadHandler.text);

           //newtonjson(request.downloadHandler.text);
              
        }             
        yield return null;
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

            foodData = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodDataResponse>("{\"foodData\":" + request.downloadHandler.text + "}");

            SaveData("{\"foodData\":" + request.downloadHandler.text + "}");
        }             
        yield return null;
    }
}
