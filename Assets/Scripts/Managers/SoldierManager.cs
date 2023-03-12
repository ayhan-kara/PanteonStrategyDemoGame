using System.Collections;
using UnityEngine;



public class SoldierManager : SoldierFeatures
{
    bool touched = false;
    int x;
    int y;
    [SerializeField] GridAI gridAI;
    [SerializeField] GameObject soldierPanel;
    bool canAttackDelay = false;
    float attackDelay = 0.0f;

    #region Monobehaviour

    private void Update()
    {
        if (canAttackDelay)
        {
            attackDelay += Time.deltaTime;
            if (attackDelay >= .0000000001f) 
            {
                canAttackDelay = false;
                attackDelay = 0.0f;
            }
        }
    }

    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GridStats"))
        {
            x = other.GetComponent<GridStats>().x;
            y = other.GetComponent<GridStats>().y;
        }
        if (other.CompareTag("Builded") && !other.GetComponent<ObjectDrag>().isDrag)
        {
            touched = true;
            gridAI.attack = false;
            gridAI.path.Clear();
            gridAI.startX = x;
            gridAI.startY = y;
        }
        if(other.CompareTag("Selected") && !other.GetComponent<ObjectDrag>().isDrag)
        {
            Debug.LogWarning("Same");
            SetDamage(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Builded") && !other.GetComponent<ObjectDrag>().isDrag)
        {
            canAttackDelay = true;
            SetDamage(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        touched = false;
    }

    #endregion

    void SetDamage(GameObject other)
    {
        if (canAttackDelay && other.GetComponent<BuildingFeatures>().health > 0)
        {
            other.GetComponent<BuildingFeatures>().health -= damage;
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }


    #region Mouse Functions
    private void OnMouseDown()
    {
        this.gameObject.tag = "SelectedSoldier";
        gridAI.selectedSoldier = this.gameObject;
        SetPath();
        if(!soldierPanel.activeInHierarchy)
        soldierPanel.SetActive(true);
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (tag != "Selected")
                tag = "Selected";
            GridAI.instance.endX = x;
            GridAI.instance.endY = y;
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        GridAI.instance.attackDistance = true;
        yield return new WaitForSeconds(.5f);
        GridAI.instance.attack = true;
        yield break;
    }

    public void SetPath()
    {
        gridAI.startX = x; 
        gridAI.startY = y;
    }

    #endregion
}
