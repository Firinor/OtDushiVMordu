using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public Queue<FightCommand> commands = new();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            commands.Enqueue(FightCommand.Attack);
        }
        if (Input.GetMouseButtonDown(1))
        {
            commands.Enqueue(FightCommand.Defence);
        }
    }
}