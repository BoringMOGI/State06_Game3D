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
        // ��û���� ��û�� �°� ���� �����.
        WWWForm form = new WWWForm();
        form.AddField("order", "getData");          // ������ ������ ��ɾ�.
        form.AddField("type", sheetType);           // ������ ����(=��Ʈ �̸�).

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

            // �� ��û�� ���������� ���� ���ٸ�.
            if (www.isDone)
            {
                string data = www.downloadHandler.text;                     // JSON������ ������.
                downloadData = JsonUtility.FromJson<GoogleData>(data);      // GoogleData �ڷ��� ��ü�� Parse.
                Debug.Log(downloadData.msg);                                // ������ ���� �޽��� ���.
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

            // �� ��û�� ���������� ���� ���ٸ�.
            if (www.isDone)
            {
                string data = www.downloadHandler.text;                     // JSON������ ������.
                downloadData = JsonUtility.FromJson<GoogleData>(data);      // GoogleData �ڷ��� ��ü�� Parse.
                Debug.Log(downloadData.msg);                                // ������ ���� �޽��� ���.

                // �ٿ�ε� �������� requestData�� Wrapper���·� �Ľ�.
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(downloadData.requestData);

                // ��û�ڿ��� ������ ����.
                callback?.Invoke(wrapper.datas);
            }
            else
            {
                Debug.Log($"Failed Web Request : {www.error}");
            }
        }
            

       
    }

    

}
