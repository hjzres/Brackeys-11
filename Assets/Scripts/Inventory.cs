using Items;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items = new Item[4];
    public Item selectedItem;

    private void Update(){
        if(NumberClicked() != 0){
            selectedItem = items[NumberClicked() - 1];
        }
        Debug.Log(selectedItem);
    }

    private int NumberClicked(){
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
