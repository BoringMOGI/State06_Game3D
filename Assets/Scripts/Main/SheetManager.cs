using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetManager : MonoBehaviour
{
    [System.Serializable] public struct GoogleData
    {
        public string type;
        public bool result;
        public string msg;
        public string requestData;
    }
    [System.Serializable] struct Wrapper<T>
    {
        public T[] datas;
    }

    const string URL = "https://script.google.com/macros/s/AKfycbz9dLYooZF2BQSV121vdOwIQl-EbzVD5AumzRUAjGCkTLzJ4VEd4Tw5A9BQWHebDaL8Lg/exec";

    [SerializeField] GoogleData downloadData;

    public void GetDatas<T>(string sheetType, System.Action<T[]> callback)
    {
        // 요청자의 요청에 맞게 폼을 만든다.
        WWWForm form = new WWWForm();
        form.AddField("order", "getData");          // 웹에게 보내는 명령어.
        form.AddField("type", sheetType);           // 데이터 형식(=시트 이름).

        StartCoroutine(Post(form, callback));
    }
    public void GetGoogleData(WWWForm form, System.Action<GoogleData> callback)
    {
        StartCoroutine(PostWeb(form, callback));
    }

    private IEnumerator PostWeb(WWWForm form, System.Action<GoogleData> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            // 웹 요청이 성공적으로 끝이 났다면.
            if (www.isDone)
            {
                string data = www.downloadHandler.text;                     // JSON형태의 데이터.
                downloadData = JsonUtility.FromJson<GoogleData>(data);      // GoogleData 자료형 객체로 Parse.
                Debug.Log(downloadData.msg);                                // 웹에서 보낸 메시지 출력.
                callback?.Invoke(downloadData);
            }
            else
            {
                Debug.Log($"Failed Web Request : {www.error}");
            }
        }
    }
    private IEnumerator Post<T>(WWWForm form, System.Action<T[]> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            // 웹 요청이 성공적으로 끝이 났다면.
            if (www.isDone)
            {
                string data = www.downloadHandler.text;                     // JSON형태의 데이터.
                downloadData = JsonUtility.FromJson<GoogleData>(data);      // GoogleData 자료형 객체로 Parse.
                Debug.Log(downloadData.msg);                                // 웹에서 보낸 메시지 출력.

                // 다운로드 데이터의 requestData를 Wrapper형태로 파싱.
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(downloadData.requestData);

                // 요청자에게 데이터 전달.
                callback?.Invoke(wrapper.datas);
            }
            else
            {
                Debug.Log($"Failed Web Request : {www.error}");
            }
        }
            

       
    }

    

}
