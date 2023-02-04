using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cont : MonoBehaviour
{
    public Text contentText;

    private string content;  // ��Ҫ��ʾ������
    private float typingTime = 0.1f;  // �ӳ�ʱ��
    private int nowLength = 0;  // ��ǰ��ӡ������

    void Start()
    {
        content = contentText.text;  // ��¼
        contentText.text = "";  // �ÿ�
        // InvokeRepeating(methodName, time, repeatRate)
        // ����ʼ time ���ÿ���� repeatRate ����Զ����� methodName ����
        InvokeRepeating("DelayTyping", 0.0f, typingTime);  // ÿ���� repeatTime ���Զ���ӡһ����
    }

    void DelayTyping()
    {
        ++nowLength;
        // Substring(startIndex, length)
        // ��startIndex��ʼ����ȡ length ���ַ�
        contentText.text = content.Substring(0, nowLength);
        if (nowLength >= content.Length)  // ��ӡ���
        {
            CancelInvoke();  // ȡ���Զ�����
        }
    }
}
