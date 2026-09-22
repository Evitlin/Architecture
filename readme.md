# Architecture Catalog System

## Project Objective
The objective of this project is to design and implement an architectural heritage catalog system for managing and displaying information about buildings and architectural styles.

The system defines three user roles and their responsibilities:
* **Guest:** Can browse architectural styles, search and filter buildings by style or city, and view user comments and ratings.
* **User:** Inherits all guest capabilities with additional functionality to submit comments and ratings for individual buildings.
* **Administrator:** Responsible for catalog data management (creating, updating, and deleting styles and buildings) and moderating user comments.

## Tech Stack
* **Back-End:** C# .NET REST API (Layered Architecture)
* **Database:** MySQL (Entity Framework Core)
* **Front-End:** React.js
