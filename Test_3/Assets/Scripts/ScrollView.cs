using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private List<Users> userDataBase = new List<Users>();

    [SerializeField] private InputField userName;
    [SerializeField] private Text usersInfo;
    

    private void Start()
    {
        ReadJSON();
        InstantiatePrefab();
    }


    void InstantiatePrefab()
    {
        foreach (var user in userDataBase)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;         
            instance.transform.SetParent(content, false);
            instance.transform.name = user.name;
            //instance.SetActive(true);
            InitializeUserView(instance, user);
        }
    }

    void InitializeUserView(GameObject viewGameObject, Users user)
    {
        TestUserView view = new TestUserView(viewGameObject.transform);
        view.titleText.text = user.name;
    }

    public class TestUserView
    {
        public Text titleText;
       
        public TestUserView(Transform rootView)
        {
            titleText = rootView.Find("UserName").GetComponent<Text>();          
        }
    }

    public void UpdateUsers()
    {


        if (userName.text == "")
        {
            usersInfo.text = "Empty field!";

            for (int i = 0; i < content.childCount; i++) content.GetChild(i).gameObject.SetActive(true);
        } 


                
        else 
        {
            usersInfo.text = "";
            

            for (int i = 0; i < userDataBase.Count; i++)
            {
                if (userDataBase[i].name.Contains(userName.text))
                {              
                    content.GetChild(i).gameObject.SetActive(true);

                    usersInfo.text += "Name: " + userDataBase[i].name + "\r\nAge: " + userDataBase[i].age +
                        "\r\nRelation: " + userDataBase[i].relation + "\r\n\r\n";
                }

                else
                {
                    content.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (usersInfo.text == "") usersInfo.text = "User is not found!";
           
        }
    }

    public void ReadJSON()
    {
        string path = Application.streamingAssetsPath + "/Users.dat";
        string JSONString = File.ReadAllText(path);
        Users[] user = JsonHelper.FromJson<Users>(JSONString);
        for (int i = 0; i < user.Length; i++)
        {
            userDataBase.Add(new Users() { userId = i, name = user[i].name, age = user[i].age, relation = user[i].relation });
        }
    }

    [System.Serializable]
    public class Users
    {
        public string name;
        public int age;
        public int relation;
        public int userId;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.users;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.users = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.users = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] users;
        }
    }
}
