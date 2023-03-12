using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GridAI.instance.endX = this.x;
            GridAI.instance.endY = this.y;
            StartCoroutine(StartPath());
        }
    }

    IEnumerator StartPath()
    {
        GridAI.instance.findDistance = true;
        yield return new WaitForSeconds(.5f);
        GridAI.instance.move = true;
        GridAI.instance.startX = GridAI.instance.endX;
        GridAI.instance.startY = GridAI.instance.endY;
        yield break;
    }
}
