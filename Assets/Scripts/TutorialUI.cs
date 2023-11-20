using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialManager;

    private List<GameObject> images = new List<GameObject>();
    private int currentActiveImage = 0;

    private void Start()
    {
        foreach(Transform tr in tutorialManager.transform)
        {
            images.Add(tr.gameObject);
            tr.gameObject.SetActive(false);
        }
        currentActiveImage = 0;
        images[0].SetActive(true);
    }

    public void OnRightArrowClicked()
    {
        if (currentActiveImage < images.Count - 1)
        {
            images[currentActiveImage].SetActive(false);
            images[currentActiveImage + 1].SetActive(true);
            currentActiveImage++;
        }
    }

    public void OnLeftArrowClicked()
    {
        if (currentActiveImage >= 1)
        {
            images[currentActiveImage].SetActive(false);
            images[currentActiveImage - 1].SetActive(true);
            currentActiveImage--;
        }
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Title");
    }
}
