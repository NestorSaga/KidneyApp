using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FoodDataResponse
{
    public FoodData [] foodData;
}

[System.Serializable]
public class FoodData{

    public string _id;
    public string name;
    public string category;
    public string language;
    public Values values;
    
}

[System.Serializable]
public class Values{
        public int C1;
        public int C2;
        public int C3;
        public int C4;
        public int C5;
        public int C6;
        public int C7;
        public int C8;
        public int C9;
        public int C10;
        public int C11;
        public int C12;
        public int C13;
        public int C14;
        public int C15;
        public int C16;
        public int C17;
        public int C18;
        public int C19;
        public int C20;
        public int C21;
        public int C22;
        public int C23;
        public int C24;
}

