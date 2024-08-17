using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public GameState gameState;

    public static DialogManager instance {  get; private set; }

    private void Awake()
    {
        instance = this;
    }

    int currentLine = 0;

    Dialog dialog;

    bool isTyping;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        this.dialog = dialog;

        dialogBox.SetActive(true);

        StopAllCoroutines();


        StartCoroutine(TypeDialog(dialog.Lines[0]));
        StopCoroutine(TypeDialog(dialog.Lines[0]));
        HandleUpdate();
       
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
          
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;

                OnHideDialog?.Invoke();
            }
        }
    }





    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;

        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            if (letter == '\n')
            {
                // Add a line break
                dialogText.text += "\n";
            }
            else
            {
                // Add the letter
                dialogText.text += letter;
            }

            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }


}
