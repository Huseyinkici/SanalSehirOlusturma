using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaDik : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject modelPrefab;
    public float xZ = 0;
    public float yZ = 0;
    void Start()
    {
        Bina();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Bina()
    {
        string filePath = "D:\\DosyalarToplu\\UnityProjects\\BitirmeSehirOlusturmaYapayZeka\\Assets\\Scripts\\cikti.txt";
        string line = System.IO.File.ReadAllText(filePath);
        string[] values = line.Split(',');
        for (int i = 0; i < values.Length; i += 4)
        {
            int sayi1 = int.Parse(values[i]);
            int sayi2 = int.Parse(values[i + 1]);
            ModelYerlestirx(sayi1 + sayi2);
            int sayi3 = int.Parse(values[i+2]);
            int sayi4 = int.Parse(values[i+3]);
            ModelYerlestirz(sayi3 + sayi4);
            Debug.Log($"1. Sayý: {sayi1}, 2. Sayý: {sayi2}, 2. Sayý: {sayi3}, 2. Sayý: {sayi4}");

            
        }
    }
    public void ModelYerlestirx(int a)
    {
        float xY = 0;
        for(int i = 0;i < a;i++)
        {
            GameObject model = Instantiate(modelPrefab, new Vector3(60f, xY, xZ), Quaternion.identity);
            model.transform.Rotate(new Vector3(-90f, 0f, 90f));
            xY = xY + 3;
        }
        xZ = xZ + 35;
    }
    public void ModelYerlestirz(int a)
    {
        float yY = 0;
        for (int i = 0; i < a; i++)
        {
            GameObject model = Instantiate(modelPrefab, new Vector3(20f, yY, yZ), Quaternion.identity);
            model.transform.Rotate(new Vector3(-90f, 0f, 90f));
            yY = yY + 3;
        }
        yZ = yZ + 35;
    }

}
