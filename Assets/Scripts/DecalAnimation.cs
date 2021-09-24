using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalAnimation : MonoBehaviour
{
    [SerializeField] private Renderer cubeExample;
    [SerializeField] private Transform textureToApply;
    [SerializeField] private Transform topOfCube;
    [SerializeField] private Transform bottomOfCube;

    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material decalMaterial;

    int stage = 0;
    bool animating = false;

    private void Awake()
    {
        textureToApply.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !animating)
        {
            switch (stage)
            {
                case 0:
                    textureToApply.gameObject.SetActive(true);
                    stage++;
                    break;
                case 1:
                    StartCoroutine(MoveToTopOfCube());
                    break;
                case 2:
                    StartCoroutine(MoveToBottomOfCube());
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator MoveToTopOfCube()
    {
        animating = true;

        var startColor = cubeExample.material.GetColor("_BaseColor");
        var startPos = textureToApply.position;
        var startRot = textureToApply.rotation;
        var endColor = startColor;
        endColor.a = 0.0f;
        var endPos = topOfCube.position;
        var endRot = topOfCube.rotation;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            cubeExample.material.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));
            textureToApply.position = Vector3.Lerp(startPos, endPos, t);
            textureToApply.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        textureToApply.position = endPos;
        textureToApply.rotation = endRot;

        stage++;
        animating = false;
    }

    private IEnumerator MoveToBottomOfCube()
    {
        animating = true;

        var startPos = topOfCube.position;
        var endPos = bottomOfCube.position;

        var placed = false;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            if(!placed && t > 0.75f)
            {
                cubeExample.material = decalMaterial;
                placed = true;
            }

            textureToApply.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        textureToApply.position = endPos;

        stage++;
        animating = false;
    }
}
