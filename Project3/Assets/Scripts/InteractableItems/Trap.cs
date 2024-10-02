using Tools;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    private GameObject trapReceiver;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            trapReceiver = other.gameObject;
            trapReceiver.GetComponent<NavMeshAgent>().speed *= 0.5f;
            Timer timer = Timer.createTimer("GameTime");
            timer.startTiming(10,true,trapTimerCompleted);
        }
    }

    private void trapTimerCompleted()
    {
        trapReceiver.GetComponent<NavMeshAgent>().speed *= 2;
    }

}
