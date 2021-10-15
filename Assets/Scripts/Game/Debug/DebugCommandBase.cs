using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP.Debug
{
    public class DebugCommandBase
    {
        private string _commandId;
        private string _commandDescription;
        private string _commandFormat;

        public string CommandId { get { return _commandId; } }
        public string CommandDescription { get { return _commandDescription; } }
        public string CommandFormat { get { return _commandFormat; } }

        public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
        {
            _commandId = commandId;
            _commandDescription = commandDescription;
            _commandFormat = commandFormat;
        }

    }

    public class DebugCommand : DebugCommandBase
    {
        private Action command;

        public DebugCommand(string commandId, string commandDescription, string commandFormat, Action command) : base(commandId, commandDescription, commandFormat)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command.Invoke();
        }
    }

    public class DebugCommand<T1> : DebugCommandBase
    {
        private Action<T1> command;

        public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T1> command) : base(commandId, commandDescription, commandFormat)
        {
            this.command = command;
        }

        public void Invoke(T1 value)
        {
            command.Invoke(value);
        }
    }

    public class DebugCommand<T1, T2> : DebugCommandBase
    {
        private Action<T1, T2> command;

        public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T1, T2> command) : base(commandId, commandDescription, commandFormat)
        {
            this.command = command;
        }

        public void Invoke(T1 value1, T2 value2)
        {
            command.Invoke(value1, value2);
        }
    }

    public class DebugCommand<T1, T2, T3> : DebugCommandBase
    {
        private Action<T1, T2, T3> command;

        public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T1, T2, T3> command) : base(commandId, commandDescription, commandFormat)
        {
            this.command = command;
        }

        public void Invoke(T1 value1, T2 value2, T3 value3)
        {
            command.Invoke(value1, value2, value3);
        }
    }
}
