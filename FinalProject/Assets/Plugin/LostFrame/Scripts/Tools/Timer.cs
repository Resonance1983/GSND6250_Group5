using UnityEngine;

public delegate void CompleteEvent();

public delegate void UpdateEvent(float t);

public class Timer : MonoBehaviour
{
    private UpdateEvent updateEvent;
    private CompleteEvent onCompleted;
    private bool isLog = true; //是否打印消息
    private float timeTarget; // 计时时间/
    private float timeStart; // 开始计时时间/
    private float offsetTime; // 计时偏差/
    private bool isTimer; // 是否开始计时/
    private bool isDestory = true; // 计时结束后是否销毁/
    private bool isEnd; // 计时是否结束/
    private bool isIgnoreTimeScale = true; // 是否忽略时间速率
    private bool isRepeate; //是否重复
    private float now; //当前时间 正计时
    private float downNow; //倒计时
    private bool isDownNow = false; //是否是倒计时

    // 是否使用游戏的真实时间 不依赖游戏的时间速度
    // 依赖Time.time来进行计时的
    private float TimeNow => isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;

    /// <summary>
    /// 创建计时器:名字  根据名字可以创建多个计时器对象
    /// </summary>
    public static Timer createTimer(string gobjName = "Timer")
    {
        var g = new GameObject(gobjName);
        var timer = g.AddComponent<Timer>();
        return timer;
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    /// <param name="time_">目标时间</param>
    /// <param name="isDownNow">是否是倒计时</param>
    /// <param name="onCompleted_">完成回调函数</param>
    /// <param name="update">计时器进程回调函数</param>
    /// <param name="isIgnoreTimeScale_">是否忽略时间倍数</param>
    /// <param name="isRepeate_">是否重复</param>
    /// <param name="isDestory_">完成后是否销毁</param>
    public void startTiming(float timeTarget, bool isDownNow = false,
        CompleteEvent onCompleted_ = null, UpdateEvent update = null,
        bool isIgnoreTimeScale = true, bool isRepeate = false, bool isDestory = true,
        float offsetTime = 0, bool isEnd = false, bool isTimer = true)
    {
        this.timeTarget = timeTarget;
        this.isIgnoreTimeScale = isIgnoreTimeScale;
        this.isRepeate = isRepeate;
        this.isDestory = isDestory;
        this.offsetTime = offsetTime;
        this.isEnd = isEnd;
        this.isTimer = isTimer;
        this.isDownNow = isDownNow;
        timeStart = TimeNow;

        if (onCompleted_ != null)
            onCompleted = onCompleted_;
        if (update != null)
            updateEvent = update;
    }

    private void Update()
    {
        if (isTimer)
        {
            now = TimeNow - offsetTime - timeStart;
            downNow = timeTarget - now;
            if (updateEvent != null)
            {
                if (isDownNow)
                    updateEvent(downNow);
                else
                    updateEvent(now);
            }

            if (now > timeTarget)
            {
                if (onCompleted != null)
                    onCompleted();
                if (!isRepeate)
                    destory();
                else
                    reStartTimer();
            }
        }
    }

    /// <summary>
    /// 获取剩余时间
    /// </summary>
    /// <returns></returns>
    public float GetTimeNow()
    {
        return Mathf.Clamp(timeTarget - now, 0, timeTarget);
    }

    /// <summary>
    /// 计时结束
    /// </summary>
    public void destory()
    {
        isTimer = false;
        isEnd = true;
        if (isDestory)
            Destroy(gameObject);
    }

    /// <summary>
    /// 暂停计时
    /// </summary>
    private float _pauseTime;

    public void pauseTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束！");
        }
        else
        {
            if (isTimer)
            {
                isTimer = false;
                _pauseTime = TimeNow;
            }
        }
    }

    /// <summary>
    /// 继续计时
    /// </summary>
    public void connitueTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束！请从新计时！");
        }
        else
        {
            if (!isTimer)
            {
                offsetTime += TimeNow - _pauseTime;
                isTimer = true;
            }
        }
    }

    /// <summary>
    /// 重新计时
    /// </summary>
    public void reStartTimer()
    {
        timeStart = TimeNow;
        offsetTime = 0;
    }

    /// <summary>
    /// 更改目标时间
    /// </summary>
    /// <param name="time_"></param>
    public void changeTargetTime(float time_)
    {
        timeTarget = time_;
        timeStart = TimeNow;
    }

    /// <summary>
    /// 游戏暂停调用
    /// </summary>
    /// <param name="isPause_"></param>
    private void OnApplicationPause(bool isPause_)
    {
        if (isPause_)
            pauseTimer();
        else
            connitueTimer();
    }
}