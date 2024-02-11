using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] Items = new Item[4];
    public Item selectedItem;

    private void Update(){
        if(numberClicked() != 0){
            selectedItem = Items[numberClicked() - 1];
        }
        Debug.Log(selectedItem);
    }

    private int numberClicked(){
        if(Input.GetKeyDown("1")){
            return 1;
        }
        if(Input.GetKeyDown("2")){
            return 2;
        }
        if(Input.GetKeyDown("3")){
            return 3;
        }
        if(Input.GetKeyDown("4")){
            return 4;
        }
        return 0;
    }
}
