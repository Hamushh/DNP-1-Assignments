# Assignment 2

## Overview
This is the second assignment for DNP-1.  
It builds directly upon the work from Assignment 1, reusing the same domain model and repository structure, while preparing the foundation for the CLI and the UI which we can execute and interact with.

## Structure
- `Server`
  - `CLI` — Commnad Line Interface/UI for interactibility
  - `Entities` — domain model classes (e.g., User, Post, Comment)
  - `InMemoryRepositories` — in-memory data storage implementations
  - `RepositoryContracts` — repository interfaces
  
- `Assignment_2.sln` — solution file for the assignment

## How to Build
```bash
dotnet build Assignment_2.sln