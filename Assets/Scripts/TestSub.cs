using UnityEngine;
using System.Collections;

public class TestSub : MonoBehaviour, IMessage<UnitsSelectedMessage> {

	// Use this for initialization
	void Start () {
	    GameManagerComponent.MessageBus.Subscribe(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Handle(UnitsSelectedMessage message) {
        Debug.Log(message.SelectedUnits.Count);
    }
}
