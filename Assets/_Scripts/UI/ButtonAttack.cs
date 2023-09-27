using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(EventTrigger))]
public class ButtonAttack : MonoBehaviour
{
    private EventTrigger _eventTrigger;
    private Player _player;   

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
    }     
    private void Start()
    {      
        EventTrigger.Entry pointerDownEvent = new EventTrigger.Entry();
        pointerDownEvent.eventID = EventTriggerType.PointerDown;
        _eventTrigger.triggers.Add(pointerDownEvent);

        EventTrigger.Entry pointerUpEvent = new EventTrigger.Entry();
        pointerUpEvent.eventID = EventTriggerType.PointerUp;
        _eventTrigger.triggers.Add(pointerUpEvent);
       
        pointerDownEvent.callback.AddListener((data) => { _player.Attack(); });
        pointerUpEvent.callback.AddListener((data) => { _player.StopAttack(); });
    }
}
