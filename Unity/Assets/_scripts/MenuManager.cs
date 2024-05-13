using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ip, port;

    private void OnEnable()
    {
        NetworkManager.OnConnect += SwitchScene;
    }

    private void Start()
    {
        ip.text = "192.168.50.2";
        port.text = "3000";
    }

    private void SwitchScene()
    {
        SceneManager.LoadSceneAsync("Dungeon");
    }

    public void Join()
    {
        NetworkManager.Instance.Connect(ip.text, int.Parse(port.text), "Deno");
    }
}
