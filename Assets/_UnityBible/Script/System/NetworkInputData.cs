using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public struct NetworkInputData : INetworkInput
    {
        public const byte MOUSEBUTTON1 = 0x01;
        public const byte MOUSEBUTTON2 = 0x02;
        public const byte KEYARROWLEFT = 0x10;
        public const byte KEYARROWRIGHT = 0x20;
        public const byte KEYSPACE = 0x40;

        public byte buttons;
        public Vector3 direction;
    }
}