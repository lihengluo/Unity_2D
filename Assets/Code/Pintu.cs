using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pintu : MonoBehaviour
{
    // 记录有没有拼图被选中
    private GameObject selectedObject;

    // 拼图预制体
    GameObject[] dragObj;
    // 记录拼图应该放的点
    GameObject[] dropObj;
    // Start is called before the first frame update
    void Start()
    {
        dragObj = GameObject.FindGameObjectsWithTag("drag");
        dropObj = GameObject.FindGameObjectsWithTag("drop");

        foreach (var item in dragObj)
        {
            item.transform.position = new Vector3(Random.Range(-5f, -1f), 0, Random.Range(-3.1f, 2.1f));

        }
    }

    // Update is called once per frame
    void Update()
    {
        // 如果按下了鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            // 选中的物体为空
            if (selectedObject == null)
            {
                // 存储的射线信息
                RaycastHit hit = CastRay();
                // 碰到的物体有碰撞器
                if (hit.collider != null)
                {
                    // 如果标签不是drag直接返回
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }
                    // 碰到的物体标签是--drag,为selectedObject赋值
                    selectedObject = hit.collider.gameObject;
                    // 设置鼠标光标不可见
                    Cursor.visible = false;
                }
            }

            // 选中物体之后，再按一下鼠标左键---放下物体
            else
            {
                // 记录鼠标点击的点
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

                #region  吸附
                //遍历每个放置点，找到和鼠标点击最近的点
                Vector3 tmpdrop = Vector3.zero;
                float minDistance = 10.0f;
                foreach (var item in dropObj)
                {
                    if (Vector3.Distance(item.transform.position, worldPosition) <= minDistance)
                    {
                        minDistance = Vector3.Distance(item.transform.position, worldPosition);
                        tmpdrop = item.transform.position;
                    }
                }
                // 如果最小距离小于---限定值，说明在拼图位置上，就赋值，否则就是鼠标位置
                if (minDistance < 0.4f)
                {
                    // 赋值
                    selectedObject.transform.position = tmpdrop + new Vector3(0, 0.05f, 0);

                }
                else
                {
                    selectedObject.transform.position = worldPosition;
                }
                #endregion
                //selectedObject.transform.position = worldPosition;
                selectedObject = null;
                Cursor.visible = true;
            }
        }
        


        // 如果selectedObject不为空，说明点击到了物体，物体跟随鼠标移动
        if (selectedObject != null)
        {
            // position存储的是鼠标的x,y坐标，自身的z坐标---屏幕坐标
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            // 将屏幕坐标，转换为世界坐标
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            // 为选中的物体赋值--这里的y是固定的0.25
            selectedObject.transform.position = new Vector3(worldPosition.x, 0.05f, worldPosition.z);

            // 按下鼠标的右键，旋转
            if (Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }
    // 返回射线碰撞信息
    private RaycastHit CastRay()
    {
        // 射线最远的点
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        // 射线最近的点
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    // 检查拼图是否拼完
    public void check()
    {
        bool isDone = true;
        // 获取有多少张拼图
        int childCount = dropObj.Length;
        // 遍历每张拼图的位置
        for (int i = 0; i < childCount; i++)
        {
            //获取两张拼图的位置
            Vector3 dropPos = dropObj[i].transform.position;
            Vector3 dragPos = dragObj[i].transform.position;

            //只要有一张拼图位置不对就没有完成
            if (dragPos != dropPos + new Vector3(0, 0.05f, 0))
            {
                isDone = false;
                break;
            }
        }

        if (isDone)
        {
            Debug.Log("完成");
        }
        else
        {
            Debug.Log("没有完成");
        }
    }
}
