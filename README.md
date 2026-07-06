# Job Tracker API

A C# ASP.NET Core Web API for tracking job applications.

## Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger

## Features

- Create job applications
- View all job applications
- View a single application by ID
- Update an application
- Delete an application
- Persistent SQLite database
- Swagger API testing

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/jobapplications` | Get all job applications |
| GET | `/api/jobapplications/{id}` | Get one job application |
| POST | `/api/jobapplications` | Create a new job application |
| PUT | `/api/jobapplications/{id}` | Update a job application |
| DELETE | `/api/jobapplications/{id}` | Delete a job application |

## Example Job Application

```json
{
  "company": "Google",
  "role": "Junior Software Developer",
  "status": "Applied",
  "appliedDate": "2026-07-05T00:00:00"
}
