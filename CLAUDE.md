# TruckApi — Contexto do Projeto

## O que é

API de monitoramento de máquinas pesadas (escavadeiras, tratores, patrol, etc.) para empresas de construção/terraplanagem. Multi-tenant: cada empresa tem seus próprios dados isolados via `company_id`.

Funcionalidades principais:
- Rastreamento GPS em tempo real das máquinas
- Checklist digital pré-operação (aprovado / aprovado com obs / reprovado)
- Registro de horas trabalhadas por operador/máquina
- Histórico de manutenções (preventiva e corretiva)
- Geofencing com alertas de entrada/saída de área
- Dashboards de produtividade e relatórios

## Stack

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 10 — Minimal APIs |
| ORM | Entity Framework Core 10 + Npgsql |
| Banco | PostgreSQL |
| Cache / Filas | Redis (StackExchange.Redis) |
| Jobs | Hangfire + Hangfire.PostgreSql |
| Runtime | .NET 10 |

## Quem desenvolve

Desenvolvedor vindo de NestJS/TypeScript (Fastify, BullMQ, Redis, PostgreSQL, Drizzle ORM). Prefere analogias com o ecossistema Node.js quando aprendendo conceitos novos do .NET.

## Estrutura de pastas

```
TruckApi/
├── Program.cs                          # Bootstrap da app (equivalente ao main.ts do NestJS)
├── appsettings.json                    # Config base
├── appsettings.Development.json        # Config de dev com credenciais (não commitado)
├── Infrastructure/
│   └── Database/
│       ├── AppDbContext.cs             # DbContext — registra todas as entidades e relacionamentos
│       └── Entities/                  # Entidades EF Core (equivalente aos schemas do Drizzle)
│           ├── Company.cs
│           ├── User.cs
│           ├── Machine.cs
│           ├── ChecklistTemplate.cs
│           ├── ChecklistTemplateItem.cs
│           ├── Checklist.cs
│           ├── ChecklistItemResponse.cs
│           ├── HourRecord.cs
│           ├── MachineLocation.cs
│           ├── MaintenanceRecord.cs
│           ├── Geofence.cs
│           └── GeofenceEvent.cs
```

## Entidades e relacionamentos

```
Company
  ├── Users[]           (SetNull ao deletar company)
  ├── Machines[]        (Restrict)
  ├── ChecklistTemplates[]
  └── Geofences[]

Machine (company_id)
  ├── Checklists[]
  ├── HourRecords[]
  ├── Locations[]       (MachineLocation — id long, alta freq.)
  └── MaintenanceRecords[]

ChecklistTemplate (company_id, machine_type?)
  └── ChecklistTemplateItems[]

Checklist (machine_id, operator_id, template_id)
  └── ChecklistItemResponses[]

HourRecord (machine_id, operator_id)

Geofence (company_id, polygon JSONB)
  └── GeofenceEvents[]  (machine_id — id long, alta freq.)
```

## Enums e convenções

- Todos os enums salvos como **string** no banco (`HasConversion<string>()`)
- `UserRole`: Admin | CompanyManager | CompanySupervisor | CompanyOperator
- `MachineType`: Excavator | Grader | Backhoe | Tractor | Bulldozer | Crane | Forklift | Truck | Other
- `MachineStatus`: Active | UnderMaintenance | Stopped
- `ChecklistResult`: Approved | ApprovedWithObservations | Rejected
- `ChecklistItemStatus`: Ok | WithObservation | Failed
- `MaintenanceType`: Preventive | Corrective
- `GeofenceEventType`: Enter | Exit
- IDs das entidades principais: `string` (nanoid — a implementar)
- IDs de tabelas de alta frequência (`MachineLocation`, `GeofenceEvent`): `long` (auto-increment)
- Namespace padrão: `TruckApi.<Camada>.<Subpasta>` (ex: `TruckApi.Infrastructure.Database.Entities`)
- Nomes de colunas: snake_case via atributo `[Column("nome_coluna")]`

## Comandos úteis

```bash
dotnet watch                          # dev com hot reload
dotnet build                          # build
dotnet ef migrations add <Nome>       # criar migration
dotnet ef database update             # aplicar migrations
dotnet add package <NomePacote>       # instalar dependência (equivalente ao npm install)
```

## Connection strings (Development)

Configuradas em `appsettings.Development.json` (não commitado):
- `ConnectionStrings:Postgres` — formato Npgsql
- `ConnectionStrings:Redis` — formato StackExchange.Redis

## Roadmap

- **MVP**: Machines, Checklist, HourRecords, mapa GPS
- **v2**: Relatórios, alertas, manutenção preventiva, app mobile
- **v3**: Geofencing, IoT/sensores, integração ERP, dashboards avançados
