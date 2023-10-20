using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//In questa versione ho tolto tutti i riferimenti a Waypoint

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    //Arancione
    //[SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);
    [SerializeField] Color pathColor = Color.blue;

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake(){
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinate();
    }

    // Update is called once per frame
    void Update()
    {
        //Sta ad indicare che verrà eseguito solo nell'editor
        if(!Application.isPlaying){
            DisplayCoordinate();
            UpdateObjectName();
            //Modalità di debug in cui vedo le coordinate
            label.enabled = true;
        }     

        SetLabelColor();  
        ToggleLabel();
    }

    void ToggleLabel(){
        if(Input.GetKeyDown(KeyCode.C)){
            label.enabled = !label.IsActive();
        }
    }

    void SetLabelColor(){
        if(gridManager == null){
            return;
        }

        Node node = gridManager.GetNode(coordinates);

        if(node == null){
            return;
        }

        if(!node.isWalkable){
            label.color = blockedColor;
        }
        else if(node.isPath){
            label.color = pathColor;
        }
        else if(node.isExplored){
            label.color = exploredColor;
        }
        else{
            label.color = defaultColor;
        }
    }

    void DisplayCoordinate(){
        if(gridManager == null){
            return;
        }

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName(){
        transform.parent.name = coordinates.ToString();
    }
}
