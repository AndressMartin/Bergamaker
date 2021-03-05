using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor
{
    int PV { get; }
    int MN { get; }
    int PA { get; }

    int AlterarPV(int alteracao);
    int AlterarMN(int alteracao);
    int AlterarPA(int alteracao);

}
