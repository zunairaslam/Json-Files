using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReadFiles : MonoBehaviour
{
    //    //[Serializable]
    //    //public struct CountryList
    //    //{
    //    //    public string Name;
    //    //    public int Idd;
    //    //    public string region;
    //    //    public Sprite Icon;

    //    }
    //[SerializeField]
    //CountryList[] countryLists;

    //  public List<Root> data = new List<Root>()   ;
    GameObject buttonTemp;
    CountryList data;
    [SerializeField] Sprite defaultIcon;


    [ContextMenu("Test Get")]
    public async void TestGet()
    {
        var url = "https://restcountries.com/v3.1/all";
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("ContantType", "application/json");
        var operation = www.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        var jsonResponse = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed: {www.error}");
        }

        //try
        //{
        Debug.Log($"success: {www.downloadHandler.text}");
        //var data = JsonConvert.DeserializeObject<JsonDataClass[]>(jsonResponse);
        data = JsonUtility.FromJson<CountryList>("{\"countries\":" + jsonResponse + "}");
        TestGetImage();
        //DrawUI();

        foreach (var item in data.countries)
        {

            Debug.Log("Name : " + item.name.common +
                " Idd : " + item.idd.root +
                " Region : " + item.region +
                " Flags Url : " + item.flags.png
                  );

        }
    }
    void DrawUI(int i)
    {
        buttonTemp = transform.GetChild(0).gameObject;
        GameObject g;

        //int N = data.countries.Length;
        //for (int i = 0; i < N; i++)
        {
            g = Instantiate(buttonTemp, transform);
            g.transform.SetAsFirstSibling();
            g.transform.GetChild(0).GetComponent<Image>().sprite = data.countries[i].icon;
            g.transform.GetChild(1).GetComponent<TMP_Text>().text = ("Name: " + data.countries[i].name.common);
            g.transform.GetChild(2).GetComponent<TMP_Text>().text = ("Idd: " + data.countries[i].idd.root.ToString());
            g.transform.GetChild(3).GetComponent<TMP_Text>().text = ("Region: " + data.countries[i].region);

            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
        
    }
    void ItemClicked(int itemIndex)
    {
        Debug.Log("name " + data.countries[itemIndex].name);
    }

    public async void TestGetImage()
    {
        Debug.Log(data);
        Debug.Log(data.countries);
        for (int i = 0; i < data.countries.Length; i++)
        {
            var url = data.countries[i].flags.png;
            var w = UnityWebRequestTexture.GetTexture(url);
            w.SetRequestHeader("ContantType", "application/json");
            var operation = w.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();
            var jsonImage = ((DownloadHandlerTexture)w.downloadHandler).texture;
            if (w.error != null)
            {
                data.countries[i].icon = defaultIcon;
            }
            else
            {
                if (w.isDone)

                {
                    Texture2D tx = jsonImage;
                    data.countries[i].icon = Sprite.Create(tx, new Rect(0f, 0f, tx.width, tx.height), Vector2.zero, 10f);
                }
            }
            DrawUI(i);
        }
        Destroy(buttonTemp);
        //var url = "https://flagcdn.com/w320/is.png";
        //using var www = UnityWebRequestTexture.GetTexture(url);
        //www.SetRequestHeader("ContantType", "application/json");
        //var operation = www.SendWebRequest();
        //while (!operation.isDone)
        //    await Task.Yield();

        //var jsonResponse = ((DownloadHandlerTexture)www.downloadHandler).texture;

        //if (www.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.LogError($"Failed: {www.error}");
        //}

        ////try
        ////{
        //Debug.Log($"success: {www.downloadHandler.text}");
        ////var data = JsonConvert.DeserializeObject<JsonDataClass[]>(jsonResponse);
        //var data = JsonUtility.FromJson<CountryList>("{\"countries\":" + jsonResponse + "}");
        ////DrawUI();

        //foreach (var item in data.countries)
        //{
        //    Debug.Log(item.flags.png);
        //    Debug.Log("Name : " + item.name.common +
        //        " Idd : " + item.idd.root +
        //        " Region : " + item.region +
        //        " Flags Url : " + item.flags.png
        //          );

        //}
    }

}
public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}






