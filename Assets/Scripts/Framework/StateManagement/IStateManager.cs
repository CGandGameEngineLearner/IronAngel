using System.Collections.Generic;

public interface IStateManager
{
    public void AddState(int stateCode);
    public void RemoveState(int stateCode);

    public bool IncludeState(int stateCode);

    public bool CheckState(List<int> included, List<int> excluded);
}