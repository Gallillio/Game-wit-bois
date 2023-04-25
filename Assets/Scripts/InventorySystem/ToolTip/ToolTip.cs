using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private void Update()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        
        //set layout element to true if headerLength or contentLength are > the wrap limit to make tool tip size itself accordingly
        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }
}
