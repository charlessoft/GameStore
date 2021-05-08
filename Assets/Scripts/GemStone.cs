using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStone : MonoBehaviour
{
    public int rowIndex = 0;  //行号
    public int columnIndex = 0; //列号
    public float xOffset = -5.5f; //x轴方向上偏移
    public float yOffset = -2.5f; //y轴方向上偏移
    // Start is called before the first frame update

    public GameObject[] gemstoneBgs;
    public int gemstoneType;
    private GameObject gemStoneBg;
    private GameController gameController;
    private SpriteRenderer renderer;
    public bool isSelect
    {
        set
        {
            if (value)
                renderer.color = Color.red;
            else
                renderer.color = Color.white;
        }
    }
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        renderer = this.gemStoneBg.GetComponent<SpriteRenderer>();
//         if (value)
//             renderer.color = Color.red;
//         else
//             renderer.color = Color.white;

    }

    public void Dispose()
    {
        Destroy(this.gameObject);
        Destroy(gemStoneBg.gameObject);
        gameController = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePosition(int _rowIndex,int _columnIndex) //宝石位置做调整
    {
        this.rowIndex = _rowIndex;
        this.columnIndex = _columnIndex;
        this.transform.position = new Vector3(columnIndex*1.2f + xOffset, rowIndex * 1.2f + yOffset, 0);
    }

    public void TewwnToPosition(int _rowIndex, int _columnIndex)
    {
        rowIndex = _rowIndex;
        columnIndex = _columnIndex;
        iTween.MoveTo(this.gameObject, iTween.Hash("x", columnIndex * 1.2f + xOffset,
            "y", rowIndex * 1.2f + yOffset, "time", 0.5f));
    }

    /// <summary>
    /// 生成随机宝石类型
    /// </summary>
    public void RandomCreateGemStoneBg()
    {
        if (this.gemStoneBg != null)
            return;
        gemstoneType = Random.Range(0, this.gemstoneBgs.Length);
        gemStoneBg = Instantiate(gemstoneBgs[gemstoneType]) as GameObject;
        gemStoneBg.transform.parent = this.transform;

    }

    public void IsSelected(bool value)
    {
        

    }
    public void OnMouseDown()
    {
        gameController.Select(this);
    }
}
