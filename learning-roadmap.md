# Learning Roadmap - Diego's Full-Stack Development Journey

## Overview

This roadmap documents Diego's structured learning path towards becoming a full-stack developer with expertise in both modern web technologies and backend development. The focus is on practical, hands-on learning through completing exercises and building real-world projects.

## Current Status

### ✅ Completed Exercises

| Date       | Exercise               | Technology          | Key Learning                                               |
| ---------- | ---------------------- | ------------------- | ---------------------------------------------------------- |
| 2026-08-31 | Counter Redux Toolkit  | React Redux Toolkit | Estado global, `createSlice`, `useSelector`, `useDispatch` |
| 2026-08-30 | Express Routes Pattern | Express.js          | Patrón de router, controladores, servicios                 |
| 2026-08-31 | MongoDB Migration Plan | Mongoose/MongoDB    | Pensamiento documental vs relacional, modelado de esquemas |

### 🔄 Current Focus

**Integration Project**: Conectar el contador Redux con Express backend y migrar a MongoDB

## Learning Objectives

### 1. Backend Node.js Ecosystem

#### Understanding NestJS, Next.js, and Express Differences

- **NestJS**: Framework completo con arquitectura basada en módulos, decoradores, inyección de dependencias
- **Next.js**: Full-stack React framework (actualmente sin React 18 apropiado)
- **Express.js**: Minimal, flexible, cuando necesitamos control total

**Key Decision**: Diego elige Express para proyectos pequeños por su simplicidad y control total

#### Backend Architecture Best Practices

- Patrones de organización limpios (`routes/`, `controllers/`, `services/`)
- Redundancia evitable vs. repetitiva vs. necesaria
- Escalabilidad para crecimiento futuro

### 2. Programming Language Fluency

#### Object-Oriented Programming Across Languages

| Language | Key Syntax/Concepts                               | Typical Use Cases                          |
| -------- | ------------------------------------------------- | ------------------------------------------ |
| **Rust** | Ownership, borrowing, type inference              | Sistemas, WebAssembly, concurrencia segura |
| **PHP**  | Herencia, interfaces, composición                 | Web, CMS, scripts del lado del servidor    |
| GO       | Pointers, channels, goroutines                    | Web, CLIs, microservicios                  |
| **C**    | Estructuras, punteros, programación de bajo nivel | Sistemas, embebidos                        |
| **C++**  | STL, RAII, templates                              | Aplicaciones de alto rendimiento, juegos   |
| **C#**   | CLIs, .NET, Web ASP.NET                           | Windows apps, servicios web                |
| **Java** | Collections, streams, concurrencia                | Aplicaciones empresariales, Android        |
| **Python**| Duck typing, decoradores, list/dict comprehensions, virtualenv | **ML/IA**, ciencia de datos, scripting, backend (Django/FastAPI) |

### 3. Modern Development Practices

#### Testing & CI/CD

- **Jest/React Testing Library** - Testing frontend
- **Playwright/Cypress** - Testing E2E
- **Docker** - Containerización de aplicaciones completas
- **GitHub Actions** - CI/CD pipelines

#### DevOps Tools (Nix-based)

```nix
# From nixconf/home-manager/features/work.nix:62-68
playwright-driver.browsers
cypress
chromedriver
geckodriver
chromium
```

## Roadmap Timeline

### Phase 1: Foundation (Weeks 1-4)

1. **Complete all three current exercises**
   - Contador Redux (integrar con backend)
   - Patrón de rutas Express
   - Plan de migración MongoDB

2. **Build integration project**
   - Frontend: Contador Redux + Provider + UI
   - Backend: Express + MongoDB
   - Testing: Unit tests + E2E tests

3. **Learn NestJS** (opcional pero recomendado)
   - Entender su arquitectura basada en módulos
   - Comparar con Express

### Phase 2: Language Expansion (Weeks 5-12)

1. **POO Fundamentals**
   - Syntax comparison across languages
   - Best practices for each language

2. **Rust Journey**
   - Complete Exercism Rust track (99 exercises)
   - Build a modern Rust web service

3. **Other languages**
   - PHP for web development
   - C/C++ for understanding systems programming
   - C# for .NET ecosystem

### Phase 3: Advanced Topics (Weeks 13-24)

1. **DevOps & Deployment**
   - Dockerize applications
   - Set up CI/CD pipelines
   - Learn monitoring and logging

2. **Advanced Frontend**
   - Next.js (con React 18 apropiado)
   - Advanced React patterns
   - Performance optimization

3. **Enterprise Backend**
   - Learn NestJS properly
   - Microservices architecture
   - GraphQL vs REST

## Learning Resources

### Primary Sources

- **JSCamp (midudev)** - Bootcamp format, practical projects
- **Exercism** - Language tracks with exercises
- **TryHackMe** - Cybersecurity fundamentals
- **Mimo** - App-based learning for modern web dev

### Supplementary

- **Bootcamps and online courses** for structured learning
- **Open source projects** for real-world code review
- **Community contributions** for collaborative learning

## Project Structure

### Current Progress

```
workspace/
├── AGENT-records/                    # Learning records
│   ├── 2026-08-31-ejercicio-o1-vim-learn-feedback.md
│   └── ... other learning notes
├── ptd-talento-back/                 # Backend projects
│   └── src/routes/2026-08-30-express-rutas.md
├── ptd-talento-front/                # Frontend projects
│   └── src/store/slices/2026-08-31-ejercicio-contador-redux.md
├── PCE-Agencia/                      # Real-world projects
│   └── server/2026-08-31-PLAN-mongodb-migracion.md
├── motivational.md                   # Resumen de la sesión del 31-08 (lazygit, vim, Redux Toolkit, Docker MongoDB, moraleja)
└── theme-neonforge.css               # Preset NeonForge (hermano: ghaerdi/neonforge) — candidato UI para MongoDB/PCE-Agencia
```

### Future Goals

```
workspace/
├── learning-roadmap.md              # This document (updated regularly)
├── projects/                         # All completed projects
│   ├── counter-redux-express/
│   ├── inventory-mongodb/
│   └── ... other projects
└── docs/                             # Technical documentation
```

## Testing & CI/CD Strategy

### Local Development Testing

```bash
# Jest for unit tests
npm test

# React Testing Library for component tests
npm run test:components

# Playwright for E2E tests
npm run test:e2e
```

### CI/CD Pipeline

```yaml
# Typical pipeline setup
stages:
  - test
  - build
  - deploy

jobs:
  - test:
      steps:
        - run_tests
        - lint_code
        - security_scan

  - deploy:
      needs: [test]
      steps:
        - build_image
        - push_to_registry
        - deploy_to_environment
```

## Key Principles

### 1. Learn by Doing

- Complete exercises fully
- Build real-world projects
- Document learning process

### 2. Cross-Language Understanding

- Learn one language thoroughly (Rust)
- Understand patterns across languages
- Choose right tool for the job

### 3. Practical Over Theoretical

- Focus on implementation
- Learn from real codebases
- Solve real problems

### 4. Consistent Documentation

- Keep learning records
- Document decisions
- Share knowledge with others

## Next Steps

### Immediate (This Week)

1. ✅ Complete the three current exercises
2. ✅ Write feedback for each exercise
3. 🔄 Create this learning roadmap document
4. 🔄 Set up testing framework
5. 🔄 Begin integration project

### Short-term (Next 4 Weeks)

1. Integrate Redux contador con backend Express
2. Implement MongoDB migration
3. Add unit tests for all components
4. Set up CI/CD pipeline

### Medium-term (Next 3 Months)

1. Complete Rust learning track
2. Build portfolio projects
3. Learn DevOps best practices
4. Prepare for job interviews

## Success Metrics

### Technical Skills

- [ ] Comprender NestJS, Express, Next.js y cuándo usar cada uno
- [ ] Escribir código eficiente y bien estructurado en múltiples lenguajes
- [ ] Implementar testing completo (unit + E2E)
- [ ] Desplegar aplicaciones en producción con CI/CD

### Soft Skills

- [ ] Documentar proceso de aprendizaje
- [ ] Colaborar en proyectos de código abierto
- [ ] Resolver problemas complejos
- [ ] Comunicar ideas técnicas

## Resources for Future Learning

### Books & Courses

- "You Don't Know JS" series
- "Elixir School's Getting Started"
- Various Rust books and tutorials

### Communities

- Rust subreddit
- Exercism Discord
- Node.js Community

### Tools

- GitHub Copilot/IDE integrations
- Nix for reproducible environments
- Docker Compose for local development

## PTD-Talento Specialized Training

### Official Resources
- **Mapa para el PTD (Roadmap)**: https://sites.google.com/cincinnatus.edu.do/mapa-para-el-ptd/%C3%ADndice?pli=1&authuser=2
- **Guía de entorno Local PTD-Talento**: https://docs.google.com/document/d/1wlv5RKBafG5zvfx83KDhmtIh8qUWSw4L/edit
- **Tech stack**: https://docs.google.com/spreadsheets/d/1Y7cvkfP1SjJdA-rkFGlMnbvFQhWMyG7-/edit?gid=1608685851#gid=1608685851

### Resource Folders
- **Carpeta PTD (main project)**: https://drive.google.com/drive/u/2/folders/0AIStrS-iHtWVUk9PVA
- **Carpeta Recursos (DevOPS, Dev, QA, PM, DB, etc.)**: https://drive.google.com/drive/u/2/folders/1OmNHh8curwmsJaSWJIRbECMzLzQ4zRpfe

### Specialized Guides Available
- Docker
- React hooks
- Redux
- And other development resources

### Suggested Integration
1. **Environment Setup**: Follow the local environment guide to configure your PTD-Talento development setup
2. **Core Technologies**: Study Docker, React hooks, and Redux guides as focused learning modules
3. **Project Exploration**: Use the main PTD folder to understand the codebase structure
4. **Role-Specific Training**: Explore the Recursos folder for DevOps, QA, PM, and DB materials

### Learning Approach
- Integrate PTD-Talento resources with your full-stack development journey
- Apply Docker knowledge to containerize your Express/MongoDB projects
- Implement React hooks in your frontend applications
- Use Redux patterns for state management in complex features
- Explore DevOps practices for CI/CD pipeline improvements

---

_Last Updated: 2026-08-31_
_Author: Diego (following Professor's guidance)_
_Status: In Progress - Phase 1 (Foundation)_

Este roadmap evolucionará a medida que avanzamos en el aprendizaje. Es un documento vivo que documenta el progreso y establece objetivos para el futuro.
