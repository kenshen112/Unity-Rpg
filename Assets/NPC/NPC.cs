﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    NPCManager NpcM;
    NPCData NpcData;
    public int NpcID;
    StateMachine states;    
    DialogueManager Dialogue;
    public GameObject SpeakerProfile;
    bool Collided;

    private void Start()
    {
        Dialogue = FindObjectOfType<DialogueManager>();
        NpcM = FindObjectOfType<NPCManager>();
        states = FindObjectOfType<StateMachine>();
    }

    void ApplyNPC()
    {
        NpcData = NpcM.ToInit[NpcID - 1];
        NpcData.CurrentSpeaker = SpeakerProfile;
        NpcData.Construct(SpeakerProfile);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collided = true;
        }

        else
        {
            Collided = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collided = false;
        }
    }

    void pollEvents()
    {
        Debug.Log("Current FLAG : " + states.CurrrentFlag.Flag);
        Debug.Log("Current FLAG ID : " + states.CurrrentFlag.ID);

        NPCEventData ToExecute;

        for (int i = 0; i < NpcData.EventData.Count; i++)
        {
            Debug.Log("FLAG : " + NpcData.EventData[i].RequiredFlag.Flag);
            Debug.Log("FLAG ID : " + NpcData.EventData[i].RequiredFlag.ID);

            if (NpcData.EventData[i].RequiredFlag.ID == states.CurrrentFlag.ID)
            {
                ToExecute = NpcData.EventData[i];
                Debug.Log("Event Executed");
                ToExecute.Execute(SpeakerProfile);
                NpcData.EventData.RemoveAt(i);
                return;
            }
        }
    }

    private void Update()
    {
        ApplyNPC();

        if (Input.GetButtonDown("Submit") && Collided )
        {
            pollEvents();
        }
    }
}
