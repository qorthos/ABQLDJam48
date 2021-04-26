using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueEventArgs : EventArgs
{
    public string Msg;
}

public class AudioEventArgs : EventArgs
{
    public AudioClip AudioClip;
    public Vector2 Position;
}