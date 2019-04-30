using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dot : MonoBehaviour
{
    public Cell cell;
    public int row, col;
    private Color color;
    private DotManager manager;
    public bool markedForRemoval;
    public DotSelectionTween currentSelectionTween;

    private void Awake()
    {
        // The initial position should be above the board, until it is set to its cell position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 1.5f, 0));
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    }

    public void SetParameters(DotManager manager, Color color)
    {
        SetManager(manager);
        SetColor(color);
    }

    public void SetCell(Cell cell)
    {
        if (this.cell != null) this.cell.dot = null; 

        this.cell = cell;
        cell.dot = this;
        row = cell.row;
        col = cell.col;

        SetPosition(); // Once the cell is set, move to the cell
    }

    public void SetPosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cell.x, cell.y, 0));

        // Tween to location
        transform.DOMoveX(worldPosition.x, 0f);
        transform.DOMoveY(worldPosition.y, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.1f);
    }

    private void SetManager(DotManager manager)
    {
        if (this.manager == null) this.manager = manager;
    }

    void OnMouseDown()
    {
        manager.OnDotMouseDown(this);
    }

    private void OnMouseEnter()
    {
        manager.OnDotMouseEnter(this);
    }

    private void OnMouseExit()
    {
        manager.OnDotMouseExit(this);
    }

    private void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
        this.color = color;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public void Destroy()
    {
        markedForRemoval = true;
        transform.DOScale(0f, 0.2f).OnComplete(()=>Destroy(gameObject));

        if (currentSelectionTween != null)
        {
            Destroy(currentSelectionTween.gameObject);
        }
    }

}