using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ip, port, nickname;

    private void OnEnable()
    {
        NetworkManager.OnConnect += SwitchScene;
    }

    private void Start()
    {
        ip.text = "192.168.50.3";
        port.text = "3000";
        nickname.text = "Dino";
    }

    private void SwitchScene()
    {
        SceneManager.LoadSceneAsync("Dungeon");
    }

    public void Join()
    {
        NetworkManager.Instance.Connect(ip.text, int.Parse(port.text), nickname.text);
    }
}
