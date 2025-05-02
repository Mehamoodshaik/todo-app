# .NET Core + Vue To-Do Application

This is a full-stack To-Do application built with **Vue.js (PrimeVue)** on the frontend and **ASP.NET Core + Entity Framework Core** on the backend. It supports a dynamic data provider system using the **Factory Pattern**, allowing users to choose between:

- **API Provider** (SQL Database)
- **Local Storage Provider**

---

### Features

Select data provider (API or local) using Factory Pattern  
Add, update, delete todos  
Set due date and time  
Mark as completed 
Toggle hide/show completed todos
Debounced search for performance  
Moolah website inspired UI with gradients and Feather-style icons  

---

##  My Approach

This project was built to demonstrate my ability to design flexible architecture using design patterns and create a modern, user-friendly frontend experience.

I implemented the Factory Pattern in the frontend to dynamically switch between LocalProvider and ApiProvider based on user selection.

I used debounced search with Lodash to improve responsiveness and avoid unnecessary API calls.

Although not required, I applied a custom UI theme inspired by Moolah website to showcase design creativity and attention to user experience.

I ensured proper handling of date and time with timezone normalization so due dates are consistent across frontend and backend.

---

## Tech Stack


- Frontend:    Vue 3, PrimeVue, Vite        
- Styling:     PrimeVue components + custom CSS
- Backend:     C# with ASP.NET Core (8) Web API 
- Database:    MySQL Server        
- Design Pattern:  Factory Pattern 

---

## Setup Instructions

Frontend

- cd todo-app-frontend
- npm install
- npm run dev

Backend 
- cd todo-app-backend
- dotnet restore
- dotnet ef database update
- dotnet run
