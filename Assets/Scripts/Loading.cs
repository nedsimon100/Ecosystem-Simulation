using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Loading : MonoBehaviour
{
    public Text percentageDone;
    void Awake()
    {
        
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (operation.isDone == false)
        {
            percentageDone.text = (((((operation.progress)/0.9)*100).ToString("0"))+("%"));
            yield return null;
        }
    }
}
