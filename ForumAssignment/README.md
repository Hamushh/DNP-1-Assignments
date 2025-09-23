# Assignment 3

## Overview
This is the third assignment for DNP-1.  
It builds directly upon the work from Assignment 1 & 2, reusing the same repository structure, while preparing the foundation for the CLI and the UI whilst adding file repositories to add persistance to the CRUD Application.

## Structure
- `Server`
  - `CLI` — Commnad Line Interface/UI for interactibility
  - `Entities` — domain model classes (e.g., User, Post, Comment)
  - `FileRepositories` — writing and reading to files in json format instead of reading off CLI
  - `InMemoryRepositories` — in-memory data storage implementations
  - `RepositoryContracts` — repository interfaces
  
- `ForumAssignment.sln` — solution file for the Application

## How to Build
```bash
dotnet build ForumAssignment.sln