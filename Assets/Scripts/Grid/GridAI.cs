using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAI : MonoBehaviour
{
    public static GridAI instance;

    public Vector3 leftBottom = new Vector3(0, 0, 0);
    [Space]
    [Header("Boolean")]
    public bool findDistance = false;
    public bool move = false;
    public bool attack = false;
    public bool attackDistance = false;
    [Space]
    [Header("Grid")] 
    public int rows;
    public int columns;
    public float scale;
    public float speed;
    [Space]
    [Header ("Coordinates")]
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public GameObject selectedSoldier;
    public GameObject[,] gridArray;
    public List<GameObject> path = new List<GameObject>();
    Coroutine MoveAI;
    Coroutine AttackAI;

    [SerializeField] Pooler gridPooler;
    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if(gridPooler)
        GenerateGrid();
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
        if (move)
        {
            if(selectedSoldier != null)
            StartCoroutine(Move());
            if (!move)
            {
                selectedSoldier.transform.position += new Vector3(0, .5f, 0);
            }
        }
        if(attackDistance)
        {
            if (selectedSoldier != null)
            SetAttackDistance();
            SetPathAndAttack();
            attackDistance = false;
        }
        if (attack)
        {
            if (selectedSoldier != null)
            StartCoroutine(Attack());
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++) 
        {
            for(int j = 0; j < rows; j++)
            {
                GridStats gridStats = GetGrid();

                gridStats.transform.position = new Vector3(leftBottom.x + scale * i, leftBottom.y, leftBottom.z + scale * j);
                gridStats.transform.SetParent(gameObject.transform);
                gridStats.GetComponent<GridStats>().x = i;
                gridStats.GetComponent<GridStats>().y = j;
                gridStats.gameObject.SetActive(true);
                gridArray[i,j] = gridStats.gameObject;
            }
        }
    }

    private GridStats GetGrid()
    {
        return gridPooler.GetGo<GridStats>();
    }

    void SetDistance()
    {
        Setup();

        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                var comp = obj.GetComponent<GridStats>();
                if (comp != null && obj && comp.visited == step - 1)
                    TestFourDirections(comp.x, comp.y, step);
            }
        }
    }
    void SetAttackDistance()
    {
        StatsSetup();
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                var comp = obj.GetComponent<GetGridStats>();
                if (comp != null && comp && comp.visited == step - 1)
                    StatsTestFourDirection(comp.x, comp.y, step);
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX,endY].GetComponent<GridStats>() != null && gridArray[endX,endY] && gridArray[endX,endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[x,y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            print("Can't reach location");
            return;
        }

        for (int i = step; step > -1; step--)
        {
            if (FindDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (FindDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (FindDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (FindDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);
            GameObject tempObj = FindCloset(gridArray[endX, endY].transform, tempList);
            var tempGrid = tempObj.GetComponent<GridStats>();
            if (tempGrid != null)
                path.Add(tempObj);
            x = tempGrid.x;
            y = tempGrid.y;
            tempList.Clear();
        }
    }

    void SetPathAndAttack()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX, endY].GetComponent<GetGridStats>() != null && gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GetGridStats>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GetGridStats>().visited - 1;
        }
        else
        {
            print("Can't reach location");
            return;
        }

        for (int i = step; step > -1; step--)
        {
            if (StatsFindDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (StatsFindDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (StatsFindDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (StatsFindDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);
            GameObject tempObj = FindCloset(gridArray[endX, endY].transform, tempList);
            var tempGrid = tempObj.GetComponent<GetGridStats>();
            if (tempGrid != null)
                path.Add(tempObj);
            x = tempGrid.x;
            y = tempGrid.y;
            tempList.Clear();
        }
    }

    void Setup()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj.GetComponent<GridStats>() != null)
            obj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
    }
    void StatsSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj.GetComponent<GetGridStats>() != null)
                obj.GetComponent<GetGridStats>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GetGridStats>().visited = 0;
    }

    bool FindDirection(int x, int y, int step, int direction)
    {
        switch (direction) 
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1 ,y].GetComponent<GridStats>() != null && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1  && gridArray[x, y - 1].GetComponent<GridStats>() != null && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y].GetComponent<GridStats>() != null && gridArray[x + 1, y] && gridArray[x + 1,y].GetComponent<GridStats>().visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 < rows && gridArray[x , y + 1].GetComponent<GridStats>() != null && gridArray[x, y + 1] && gridArray[x ,y + 1].GetComponent<GridStats>().visited == step)
                    return true;
            else
                    return false;
        }
        return false;
    }

    bool StatsFindDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y].GetComponent<GetGridStats>() != null && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GetGridStats>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1].GetComponent<GetGridStats>() != null && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GetGridStats>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y].GetComponent<GetGridStats>() != null && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GetGridStats>().visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1].GetComponent<GetGridStats>() != null && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GetGridStats>().visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (FindDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (FindDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);
        if (FindDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (FindDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }

    void StatsTestFourDirection(int x, int y, int step) 
    {
        if (StatsFindDirection(x, y, -1, 1))
            StatsSetVisited(x, y + 1, step);
        if (StatsFindDirection(x, y, -1, 2))
            StatsSetVisited(x + 1, y, step);
        if (StatsFindDirection(x, y, -1, 3))
            StatsSetVisited(x, y - 1, step);
        if (StatsFindDirection(x, y, -1, 4))
            StatsSetVisited(x - 1, y, step);
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStats>().visited = step;

    }

    void StatsSetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x,y].GetComponent<GetGridStats>().visited = step;
    }

    GameObject FindCloset(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }

        return list[indexNumber];
    }

    IEnumerator Move()
    {
        for (int i = path.Count; i >= 0; i--)
        {
            MoveAI = StartCoroutine(Moving(i));
            yield return MoveAI;
        }
    }
    IEnumerator Moving(int current)
    {
        if(selectedSoldier.transform.position != path[current].transform.position)
        {
            yield return new WaitForSeconds(.2f);
            selectedSoldier.transform.position = Vector3.Lerp(selectedSoldier.transform.position, path[current].transform.position, speed * Time.deltaTime);
            yield return null;
            move = false;
        }
    }
    IEnumerator Attack()
    {
        for (int i = path.Count; i >= 0; i--)
        {
            AttackAI = StartCoroutine(Attacking(i));
            yield return AttackAI;
        }
    }
    IEnumerator Attacking(int current)
    {
        if (selectedSoldier.transform.position != path[current].transform.position)
        {
            yield return new WaitForSeconds(.2f);
            selectedSoldier.transform.position = Vector3.Lerp(selectedSoldier.transform.position, path[current].transform.position, speed * Time.deltaTime);
            yield return null;
            attack = false;
        }
    }
}
