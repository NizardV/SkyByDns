# SkyByDns — Frontend

> Interface web React/Vite pour la gestion DNS / React/Vite web interface for DNS management

[![React](https://img.shields.io/badge/React-18+-61DAFB?style=flat&logo=react&logoColor=white)](https://react.dev)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.x-3178C6?style=flat&logo=typescript&logoColor=white)](https://www.typescriptlang.org)
[![Vite](https://img.shields.io/badge/Vite-5.x-646CFF?style=flat&logo=vite&logoColor=white)](https://vitejs.dev)
[![shadcn/ui](https://img.shields.io/badge/shadcn%2Fui-latest-000000?style=flat)](https://ui.shadcn.com)

> Retour au projet principal / Back to main project: [SkyByDns](../README.md)

---

## Installation / Install

```bash
npm install
```

## Lancer / Run

```bash
# Développement / Development
npm run dev

# Build de production / Production build
npm run build

# Prévisualiser le build / Preview the build
npm run preview
```

## Variables d'environnement / Environment variables

```env
VITE_API_URL=http://localhost:5001
```

## Structure

```
src/
├── components/   # Composants réutilisables / Reusable components (shadcn/ui)
├── pages/        # Pages de l'application / Application pages
├── services/     # Appels API / API calls
└── types/        # Types TypeScript partagés / Shared TypeScript types
```

La documentation complète est dans le [README principal](../README.md).
Full documentation in the [main README](../README.md).
