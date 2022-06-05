using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{


    public MenuData identity;

    public bool selected = false;


    public void Interact(){


        if(MenuController.Instance.currenState == MenuController.CurrenState.LookingMenu){


            /*foreach(FoodData data in MenuController.Instance.newMenu.aliments){

                Debug.Log(data.name);

                    if(data == identity){
                        MenuController.Instance.removeAliment(MenuController.Instance.newMenu, identity);
                        Debug.Log("Encontrado");
                    } 
                    
            }*/
            
            MenuController.Instance.OnMenuClick(identity, selected);
            

        } 


        

        //else display info
    }




}
