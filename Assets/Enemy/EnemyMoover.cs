using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMoover : MonoBehaviour
{
    //Range implementa uno slider limitato
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake(){
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void RecalculatePath(bool resetPath){
        Vector2Int coordinates = new Vector2Int();

        if(resetPath){
            coordinates = pathFinder.StartCoordinates;
        }
        else{
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        //Lo metto qui per stoppare la coroutine prima di ottenere il nuovo path
        StartCoroutine(FollowPath());
    }

    //Fa spawnare nel blocco di partenza
    void ReturnToStart(){
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishedPath(){
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath(){
        //Partire da 1 elimina il problema del rallenty durante il ricalcolo
        for(int i = 1; i < path.Count; i++){
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            //1 Rappresenta il 100%
            while(travelPercent < 1f){
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame(); 
            }
        }

        //Ha concluso il percorso
        FinishedPath();
    }
}
