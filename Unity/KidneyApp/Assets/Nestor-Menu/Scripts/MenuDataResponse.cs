using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuData
{   
    public string _id {get;set;}
    public string name {get;set;}
    public string author {get;set;}

    public FoodData [] aliments {get;set;} = new FoodData[50];
    public string description {get;set;}
    public int score {get;set;}
    public int IMCValue {get;set;}
}

[System.Serializable]
public class MenuDataResponse {

    public MenuData [] menuData {get;set;}
}
