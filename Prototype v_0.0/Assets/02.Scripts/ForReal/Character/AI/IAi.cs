using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAi
{
    AiState Pattern();
}

public enum AiState { Idle, Move, Attack, Dead };

