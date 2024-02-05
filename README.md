# tudu
todo web app - managing tasks, goals, projects

Custom web app designed & coded as a practice project and a tool to help me organize stuff I need to manage

# features
Most basic and crucial functionality I need, feasible for version 1.0.

- I want to browse and manage daily todo list
- I want to have a todo backlog that can be filtered, sorted and grouped
- I want to customize a todo by tag, urgency, importance, deadline, status, description and link other todos as subtodos
- I want to move unfinished todos in daily list for next day, specified date or to backlog

Other features that might come in later versions:

- Project/goals management
- Calendar
- Reports/dashboards
- Habits support
- Notifications
- Managing notes and knowledge base, adding notes per day
  
More to be defined

# implementation
I'd like to write this app as a REST web API in .NET and make responsive UI to support desktop and mobile, not yet decided in what technology (probably React).

Ideally, I want to dockerize the project and use free tier of Azure cloud services to host tudu, which means for instance NoSQL Azure CosmosDb as database.