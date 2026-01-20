# Payments API

> Microsserviço responsável pelo **processamento de pagamentos** da plataforma **PAIF Games**, atuando como **consumer e publisher** em uma arquitetura **event-driven**, com foco em desacoplamento, clareza de responsabilidades e escalabilidade.

---

## 🎯 Objetivo

Este serviço é responsável por:

* Gerenciar o **ciclo de vida de pagamentos** (CRUD)
* Consumir eventos de **pedido criado**
* Executar a **lógica de processamento de pagamento**
* Publicar eventos de **pagamento processado** para outros domínios

Tudo de forma **assíncrona**, resiliente e orientada a eventos.

---

## 🧠 Visão Geral da Arquitetura

* **.NET 8**
* **Minimal APIs + Carter**
* **CQRS (Commands / Queries)**
* **MediatR**
* **MassTransit**
* **RabbitMQ**
* **PostgreSQL** (via Document Session)
* **Docker (multi-stage)**
* Pronto para ambientes distribuídos

Arquitetura em camadas:

* API (Endpoints)
* Core (Domain + Application)
* Infra (Messaging, Publishers, Configuração)
* BuildingBlocks (CQRS abstractions)

---

## 📦 Responsabilidades do Serviço

* Criar, atualizar, listar e remover pagamentos
* Manter estado do pagamento (Pending, Paid, Failed, Canceled)
* Consumir evento `OrderPlacedMessage`
* Processar pagamento (simulado)
* Publicar evento `PaymentProcessedMessage`
* Atuar como **Consumer e Publisher**

---

## 📡 Mensageria (RabbitMQ)

### 🔹 Eventos Consumidos

| Evento               | Origem  | Fila                 |
| -------------------- | ------- | -------------------- |
| `OrderPlacedMessage` | Catalog | `order_placed_queue` |

### 🔹 Eventos Publicados

| Evento                    | Destino       | Fila                      |
| ------------------------- | ------------- | ------------------------- |
| `PaymentProcessedMessage` | Notifications | `payment_processed_queue` |

Fluxo orientado a eventos, sem dependência direta entre serviços.

---

## 🔄 Fluxo de Processamento

1. Serviço de catálogo publica `OrderPlacedMessage`
2. Payments API consome o evento
3. Pagamento é processado (regra simulada)
4. Evento `PaymentProcessedMessage` é publicado
5. Serviços downstream reagem ao evento

---

## 🔌 Endpoints (Carter + CQRS)

| Método | Rota           | Descrição               |
| ------ | -------------- | ----------------------- |
| POST   | /payments      | Criar pagamento         |
| GET    | /payments      | Listar pagamentos       |
| GET    | /payments/{id} | Buscar pagamento por ID |
| PUT    | /payments/{id} | Atualizar pagamento     |
| DELETE | /payments/{id} | Remover pagamento       |

---

## ⚙️ Configuração

### appsettings.json (exemplo)

```json
{
  "ConnectionStrings": {
    "Database": "Server=localhost;Port=5433;Database=PaymentDb;User Id=postgres;Password=***;"
  },
  "RabbitSettings": {
    "HostName": "localhost",
    "QueueName": "payment_processed_queue",
    "QueueNameConsumer": "order_placed_queue",
    "StartConsumer": true
  }
}
```

---

## 🔐 Variáveis de Ambiente

```text
ConnectionStrings__Database
RabbitSettings__HostName
RabbitSettings__Username
RabbitSettings__Password
RabbitSettings__QueueName
RabbitSettings__QueueNameConsumer
RabbitSettings__StartConsumer
```

---

## 🧠 Design Decisions

* CQRS aplicado para separar leitura e escrita
* MediatR usado para orquestração de handlers
* MassTransit abstrai a mensageria
* RabbitMQ garante desacoplamento entre domínios
* Serviço consome e publica eventos por design

---

## 🐳 Docker

Build:

```bash
docker build -t payments-api -f Service/Payments/Payments.API/Dockerfile .
```

Run:

```bash
docker run -p 8080:8080 \
  -e ASPNETCORE_URLS=http://+:8080 \
  payments-api
```

---

## ▶️ Executando Localmente

Pré-requisitos:

* .NET SDK 8
* PostgreSQL
* RabbitMQ
* Docker (opcional)

Run:

```bash
dotnet restore
dotnet run --project Service/Payments/Payments.API/Payments.API.csproj
```

Swagger disponível automaticamente em ambiente Development.

---

## 🧩 Extensibilidade

* Novos métodos de pagamento podem ser adicionados sem impacto externo
* Regras de processamento isoladas em UseCases
* Fácil adição de retry, DLQ e observabilidade

---

## 🚫 Fora do Escopo (intencional)

* ❌ Integração real com gateways de pagamento
* ❌ Dados sensíveis
* ❌ Tokens ou credenciais

---

## 📄 Licença

Projeto para fins educacionais e demonstrativos.

---

**Serviço orientado a eventos, previsível e preparado para escala.**
