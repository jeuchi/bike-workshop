using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public int questionNumber = 1;
    public Text questionCanvas;
    public Text optionsCanvas;

    private bool quizFinished = false;
    private int correctAnswers = 0;
    private int correctAnswer = 0;
    private int questionAttempts = 0;

    public void OnButton(int buttonNumber)
    {
        if (quizFinished)
        {
            return;
        }

        if (buttonNumber == correctAnswer)
        {
            if (questionAttempts == 0)
            {
                correctAnswers++;
            }
            questionNumber++;
            NextQuestion();
        }
        else
        {
            questionAttempts++;
        }
    }

    void OnEnable()
    {
        NextQuestion();
    }

    public void NextQuestion()
    {
        questionAttempts = 0;

        if (questionCanvas == null || optionsCanvas == null)
        {
            return;
        }

        switch(questionNumber) {
            case 1:
                questionCanvas.text = "Quiz Time!\n\nWhen using a tire lever, what is the correct way to remove the outer tire?";
                optionsCanvas.text = "A. Insert one end of the tire lever between the tire and the rim, pry the tire open, and move the lever along the rim to gradually loosen the tire"
                            + "\nB. Use the tire lever to forcefully pry the tire off in one motion"
                            + "\nC. Use two tire levers to pry open both sides of the tire simultaneously"
                            + "\nD. Do not use a tire lever; remove the tire by hand";
                correctAnswer = 0;
                break;
            case 2:
                questionCanvas.text = "When removing the inner tube, where should you start?";
                optionsCanvas.text = "A. Near the valve stem"
                            + "\nB. Away from the valve stem";
                correctAnswer = 1;
                break;
            case 3:
                questionCanvas.text = "In which situation is a tire more likely to burst?";
                optionsCanvas.text = "A. High pressure"
                            + "\nB. Low pressure"
                            + "\nC. Correct pressure";
                correctAnswer = 0;
                break;
            case 4:
                questionCanvas.text = "When reinstalling the outer tire, how should you inflate the tube to avoid pinching or puncturing it?";
                optionsCanvas.text = "A. Inflate it fully in one go"
                            + "\nB. Inflate it gradually in stages and check the tire position along the rim"
                            + "\nC. Inflate it without checking for alignment"
                            + "\nD. Use a manual pump to inflate it quickly";
                correctAnswer = 1;
                break;
            default:
                quizFinished = true;
                questionCanvas.text = "You have finished the quiz!";
                optionsCanvas.text = "Correct answers: " + correctAnswers + " / " + (questionNumber-1);
                break;
        }
    }
}
