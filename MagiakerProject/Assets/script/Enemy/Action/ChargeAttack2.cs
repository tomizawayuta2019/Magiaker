using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeAttack2 : Action

{
    Rigidbody rb;
    Vector3 targetRot;
    [SerializeField]
    GameObject attackArea;
    [SerializeField]
    float RotationSpeed = 10f;//回転速度
    //突進の時間
    [SerializeField]
    float MoveTime = 3;
    //待機時間
    [SerializeField]
    float WaitTime = 2;
    //移動速度
    [SerializeField]
    float speed = 50;
    [SerializeField]
    float Damage = 10;
    [SerializeField]
    GameObject asd;
    AttackArea Range;
    EnemyController EneCon;
    public bool SearchAction = true;
    void SetSearchAction(bool set) { SearchAction = set; }
    bool GetSearchAction() { return SearchAction; }
    float radius = 1;
    Transform target;     // 目標地点
    NavMeshAgent agent;
    [SerializeField]
    LineRenderer line;
    NavMeshPath path;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        EneCon = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody>();
        Range = attackArea.GetComponent<AttackArea>();
        Range.Damage = Damage;
        attackArea.SetActive(false);
    }
    public override void ActionEnter(GameObject target, GameObject self)
    {
        base.ActionEnter(target, self);
        base.ActionEnter(target, self);
        if (FlagReset)
        {
            SetSearchAction(true);
            attackArea.SetActive(false);
            FlagReset = false;
        }
        if (GetSearchAction())
        {
            Debug.Log("Start");
            //targetに対しての正面方向をむく
            Turn(target, self);
        }
        //targetに対しての正面方向をむいたら回転をやめChargeActionのコルーチンを開始する
        if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(targetRot.y)) < 0.5 && GetSearchAction())
        {
            SetSearchAction(false);
            StartCoroutine("ChargeAction", target);
            // NavMeshAgent に目的地を設定する
        }
    }
    void Turn(GameObject target, GameObject self)
    {
        Vector3 targetPos = target.transform.position;
        targetPos.y = self.transform.position.y;
        //targetに対しての正面方向を取得する
        Quaternion TargetRotation = Quaternion.LookRotation(targetPos - self.transform.position);
        //targetに対して正面になるように徐々に回転させる
        self.transform.rotation = Quaternion.Slerp(self.transform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        targetRot = TargetRotation.eulerAngles;
    }
    public IEnumerator ChargeAction(GameObject target)
    {
        Vector3 TargetPos = target.transform.position;
        TargetPos.y = transform.position.y;
        Range.CharacterOnTouch = false;
        attackArea.SetActive(true);
        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            agent.SetDestination(TargetPos);
            // 経路取得用のインスタンス作成
            path = new NavMeshPath();
            // 明示的な経路計算実行
            agent.CalculatePath(TargetPos, path);
            // LineRendererで経路描画！
            line.SetVertexCount(path.corners.Length);
            line.SetPositions(path.corners);
            asd.transform.position = path.corners[path.corners.Length - 1];
            asd.transform.SetParent(null);

        }
        agent.acceleration = speed / 5;
        agent.speed = speed;
        agent.Resume();       
        float AttackTime = 0;
        for (;;)
        {
            asd.transform.position = path.corners[path.corners.Length - 1]+(transform.forward*5);
            line.SetVertexCount(path.corners.Length);
            line.SetPositions(path.corners);
            AttackTime += Time.deltaTime;
            //Playerのタグを持つものに当たるとWaitTimeの間、待機してfor文を抜ける
            if (Range.CharacterOnTouch)
            {
                Debug.Log(1132);
                agent.autoBraking = true;
                agent.SetDestination(transform.position);
                yield return null;
                break;
            }
            if (MoveTime <= AttackTime)
            {
                agent.Stop();
                yield return null;
                break;
            }
            yield return null;
        }
        agent.SetDestination(transform.position);
        attackArea.SetActive(false);
        agent.speed = 0f;
        agent.Stop();
        agent.autoBraking = false;
        Range.CharacterOnTouch = false;
        if (Range.TouchChar == Tags.Player)
        {
            yield return new WaitForSeconds(WaitTime);
        }
        yield return new WaitForSeconds(WaitTime / 3);
        agent.angularSpeed = 180;
        SetSearchAction(true);
        attackArea.SetActive(false);
    }
}
