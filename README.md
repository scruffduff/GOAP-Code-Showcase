# GOAP-Code-Showcase
My example of a Goal Oriented Action Planning implementation using the Unity Game Engine 

# Project Structure
I think this small scale GOAP frame provides a solid insight into my current understanding of best practises and C# syntax while being much more user friendly to quickly read through than a large scale project.  
The project’s ‘_Code’ folder is split into two main subfolders: ‘GOAP’ and ‘Game’ (both with their own assembly definitions).   The GOAP folder contains code for the GOAP framework whereas the Game folder contains code that uses the framework in the context of a game.  I hope this shows my understanding of the importance of keeping frameworks and implementation.    
# Goals & Actions Overview
## Goal & Goal Data SO
‘GoalDataSO’ is a scriptable object data container which stores all the information and settings for that goa (e.g. the goal’s name and maximum priority).  This approach is more designer friendly as it gives them a single object to interact with (rather than making sure that all the prefabs that share a goal have updated individually).
The goal component is a mono behaviour that holds a reference to that goal’s current priority and the Goal’s Data.  It does not run any logic itself (but acts more like a instanced data container).
## Action & Action Data SO
The Action mono behaviour is designed to be used a parent class that different AI behaviours inherit from.  It has four virtual functions (each called during a different stage of performing the action).  These functions run the core & shared logic of an action while the unique parts can be added after the base of the function has been run in any child classes.
‘ActionDataSO’ is designed in the same way as Goal Data.  It contains all the properties for that action (e.g. actions duration & the amount it modifies its target goal on completion).
# Agent Overview & Architecture
There are three custom components that are required by each agent; Agent Brain, Goals & Actions.
## Agent Goals
This class finds all of the agent’s goals on awake and stores them in an array.
During runtime, it handles the logic for updating the priority of each goal and then stores the goal that currently has the highest priority.
It also sends an event out whenever the goal with the highest priority changes.  This event is subscribed to by the Agent Brain to save it being checked every frame.
## Agent Actions
This class finds all of the agent’s actions on awake and stores them in an array (after checking they are all set up correctly).
The only other function of this class is to return the action with the lowest cost that also stratifies a specific goal.  The cost of each goal can be calculated on a per instance basis by each type of action by overriding the GetActionCost() function in the Action class.
## Agent Brain
This class runs the updates the behaviour logic via the current action and then calls the completed function when the task has been complete.
This class also contains multiple public functions that can be used by the current action behaviour. For example, setting the pathfinding destination.
# Taking The System Further
I hope this system proves some of my understanding of the best practises in AI programming, documentation and C# syntax.  
However, I recognise that the system would need to be build upon further to ensure that it scales in a complex production setting.   Below are a few of the improvements I would make:
To ensure, the system can handle a large number of agents at one time I would remove all the Update loop from the Agent Brain class. Instead, I would introduce a Agent Manager component.  This class would contain a list of all the active agents and only update one of those agents per frame (passing in a modified ‘Delta Time’ for how long it has been since the agent was last updated).  
To allow for more complex behaviours, I would introduce a action queue on each agent. This would allow agents to chain multiple actions together to complete a single goal.  This would mean less boilerplate code (like walking to and from objects) that currently has to be rewritten with every new added behaviours. 
When lots different types of actions exist, that all satisfy the same goal, I would expand on each of the action’s ‘Get Cost’ function to starting introducing some of the interesting aspects of utility AI. However, the frame work for picking the lowest cost goal as well calculating the cost of an action already exists in this implementation.  This could also be expanded to allow for conditional based checks for if an agent can perform a specific action at all (for example based on their skill level).
# Third Party Assets & Plugins
The only third party plugin I used was Odin Inspector and Serializer.  It does not have anything to do with AI programming. It is just a amazing tool for a designing better looking and more debug-able Unity inspector.
