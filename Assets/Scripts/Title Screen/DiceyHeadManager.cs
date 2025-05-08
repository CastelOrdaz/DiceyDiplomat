using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceyHeadManager : MonoBehaviour
{
    [SerializeField] private GameObject headPrefab;

    [SerializeField] private int rows, cols;
    [SerializeField] private Vector2 rowColOffset;
    [SerializeField] private float colStagger, originOffset;
    [SerializeField] private Vector3[] faceRotations;

    private GameObject[,] heads;
    private float angleOffset;
    [SerializeField] private float angle = 90;

    private void MakeArray()
    {
        heads = new GameObject[cols, rows];

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                heads[x,y] = Instantiate(headPrefab, transform);
            }
        }
    }

    private void OffsetHeads()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                float stagger = colStagger * (x % 2);

                float yOffset = (rowColOffset.y * y) + stagger + originOffset;
                float xOffset = (rowColOffset.x * x) + originOffset;

                Vector3 rotation = faceRotations[x % 6];
                Debug.Log(rotation);

                //x = left face
                //y = right face
                //z = top face

                heads[x,y].transform.position = new Vector3(xOffset, 1, yOffset) * angleOffset;
                heads[x,y].transform.Rotate(rotation * angle, Space.Self);
            }
        }
    }

    public void Start()
    {
        angleOffset = Mathf.Sin(Mathf.Deg2Rad * 45);

        heads = new GameObject[rows,cols];

        MakeArray();
        OffsetHeads();
    }

    public void Update()
    {
        //OffsetHeads();
    }
}
