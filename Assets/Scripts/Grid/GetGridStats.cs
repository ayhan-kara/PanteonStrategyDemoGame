using UnityEngine;

public class GetGridStats : MonoBehaviour
{
    public int x;
    public int y;
    public int visited = -1;

    GridStats gridStats;

    void Start()
    {
        gridStats = this.GetComponent<GridStats>();    

        x = gridStats.x;
        y = gridStats.y;
    }
}
