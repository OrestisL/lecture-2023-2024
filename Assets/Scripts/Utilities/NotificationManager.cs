using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class Notification : Command
{
    public string Message { get; private set; }
    public float LifeTime = 1.0f;
    public override bool IsFinished => LifeTime <= 0;
    public Notification(string message, float liveTime = 1.0f)
    {
        Message = message;
        LifeTime = liveTime;
    }
    public override void Execute()
    {
        LifeTime -= Time.deltaTime;
    }
}

public class NotificationManager : GenericSingleton<NotificationManager>
{
    private Queue<Notification> notifications = new Queue<Notification>();
    private Notification _currentNotif;
    public GameObject notificationPanel;
    private TextMeshProUGUI notificationText;

    public override void Awake()
    {
        base.Awake();
        notificationText = notificationPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        ProcessNotifications();
    }

    public void AddNotification(string message, float duration, bool force = false)
    {
        Notification notification = new Notification(message,  duration);

        notifications.Enqueue(notification);

        if (_currentNotif == null || force)
        {
            _currentNotif = notifications.Dequeue();

            notificationText.text = _currentNotif.Message;
            notificationPanel.SetActive(true);
        }
    }

    void ProcessNotifications()
    {
        if (_currentNotif == null)
        {
            notificationPanel.SetActive(false);
        }
        else
        {
            if (!_currentNotif.IsFinished)
            {
                _currentNotif.Execute();
            }
            else if (_currentNotif.IsFinished)
            {
                if (notifications.Count > 0)
                {
                    _currentNotif = notifications.Dequeue();

                    notificationText.text = _currentNotif.Message;
                }
                else { _currentNotif = null;  }
            }
        }
    }
}
