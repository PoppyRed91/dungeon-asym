using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;
    private RawImage _fader;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _fader = GetComponentInChildren<RawImage>();
    }
    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            _fader.DOFade(0, 2);
        }
        else _fader.DOFade(1, 2);

    }
}
