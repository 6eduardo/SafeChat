# SafeChat

Aplicação de mensagens instantâneas com encriptação ponta-a-ponta (E2EE) desenvolvida em .NET MAUI + ASP.NET Core + SignalR.

## Arquitectura

```
SafeChat.slnx
├── SafeChat.Domain        — Entidades e regras de negócio (core)
├── SafeChat.Application   — Casos de uso (CQRS com MediatR + FluentValidation)
├── SafeChat.Infrastructure — Persistência (EF Core + SQL Server), segurança
├── SafeChat.API           — Web API ASP.NET Core + SignalR + JWT
└── SafeChat.Mobile        — Frontend .NET MAUI (Android, iOS, Windows, Mac)
```

## Segurança

| Tecnologia | Função |
|---|---|
| **RSA-2048** | Par de chaves único por utilizador para troca de chaves AES |
| **AES-256-CBC** | Encriptação individual por mensagem com chave aleatória |
| **SecureStorage / Android Keystore** | Chave privada nunca sai do dispositivo |
| **BCrypt** | Hashing de passwords |

**Propriedade fundamental**: o servidor nunca tem acesso ao texto claro — apenas encaminha payloads encriptados.

## Funcionalidades

- Autenticação (registo, login, logout) com JWT
- Gestão de chaves RSA no dispositivo
- Conversas entre utilizadores
- Envio e recepção de mensagens com encriptação E2EE
- Estado online/offline em tempo real via SignalR
- Histórico de mensagens

## Stack

- **Backend**: ASP.NET Core 10, Entity Framework Core 10, SignalR, JWT Bearer, SQL Server
- **Mobile**: .NET MAUI, MVVM (CommunityToolkit.Mvvm)
- **Criptografia**: RSA-2048, AES-256-CBC, BCrypt

## Sprints

| Sprint | Foco |
|---|---|
| 1 | Fundação Backend + API + DB + Autenticação + Ecrãs Login/Registo |
| 2 | Segurança Base — RSA + SecureStorage |
| 3 | Comunicação — SignalR Hub |
| 4 | Chat — Contactos, Conversas, Interface de mensagens |
| 5 | Segurança Avançada — AES por mensagem + Desencriptação local |
| 6 | Finalização — UI, optimizações, documentação |
