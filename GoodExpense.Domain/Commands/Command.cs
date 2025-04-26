﻿using GoodExpense.Domain.Events;

namespace GoodExpense.Domain.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; protected set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }
}