using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour {
    public GameObject bloodhelp;
    private GameObject[] bloodhelps;
    public GameObject bullethelp;
    private GameObject[] bullethelps;
    public int bloodnum = 0;
    public int bulletnum = 0;
     Vector3[] inialPos;
    Vector3[] inialBulletPos;
	// Use this for initialization
	void Start () {
        inialPos = new Vector3[bloodnum];
        inialBulletPos = new Vector3[bulletnum];
        bloodhelp = Resources.Load<GameObject>("m_prefabs/bloodhelp");
        bullethelp = Resources.Load<GameObject>("m_prefabs/Bullet");
        for(int i = 0; i < inialPos.Length; i++)
        {
            inialPos[i] = new Vector3(Random.Range(-12, 13), 0, Random.Range(-17,17));
            GameObject go = Instantiate < GameObject > (bloodhelp);
            go.transform.position = inialPos[i];
        }
        for(int j=0; j < inialBulletPos.Length; j++)
        {
            inialBulletPos[j] = new Vector3(Random.Range(-12, 13), 0, Random.Range(-17, 17));
            GameObject go = Instantiate<GameObject>(bullethelp);
            go.transform.position = inialBulletPos[j];
        }
        InvokeRepeating("generateBlood", 0, 10F);
        InvokeRepeating("generateBullet", 0, 20F);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void generateBlood()
    {
        bloodhelps = GameObject.FindGameObjectsWithTag("blood");
        if (bloodhelps.Length >= bloodnum) return;
        for (int i = 0; i < (bloodnum - bloodhelps.Length); i++)
        {
            Vector3 pos = new Vector3(Random.Range(-12, 13), 0, Random.Range(-17, 17));
            GameObject go = Instantiate<GameObject>(bloodhelp);
            go.transform.position = pos;
        }
    }

    private void generateBullet()
    {
        bullethelps = GameObject.FindGameObjectsWithTag("bullet");
        if (bullethelps.Length >= bulletnum) return;
        for (int i = 0; i < (bulletnum - bullethelps.Length); i++)
        {
            Vector3 pos = new Vector3(Random.Range(-12, 13), 0, Random.Range(-17, 17));
            GameObject go = Instantiate<GameObject>(bullethelp);
            go.transform.position = pos;
        }
    }
}
