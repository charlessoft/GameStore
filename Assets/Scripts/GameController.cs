using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GemStone gemstone;
    public int rowNum = 7; //行数
    public int columNum = 10;//列数
    public ArrayList gemstoneList;
    public GemStone current;
    public ArrayList mathesGamestone;
    public AudioClip errorClip;
    public AudioClip swapClip;
    public AudioClip matchClip;
    // Start is called before the first frame update
    void Start()
    {
        gemstoneList = new ArrayList();
        mathesGamestone = new ArrayList();
        for(int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            for(int columnIndex = 0;columnIndex< columNum; columnIndex ++ )
            {
                GemStone c = this.AddGemstone(rowIndex, columnIndex);
                temp.Add(c);
            }
            gemstoneList.Add(temp);
        }

        if (CheckHorizontalMathes() || CheckVerticalMathes())
        {
            RemoveMathes();
        }


    }
    public  GemStone AddGemstone(int rowIndex,int columnIndex)
    {
        GemStone c = Instantiate(gemstone) as GemStone;
        c.transform.parent = this.transform;
        //c.UpdatePosition(rowIndex, columnIndex);
        c.GetComponent<GemStone>().RandomCreateGemStoneBg();
        c.GetComponent<GemStone>().UpdatePosition(rowIndex, columnIndex);

        return c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Select(GemStone c)
    {
        //Destroy(c.gameObject);
        if(current ==null)
        {
            current = c;
            Debug.Log(string.Format("x:{0},y:{1}", current.rowIndex, current.columnIndex));
            //current.IsSelected(true);
            current.isSelect = true;
            return;
        }
        else
        {
          if(Mathf.Abs(current.rowIndex-c.rowIndex) + Mathf.Abs(current.columnIndex-c.columnIndex) ==1)
            {
                // Exchange(current, c);
                ExangeAndMathes(current, c);


            }
            //current.IsSelected(false);
            current.isSelect = false;
            current = null;
        }
    }
    void ExangeAndMathes(GemStone c1,GemStone c2)
    {
        Exchange(c1, c2);
        if(CheckHorizontalMathes() || CheckVerticalMathes())
        {
            RemoveMathes();
        }
        else
        {
            Exchange(c2, c1);
        }
    }
    bool CheckVerticalMathes()
    {
        bool isMathes = false;
        for (int columnIndex = 0; columnIndex < columNum; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowNum - 2; rowIndex++)
            {
                if ((GetGemstone(rowIndex, columnIndex).gemstoneType == GetGemstone(rowIndex+1, columnIndex ).gemstoneType) &&
                    (GetGemstone(rowIndex, columnIndex).gemstoneType == GetGemstone(rowIndex+2, columnIndex ).gemstoneType))
                {
                    Debug.Log("发现相同宝石");
                    AddMathes(GetGemstone(rowIndex, columnIndex));
                    AddMathes(GetGemstone(rowIndex + 1, columnIndex));
                    AddMathes(GetGemstone(rowIndex + 2, columnIndex));
                    isMathes = true;
                }

            }
        }
        return isMathes;
    }

    bool CheckHorizontalMathes()
    {
        bool isMathes = false;
        for(int rowIndex =0;rowIndex < rowNum;rowIndex ++)
        {
            for (int columnIndex = 0; columnIndex < columNum-2;columnIndex++)
            {
                if((GetGemstone(rowIndex, columnIndex).gemstoneType == GetGemstone(rowIndex, columnIndex + 1).gemstoneType) &&
                    (GetGemstone(rowIndex, columnIndex).gemstoneType == GetGemstone(rowIndex, columnIndex + 2).gemstoneType))
                {
                    Debug.Log("发现相同宝石");
                    AddMathes(GetGemstone(rowIndex, columnIndex));
                    AddMathes(GetGemstone(rowIndex, columnIndex + 1));
                    AddMathes(GetGemstone(rowIndex, columnIndex + 2));

                    isMathes = true;
                }

            }
        }
        return isMathes;
    }

    void AddMathes( GemStone c)
    {
        if (mathesGamestone == null)
            mathesGamestone = new ArrayList();
        int index = mathesGamestone.IndexOf(c);
        if(index == -1)
        {
            mathesGamestone.Add(c);
        }
    }
    void RemoveMathes()
    {
        for(int i = 0;i<mathesGamestone.Count;i++)
        {
            GemStone c = mathesGamestone[i] as GemStone;
            RemoveGemstone(c);
        }
        mathesGamestone = new ArrayList();
    }

    void RemoveGemstone(GemStone c)
    {
        // 删除
        Debug.Log("删除宝石");
        c.Dispose();
        
        for(int i =c.rowIndex+1;i<rowNum;i++)
        {
            GemStone temGemstone = GetGemstone(i, c.columnIndex);
            temGemstone.rowIndex--;
            SetGemstone(temGemstone.rowIndex, temGemstone.columnIndex, temGemstone);
            //temGemstone.UpdatePosition(temGemstone.rowIndex, temGemstone.columnIndex);
            temGemstone.TewwnToPosition(temGemstone.rowIndex, temGemstone.columnIndex);
        }

        GemStone newGemstone = AddGemstone(rowNum, c.columnIndex);
        newGemstone.rowIndex--;
        SetGemstone(newGemstone.rowIndex, newGemstone.columnIndex,newGemstone);
        //newGemstone.UpdatePosition(newGemstone.rowIndex, newGemstone.columnIndex);
        newGemstone.TewwnToPosition(newGemstone.rowIndex, newGemstone.columnIndex);


    }


    public GemStone GetGemstone(int rowIndex,int columnIndex)
    {
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        GemStone c =  temp[columnIndex] as GemStone;
        return c;
    }

    public void SetGemstone(int rowIndex, int columnIndex,GemStone c)
    {
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        temp[columnIndex] = c;
    }

    public void Exchange(GemStone c1,GemStone c2)
    {
        SetGemstone(c1.rowIndex, c1.columnIndex, c2);
        SetGemstone(c2.rowIndex, c2.columnIndex, c1);
        //交换c1,c2行号
        int tempRowIndex;
        tempRowIndex = c1.rowIndex;
        c1.rowIndex = c2.rowIndex;
        c2.rowIndex = tempRowIndex;

        int tempColIndex;
        tempColIndex = c1.columnIndex ;
        c1.columnIndex = c2.columnIndex;
        c2.columnIndex = tempColIndex;

//         c1.UpdatePosition(c1.rowIndex, c1.columnIndex);
//         c2.UpdatePosition(c2.rowIndex, c2.columnIndex);
        c1.TewwnToPosition(c1.rowIndex, c1.columnIndex);
        c2.TewwnToPosition(c2.rowIndex, c2.columnIndex);
    }
}
