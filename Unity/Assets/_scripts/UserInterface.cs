using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;
    [SerializeField] private RawImage _fader;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            _fader.DOFade(0, 1);
        }
        else _fader.DOFade(1, 1);

    }
}
