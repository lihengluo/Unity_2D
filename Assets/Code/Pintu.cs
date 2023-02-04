using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pintu : MonoBehaviour
{
    // ��¼��û��ƴͼ��ѡ��
    private GameObject selectedObject;

    // ƴͼԤ����
    GameObject[] dragObj;
    // ��¼ƴͼӦ�÷ŵĵ�
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
        // ���������������
        if (Input.GetMouseButtonDown(0))
        {
            // ѡ�е�����Ϊ��
            if (selectedObject == null)
            {
                // �洢��������Ϣ
                RaycastHit hit = CastRay();
                // ��������������ײ��
                if (hit.collider != null)
                {
                    // �����ǩ����dragֱ�ӷ���
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }
                    // �����������ǩ��--drag,ΪselectedObject��ֵ
                    selectedObject = hit.collider.gameObject;
                    // ��������겻�ɼ�
                    Cursor.visible = false;
                }
            }

            // ѡ������֮���ٰ�һ��������---��������
            else
            {
                // ��¼������ĵ�
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

                #region  ����
                //����ÿ�����õ㣬�ҵ������������ĵ�
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
                // �����С����С��---�޶�ֵ��˵����ƴͼλ���ϣ��͸�ֵ������������λ��
                if (minDistance < 0.4f)
                {
                    // ��ֵ
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
        


        // ���selectedObject��Ϊ�գ�˵������������壬�����������ƶ�
        if (selectedObject != null)
        {
            // position�洢��������x,y���꣬�����z����---��Ļ����
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            // ����Ļ���꣬ת��Ϊ��������
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            // Ϊѡ�е����帳ֵ--�����y�ǹ̶���0.25
            selectedObject.transform.position = new Vector3(worldPosition.x, 0.05f, worldPosition.z);

            // ���������Ҽ�����ת
            if (Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }
    // ����������ײ��Ϣ
    private RaycastHit CastRay()
    {
        // ������Զ�ĵ�
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        // ��������ĵ�
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

    // ���ƴͼ�Ƿ�ƴ��
    public void check()
    {
        bool isDone = true;
        // ��ȡ�ж�����ƴͼ
        int childCount = dropObj.Length;
        // ����ÿ��ƴͼ��λ��
        for (int i = 0; i < childCount; i++)
        {
            //��ȡ����ƴͼ��λ��
            Vector3 dropPos = dropObj[i].transform.position;
            Vector3 dragPos = dragObj[i].transform.position;

            //ֻҪ��һ��ƴͼλ�ò��Ծ�û�����
            if (dragPos != dropPos + new Vector3(0, 0.05f, 0))
            {
                isDone = false;
                break;
            }
        }

        if (isDone)
        {
            Debug.Log("���");
        }
        else
        {
            Debug.Log("û�����");
        }
    }
}
