using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Managers;
using UnityEngine;

namespace EnemyBehaviorTrees.Nodes
{
    // This action changes an NPC Agent's current Navigation Activity to a new one or reaffirms it.
    
    public class SetNavigationActivityTo : Node
    {
        private NavigationActivity newActivity;
    
        // Constructor - declare which activity to change to
        public SetNavigationActivityTo(NavigationActivity newActivity)
        {
            this.newActivity = newActivity;
            Name = $"Set NavigationActivity to {newActivity}";
        }
        
        // OnReset() - empty
        protected override void OnReset() { }
        
        protected override NodeStatus OnRun()
        {
            // Check instances
            if (EnemyBehaviorTreeGameManager.Instance == null || EnemyBehaviorTreeGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NodeStatus.FAILURE;
            }
    
            // Set Agent's Navigation Activity to new activity and return Success
            EnemyBehaviorTreeGameManager.Instance.NPC.MyActivity = newActivity;
    
            Debug.Log($"Enemy's current navigation activity is {newActivity}");
            return NodeStatus.SUCCESS;
        }
    }
}