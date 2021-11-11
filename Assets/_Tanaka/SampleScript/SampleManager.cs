using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{   
    // Manager��Instance�����A��������obj�ɓn���ł͂Ȃ��Aobj��Start��Instance���n���悤�ɕύX(objBase)

    // ��������
    [Header("��������")]
    public int limitTime;
    private float time;

    // �X�R�A
    [Header("�X�R�A")]
    public int scoreCount;

    [Header("�J�n��bool")]

    public bool isStart;
    public bool isEnd;


    [Header("�J�nUI��Object")]
    public GameObject startUIParent;

    [Header("�I��UI��Object")]
    public GameObject endUIParent;
    // 
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        isStart = false;
        scoreCount = 0;

        // �I��UI�̔�\��
        endUIParent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
            if(time > limitTime)
            {
                time = limitTime;
                isStart = false;

                // �I��UI�̕\��
                endUIParent.SetActive(true);
            }
        }
    }


    public void ClickStart()
    {   
        // StartUI��Obj��OFF
        startUIParent.SetActive(false);
    }

    public void ClickEnd()
    {
        
    }

    public void ClickRestart()
    {   
        // �V�[���̍ēǂݍ���
        SceneManager.LoadScene("");
    }
}
