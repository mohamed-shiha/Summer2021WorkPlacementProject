using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Teams
{
    Red,
    Blue,
    Black,
    Green
}
public class SpawnPoint : MonoBehaviour
{
    public Teams Team;
    public Transform[] Positions;
    private Queue<Vector3> queue = new Queue<Vector3>();

    public Vector3 GetNewPosition()
    {
        if (queue.Count == 0)
            foreach (var pos in Positions)
                queue.Enqueue(pos.position);
        Vector3 newPos = queue.Dequeue();
        queue.Enqueue(newPos);

        // Debug.Log(SceneManager.GetActiveScene().name);
        //Debug.Log("New location" + newPos);
        return newPos;
    }
}
