# SkyByDns

> Application web de gestion de domaines et enregistrements DNS / Web application for managing domains and DNS records

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![React](https://img.shields.io/badge/React-18+-61DAFB?style=flat&logo=react&logoColor=white)](https://react.dev)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.x-3178C6?style=flat&logo=typescript&logoColor=white)](https://www.typescriptlang.org)
[![Vite](https://img.shields.io/badge/Vite-5.x-646CFF?style=flat&logo=vite&logoColor=white)](https://vitejs.dev)
[![shadcn/ui](https://img.shields.io/badge/shadcn%2Fui-latest-000000?style=flat)](https://ui.shadcn.com)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat&logo=docker&logoColor=white)](https://www.docker.com)
[![Azure DevOps](https://img.shields.io/badge/Azure%20DevOps-CI%2FCD-0078D7?style=flat&logo=azuredevops&logoColor=white)](https://dev.azure.com)

## 🇫🇷 Français | 🇬🇧 English

[Voir en français](#-présentation) | [View in English](#-overview)

---

## 🇫🇷 Présentation

SkyByDns est une application web complète de gestion de domaines et d'enregistrements DNS. Le backend suit une Clean Architecture stricte et le frontend propose un dashboard React moderne avec shadcn/ui. Développée dans le cadre du projet P3 DIIAGE avec pipeline CI/CD Azure DevOps.

## Stack technique

| Composant | Technologies |
|-----------|-------------|
| Backend | C#, ASP.NET Core, Clean Architecture, Entity Framework Core |
| Frontend | React, Vite, TypeScript, shadcn/ui, Nginx |
| Base de données | PostgreSQL |
| Infrastructure | Docker, Docker Compose |
| CI/CD | Azure DevOps |
| Tests | Tests unitaires et d'intégration (.NET) |

## Architecture

```
SkyByDns-repo/
├── backend/            # ASP.NET Core — Clean Architecture
│   ├── Domain/         # Entités, interfaces métier
│   ├── Application/    # Cas d'usage, DTOs, services
│   ├── Infrastructure/ # EF Core, accès base de données
│   └── API/            # Controllers REST, auth JWT
└── frontend/           # React / Vite
    ├── src/            # Composants, pages, services API
    └── nginx.conf      # Configuration Nginx (production)
```

> Voir aussi : [backend/](./backend/) · [frontend/](./frontend/)

## Fonctionnalités principales

- CRUD des domaines et enregistrements DNS
- Authentification JWT (login sécurisé)
- Dashboard React avec composants shadcn/ui
- Pipeline CI/CD Azure DevOps
- Tests unitaires et d'intégration backend

## Lancer en local

### Prérequis

- Docker & Docker Compose
- .NET 8 SDK (sans Docker)
- Node.js 20+ (sans Docker)

### Avec Docker Compose

```bash
git clone https://github.com/NizardV/SkyByDns
cd SkyByDns-repo
docker compose up -d
```

### Backend seul (sans Docker)

```bash
cd backend
dotnet restore
dotnet run --project API
```

L'API sera disponible sur `https://localhost:5001`.

### Frontend seul (sans Docker)

```bash
cd frontend
npm install
npm run dev
```

Le dashboard sera disponible sur `http://localhost:5173`.

## Équipe

Projet P3 DIIAGE — 4 contributeurs

| Rôle | Nom |
|------|-----|
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |

---

## 🇬🇧 Overview

SkyByDns is a full-stack web application for managing domains and DNS records. The backend follows strict Clean Architecture and the frontend provides a modern React dashboard with shadcn/ui. Developed as part of the DIIAGE P3 project with an Azure DevOps CI/CD pipeline.

## Tech stack

| Component | Technologies |
|-----------|-------------|
| Backend | C#, ASP.NET Core, Clean Architecture, Entity Framework Core |
| Frontend | React, Vite, TypeScript, shadcn/ui, Nginx |
| Database | PostgreSQL |
| Infrastructure | Docker, Docker Compose |
| CI/CD | Azure DevOps |
| Tests | Unit and integration tests (.NET) |

## Architecture

```
SkyByDns-repo/
├── backend/            # ASP.NET Core — Clean Architecture
│   ├── Domain/         # Business entities, interfaces
│   ├── Application/    # Use cases, DTOs, services
│   ├── Infrastructure/ # EF Core, database access
│   └── API/            # REST controllers, JWT auth
└── frontend/           # React / Vite
    ├── src/            # Components, pages, API services
    └── nginx.conf      # Nginx configuration (production)
```

> See also: [backend/](./backend/) · [frontend/](./frontend/)

## Key features

- Domain and DNS record CRUD
- JWT authentication (secure login)
- React dashboard with shadcn/ui components
- Azure DevOps CI/CD pipeline
- Backend unit and integration tests

## Run locally

### Prerequisites

- Docker & Docker Compose
- .NET 8 SDK (without Docker)
- Node.js 20+ (without Docker)

### With Docker Compose

```bash
git clone https://github.com/NizardV/SkyByDns
cd SkyByDns-repo
docker compose up -d
```

### Backend only (without Docker)

```bash
cd backend
dotnet restore
dotnet run --project API
```

API available at `https://localhost:5001`.

### Frontend only (without Docker)

```bash
cd frontend
npm install
npm run dev
```

Dashboard available at `http://localhost:5173`.

## Team

P3 DIIAGE project — 4 contributors

| Role | Name |
|------|------|
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |
