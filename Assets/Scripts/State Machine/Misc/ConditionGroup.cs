using UnityEngine;
using System.Collections;
using FSM;

[System.Serializable]
public class ConditionGroup
{
    public bool isAnd = true;
    public Condition[] conditions;

    public bool Result(Controller controller)
    {
        if (isAnd)
        {  
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].Decide(controller))
                {
                    return false; // if any of the conditions are false, the And fails
                }
            }
            return true; // all conditions were true, and passes
        }
        else
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i].Decide(controller))
                {
                    return true; // if any of the conditions are true, the Or passes
                }
            }
        }
        return false;
    }

}
