using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class PossibleStats : MonoBehaviour
    {
        [field: SerializeField] public Structs.Body Body { get; protected set; }

        [field: SerializeField] public Structs.Gun Gun { get; protected set; }

        [field: SerializeField] public Structs.Combat Combat { get; protected set; }
    }

}
