﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStateMessage : IMessage
{

    Messaging Messenger;
    private Flags _Flag;
    private States _CurrentState;

    public void construct(States GameState, Flags SetFlag)
    {
        _CurrentState = GameState;
        _Flag = SetFlag;
    }

    public MessageType GetMessageType()
    {
        return MessageType.GAME_STATE;
    }

    public States GetState()
    {
        return _CurrentState;
    }

    public Flags GetFlag()
    {
        return _Flag;
    }

}
