using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CsvToPngConvert : MonoBehaviour
{
    static string txtFolderName;
    static string imgFolderName;
    static string savePath;
    public Material textureMaterial;

    Color colorValue = Color.black;

    string[] strReadText;
    string[] strSplitText;
    float[,] cvtText;
    public Texture2D texture;
    public Image img;

    System.IO.FileInfo[] fileText;
    System.IO.DirectoryInfo dirText;
    System.IO.FileInfo[] fileImage;
    System.IO.DirectoryInfo dirImage;
    
    int num = 0;

    //180623 yy

    public void Start()
    {
        txtFolderName = Application.dataPath + "/Resources/Text/";
        imgFolderName = Application.dataPath + "/Resources/Image/";
        savePath = Application.dataPath + "/Resources/Image/";
    }
    public void ConvertBtn()
    {
        StartCoroutine(SearchFileList());
    }

    IEnumerator SearchFileList()
    {
        dirText = new System.IO.DirectoryInfo(txtFolderName);
        fileText = dirText.GetFiles("*.csv");

        if (fileText.Length == 0)
        {
            Debug.Log("Csv File Empty");
        }
        else
        {
            string str = "";

            for (int i = 0; i < fileText.Length; i++) { 
                str += fileText[i].Name.ToString() + Environment.NewLine;
            }

            txt.text = "txtFolderName" + txtFolderName + "\n textlist : " + fileText.Length;
        }

        dirImage = new System.IO.DirectoryInfo(imgFolderName);
        fileImage = dirImage.GetFiles("*.png");

        if (fileImage.Length == 0)
        {
            Debug.Log("Png File Empty");
        }
        else
        {
            string s = "";
            for (int i = 0; i < fileImage.Length; i++)
                s += fileImage[i].Name.ToString() + Environment.NewLine;
        }

        for (num = 0; num < fileImage.Length; num++)
        {
            if (fileText[num].Name.Substring(0, 12) == fileImage[num].Name.Substring(0, 12))
            {
                Debug.Log("Already Created");
            }
            else
            {
                Debug.Log("");
                break;
            } 
        }

        Debug.Log(num + "/" + fileImage.Length);

        yield return new WaitForSeconds(3f);

        StartCoroutine(convert_png());              
    }

    IEnumerator ConvertPng()
    {
        if (num == fileText.Length)
        {
            num--;
        }

        strReadText = File.ReadAllLines(fileText[num].ToString());

        int arrSize = Convert.ToInt32(File.ReadAllLines(fileText[0].ToString()).Length);

        cvtText = new float[arrSize, arrSize];
        texture = new Texture2D(arrSize, arrSize);

        if (num < Convert.ToInt32(fileText.Length))
        {
            for (int i = 0; i < arrSize; i++)
            {
                strSplitText = strReadText[i].Split(',');
                
                for (int j = 0; j < arrSize; j++)
                {
                    cvtText[i, j] = Convert.ToSingle(strSplitText[j]);
                    TextToColor(i, j);
                    texture.SetPixel(i, j, colorValue);
                }
            }

            byte[] textureByte = texture.EncodeToPNG();
            string saveFileName = fileText[num].Name.Substring(0, 12);
            File.WriteAllBytes(savePath + saveFileName + ".png", textureByte);
        }

        else
        {
            Debug.Log("Text To Png Complete");
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(SearchFileList());
    }

    public void ButtonEvt()
    {

        texture = Resources.Load<Texture2D>("Image/" + Path.GetFileNameWithoutExtension(fileImage[fileImage.Length - 1].Name));
        textureMaterial.mainTexture = texture;
        img.material = null;
        img.material = textureMaterial;
    }

    public void TextToColor(int i, int j)
    {
        float txt = Convert.ToSingle(cvtText[i, j]);
        if (cvtText[i, j].Equals(-1000f))
        {
            colorValue = new Color32(0, 0, 0, 0);
        }
        else
        {
            //변환 후 표출되야 하는 이미지의 색상 범례값
            //예제코드이기 때문에 아래는 임의 값 사용 
            if (txt > 100.0f) { colorValue = new Color32(1, 0, 1, 255); }
            else if (txt > 90f && txt <= 100f) { colorValue = new Color32(7, 40, 6, 255); }
            else if (txt > 80f && txt <= 90f) { colorValue = new Color32(8, 1, 5, 255); }
            else if (txt > 70f && txt <= 80f) { colorValue = new Color32(9, 65, 4, 255); }
            else if (txt > 60f && txt <= 70f) { colorValue = new Color32(10, 233, 3, 255); }
            else if (txt > 50f && txt <= 60f) { colorValue = new Color32(11, 7, 2, 255); }
            else if (txt > 40f && txt <= 50f) { colorValue = new Color32(12, 56, 1, 255); }
            else if (txt > 30f && txt <= 40f) { colorValue = new Color32(13, 86, 0, 255); }
            else if (txt > 0f && txt <= 30f) { colorValue = new Color32(14, 33, 0, 0); }
            else if (txt <= 0f) { colorValue = new Color32(255, 255, 255, 0); }
        }
    }
}
