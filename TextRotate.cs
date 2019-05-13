﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotate : MonoBehaviour
{
    public Transform textMeshTransform;
    void Update()
    {
        textMeshTransform.rotation = Quaternion.LookRotation(textMeshTransform.position - Camera.main.transform.position);
    }
}
