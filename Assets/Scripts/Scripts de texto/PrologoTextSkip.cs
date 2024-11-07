using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PrologoTextSkip : MonoBehaviour
{
    private Animator animator;
    public string nextSceneName;
    private bool sceneChangeScheduled = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !sceneChangeScheduled)
        {
            animator.Play("PrologoAnimator", 0, 1f);

            Invoke("LoadNextScene", 2f);
            sceneChangeScheduled = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !sceneChangeScheduled)
        {
            Invoke("LoadNextScene", 2f);
            sceneChangeScheduled = true;
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
