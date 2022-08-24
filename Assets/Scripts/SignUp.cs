using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SignUp : MonoBehaviour
{
    [SerializeField] SheetManager sheetManager;
    [SerializeField] InputField inputName;
    [SerializeField] InputField inputID;
    [SerializeField] InputField inputPW;
    [SerializeField] InputField inputPwRe;
    [SerializeField] Text errorText;
    [SerializeField] Button signUpButton;
    [SerializeField] Button cancelButton;
    [SerializeField] GameObject loginPanel;


    List<InputField> inputList;
    EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        inputList = new List<InputField>();
        inputList.Add(inputName);
        inputList.Add(inputID);
        inputList.Add(inputPW);
        inputList.Add(inputPwRe);

        // ��й�ȣ ���Է� �ʵ忡 �̺�Ʈ ���.
        inputPwRe.onValueChanged.AddListener((str) => { OnDuplicatePw(); });

        // ȸ������ ��ư �̺�Ʈ�� OnSignUp�� ���.
        signUpButton.onClick.AddListener(OnSignUp);

    }

    private void OnEnable()
    {
        inputName.text = string.Empty;
        inputID.text = string.Empty;
        inputPW.text = string.Empty;
        inputPwRe.text = string.Empty;
        errorText.text = string.Empty;

        inputName.Select();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable current = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
            if (current == null || !(current is InputField))
                return;

            // ���� �������� inputField�� ��, Ȥ�� �Ʒ��� ������Ʈ ����.
            Selectable next = null;

            if (Input.GetKey(KeyCode.LeftShift))
                next = current.navigation.selectOnUp;
            else
                next = current.navigation.selectOnDown;

            next?.Select();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            signUpButton.Select();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cancelButton.Select();
        }
    }

    private bool OnDuplicatePw()
    {
        bool isDuplicate = inputPW.text == inputPwRe.text;

        errorText.text = isDuplicate ? "��й�ȣ�� �����մϴ�" : "��й�ȣ�� �ٸ��ϴ�.";
        return isDuplicate;
    }
    private void OnSignUp()
    {
        if (!CheckLength())
            return;

        if (!OnDuplicatePw())
            return;

        WWWForm form = new WWWForm();
        form.AddField("order", "signUp");
        form.AddField("id", inputID.text);
        form.AddField("pass", inputPW.text);
        form.AddField("name", inputName.text);

        sheetManager.GetGoogleData(form, OnReceivedWeb);
    }
    private void OnReceivedWeb(SheetManager.GoogleData data)
    {
        if(data.result)
        {
            errorText.text = data.msg;
            loginPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            errorText.text = data.msg;
        }
    }



    private bool CheckLength()
    {
        foreach (InputField field in inputList)
        {
            if (!IsValidLength(field))
            {
                errorText.text = "�Է� ������ ���̴� 4~12���ڿ��� �մϴ�.";
                return false;
            }
        }

        return true;
    }
    private bool IsValidLength(InputField field)
    {
        string text = field.text;
        return (text.Length >= 4 && text.Length <= 12);
    }

}
