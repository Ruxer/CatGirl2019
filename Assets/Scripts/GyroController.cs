using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GyroController : MonoBehaviour {

    public bool enableGyro;
    public RectTransform targetRect;
    [Header("移动范围")]
    [Range(0f,1f)]
    [Tooltip("X方向移动范围，移动物体宽度的百分比")]
    public float xMoveRange=0.3f;
    [Range(0f, 1f)]
    [Tooltip("Y方向移动范围,移动物体高度的百分比")]
    public float yMoveRange=0.3f;
    [Header("移动速度")]
    [Range(0.5f,2.5f)]
    public float moveSpeed = 1.5f;

    //public Text debugText;
    //public Transform refObj;

    private float xFixedAngleRange=0.07f;//最大与最小角度范围内的百分比
    private float yFixiedAngleRange = 0.07f;
    private float yPosMaxAngle = 40f;
    private float yNegMinAngle = 320f;
    private float xPosMaxAngle = 40f;
    private float xNegMinAngle = 320f;
    private float deltaXAngle;
    private float deltaYAngle;
    private float targetWidth;
    private float targetHeight;
    private float deltaWidth;
    private float deltaHeight;
    private Vector2 originAnchoredPos;
    private float xRange;
    private float yRange;
    private Vector2 targetAnchoredPos;
    

	void Awake () {
        Input.gyro.enabled = enableGyro;
        deltaYAngle = (yPosMaxAngle + (360f - yNegMinAngle))*yFixiedAngleRange / 2f;
        deltaXAngle = (xPosMaxAngle + (360f - xNegMinAngle))*xFixedAngleRange / 2f;
        targetWidth = targetRect.sizeDelta.x;
        targetHeight = targetRect.sizeDelta.y;
        deltaWidth = targetWidth * xMoveRange / 2f;
        deltaHeight = targetHeight * yMoveRange / 2f;
        originAnchoredPos = targetRect.anchoredPosition;
        //Debug.Log(deltaXAngle+":"+deltaYAngle+":"+deltaWidth+":"+deltaHeight);
	}
	
	void Update () {
        if (enableGyro)
        {
            xRange = GetRange().x;
            yRange = GetRange().y;
            targetAnchoredPos = new Vector3(originAnchoredPos.x + GetRange().x * deltaWidth,
                originAnchoredPos.y + GetRange().y * deltaHeight);
            targetRect.anchoredPosition = Vector2.Lerp(targetRect.anchoredPosition,targetAnchoredPos,moveSpeed*Time.deltaTime);
            /*targetRect.anchoredPosition = new Vector3(originAnchoredPos.x + GetRange().x * deltaWidth,
                originAnchoredPos.y + GetRange().y * deltaHeight);
            debugText.text = GetRange().ToString();*/
            //debugText.text = GyroToUnity(Input.gyro.attitude).eulerAngles.ToString();
        }
        
	}
    //x角度为90时 range.x=-1 ?? 万向锁？
    private Vector2 GetRange()
    {
        Vector2 range = new Vector2(xRange,yRange);
        Vector3 angles = GyroToUnity(Input.gyro.attitude).eulerAngles;//GyroToUnity(Input.gyro.attitude).eulerAngles;  //refObj.rotation.eulerAngles;
        //Debug.Log(refObj.rotation.eulerAngles);
        if (angles.y >= 0f && angles.y <=deltaYAngle)
        {
            range.x = 0;
        }else if (angles.y <= 360f && angles.y >= (360f-deltaYAngle))
        {
            range.x = 0;
        }else if (angles.y > deltaYAngle && angles.y < yPosMaxAngle)
        {
            range.x = -(angles.y -deltaYAngle)/ (yPosMaxAngle - deltaYAngle);
        }
        else if (angles.y > yNegMinAngle && angles.y < (360f - deltaYAngle))
        {
            range.x =1-(angles.y-yNegMinAngle-deltaYAngle)/(360f-yNegMinAngle-deltaYAngle);
            //Debug.Log(angles.y-yNegMinAngle-deltaYAngle);
            //Debug.Log(360f-yNegMinAngle-deltaYAngle);
        }/*else if (angles.y > yPosMaxAngle&&angles.y<180f)
        {
            range.x = 1;
        }else if (angles.y >= 180f && angles.y <= yNegMinAngle)
        {
            range.x = -1;
        }*/


        if (angles.x >= 0f && angles.x <=deltaXAngle)
        {
            range.y = 0;
        }else if (angles.x <= 360f && angles.x >= (360f - deltaXAngle))
        {
            range.y = 0;
        }else if (angles.x > deltaYAngle && angles.x <= xPosMaxAngle)
        {
            range.y = (angles.x - deltaXAngle) / (xPosMaxAngle - deltaXAngle);
        }else if (angles.x > xNegMinAngle && angles.x < (360f-deltaYAngle))
        {
            range.y = (angles.x - xNegMinAngle - deltaXAngle) / (360f - xNegMinAngle - deltaXAngle)-1;
        }
        /*else if (angles.x > xPosMaxAngle && angles.x < 180f)
        {
            range.y = 1;
        }
        else if (angles.x >= 180f && angles.x <= xNegMinAngle)
        {
            range.y = -1;
        }*/
        return range;
    }
    
    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x,q.y,-q.z,-q.w);
    }

}
