using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FoodDataResponse
{
    public FoodData [] foodData;
}

[System.Serializable]
public class FoodData : System.IEquatable<FoodData>{

    public string _id {get;set;}
    public string name {get;set;}
    public string category {get;set;}
    public string language {get;set;}
    public Values values {get;set;}



    public bool Equals(FoodData other){
        return this._id == other._id;
    }
    
}

[System.Serializable]
public class Values{
        public int C1 {get;set;}
        public int C2 {get;set;}
        public int C3 {get;set;}
        public int C4 {get;set;}
        public int C5 {get;set;}
        public int C6 {get;set;}
        public int C7 {get;set;}
        public int C8 {get;set;}
        public int C9 {get;set;}
        public int C10 {get;set;}
        public int C11 {get;set;}
        public int C12 {get;set;}
        public int C13 {get;set;}
        public int C14 {get;set;}
        public int C15 {get;set;}
        public int C16 {get;set;}
        public int C17 {get;set;}
        public int C18 {get;set;}
        public int C19 {get;set;}
        public int C20 {get;set;}
        public int C21 {get;set;}
        public int C22 {get;set;}
        public int C23 {get;set;}
        public int C24 {get;set;}
}

