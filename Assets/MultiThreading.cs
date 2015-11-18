using UnityEngine;
using System.Collections;

public class MultiThreading
{
    private bool finished = false;
    private object handle = new object();
    private System.Threading.Thread thread = null;

 
    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (handle)
            {
                tmp = finished;
            }
            return tmp;
        }
        set
        {
            lock (handle)
            {
                finished = value;
            }
        }
    }

    public virtual void Start()
    {
        thread = new System.Threading.Thread(Run);
        thread.Start();
    }

    public virtual void Abort()
    {
        thread.Abort();
    }

    protected virtual void ThreadFunction() { }

    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (finished)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }

    private void Run()
    {
        ThreadFunction();
        finished = true;
    }

}
