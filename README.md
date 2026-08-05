# 🧠 Second Brain — RAG-Powered Personal Knowledge System

A full-stack AI application that transforms your notes, documents, and articles into a queryable knowledge base with **temporal reasoning**, **semantic search**, and **knowledge decay detection**.

Built with **.NET 9**, **Angular 22**, **PostgreSQL + pgvector**, and **Ollama** for fully local AI inference.

---

## ✨ Features

- **Semantic Search** — Ask natural language questions and get AI-generated answers grounded in your knowledge base
- **Temporal Reasoning** — Knowledge is ranked by recency, access frequency, and similarity score
- **RAG Pipeline** — Retrieval-Augmented Generation using local LLMs via Ollama
- **Knowledge Decay** — Detect stale knowledge that hasn't been accessed recently
- **Contradiction Detection** — Automatically flag conflicting information from different time periods
- **Growth Analytics** — Visualize how your knowledge base grows over time
- **Source Management** — Organize, tag, filter, and delete knowledge sources
- **Keyword Search** — Search with date ranges, type filters, and pagination
- **Fully Local AI** — No API keys required — runs entirely on your machine

---

## 🏗️ Architecture

```
Angular 22 Frontend (localhost:4200)
  ↓
.NET 9 Web API (localhost:5163)
  ├── EmbeddingService    → nomic-embed-text (768 dims)
  ├── IngestionService    → chunk + embed + store
  ├── GenerationService   → gemma4:latest
  └── QueryService        → RAG + temporal reranking
  ↓
PostgreSQL + pgvector (localhost:5432)
  ↓
Ollama (localhost:11434)
  ├── nomic-embed-text    → embeddings
  └── gemma4:latest       → text generation
```

### Temporal Reranking Formula

```
Final Score = (Similarity × 0.60) + (Recency × 0.25) + (Access × 0.15)

Similarity  = cosine similarity between query and chunk embeddings
Recency     = exp(-daysSinceIngestion / 365)
Access      = log(accessCount + 1) / 10
```

---

## 🗄️ Database Schema

```
sources           → knowledge sources (title, type, url, tags)
knowledge_chunks  → text chunks with 768-dim vector embeddings
tags              → tag definitions
chunk_tags        → many-to-many chunk ↔ tag
queries           → query history with answers
query_chunk_links → which chunks answered which query
ingestion_jobs    → ingestion status tracking
contradictions    → detected knowledge conflicts
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Angular 22, Tailwind CSS |
| Backend | .NET 9, ASP.NET Core Web API |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL 17 + pgvector |
| Embeddings | nomic-embed-text (768 dims) |
| Generation | gemma4:latest |
| AI Runtime | Ollama (local inference) |
| Containerization | Docker (PostgreSQL) |

---

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [Angular CLI 22](https://angular.io/cli) — `npm install -g @angular/cli`
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Ollama](https://ollama.ai/)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/<your-username>/second-brain.git
cd second-brain
```

### 2. Start PostgreSQL

```bash
docker-compose up -d
```

Verify it's running:

```bash
docker ps
```

### 3. Pull Ollama Models

```bash
ollama pull nomic-embed-text
ollama pull gemma4
```

Start Ollama:

```bash
ollama serve
```

### 4. Configure & Run the API

```bash
cd SecondBrain.API
```

Update `appsettings.json` with your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=secondbrain;Username=postgres;Password=postgres"
  }
}
```

Apply database migrations:

```bash
dotnet ef database update --project SecondBrain.Infrastructure --startup-project SecondBrain.API
```

Start the API:

```bash
dotnet run
```

API runs at `http://localhost:5163`

### 5. Run the Frontend

```bash
cd second-brain-ui
npm install --legacy-peer-deps
ng serve --port 4200
```

Open `http://localhost:4200` in your browser.

---

## 🔌 API Reference

### Brain

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/brain/ingest` | Ingest a new knowledge source |
| `POST` | `/api/brain/query` | Query the knowledge base |

**Ingest Request:**
```json
{
  "content": "Your text content here...",
  "title": "Source Title",
  "type": "note",
  "url": "https://optional-source.com",
  "publishedAt": "2024-01-01T00:00:00Z",
  "tags": ["rag", "ai", "embeddings"]
}
```

**Query Request:**
```json
{
  "question": "What is RAG?",
  "topK": 5
}
```

**Query Response:**
```json
{
  "answer": "RAG (Retrieval-Augmented Generation) is...",
  "sources": [...],
  "contradictions": [...]
}
```

### Sources

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/sources` | List all sources |
| `GET` | `/api/sources/{id}` | Get source details |
| `GET` | `/api/sources/{id}/chunks` | Get chunks for a source |
| `DELETE` | `/api/sources/{id}` | Delete source and chunks |
| `PATCH` | `/api/sources/{id}/tags` | Update source tags |

### Search

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/search` | Keyword search with filters |
| `GET` | `/api/search/sources` | Search sources |
| `GET` | `/api/search/timeline` | Ingestion timeline |

### Insights

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/insights/stats` | Overall statistics |
| `GET` | `/api/insights/growth` | Growth over time |
| `GET` | `/api/insights/decay` | Stale knowledge chunks |
| `GET` | `/api/insights/contradictions` | Knowledge conflicts |
| `PATCH` | `/api/insights/contradictions/{id}/resolve` | Resolve a contradiction |

---

## 🖥️ Frontend Pages

| Route | Component | Description |
|---|---|---|
| `/` | Dashboard | Stats overview, most-used knowledge |
| `/query` | Query | Semantic RAG search interface |
| `/ingest` | Ingest | Add new knowledge sources |
| `/sources` | Sources | Manage all knowledge sources |
| `/insights` | Insights | Growth, decay, contradictions |
| `/search` | Search | Keyword search with filters |

---

## ⚙️ Configuration

### docker-compose.yml

```yaml
services:
  db:
    image: pgvector/pgvector:pg17
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: secondbrain
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
volumes:
  pgdata:
```

### Ollama Models

| Model | Purpose | Dimensions |
|---|---|---|
| `nomic-embed-text` | Text embeddings | 768 |
| `gemma4:latest` | Text generation | — |

---

## 🧪 Testing

### Test Workflow

1. Navigate to `/ingest` and add a few knowledge sources
2. Navigate to `/query` and ask questions about your content
3. Check `/dashboard` for updated stats
4. Use `/search` to filter knowledge by keyword and date
5. View `/insights` for growth and decay analytics
6. Manage sources at `/sources`

### API Testing with curl

```bash
# Test embedding
curl -X POST http://localhost:5163/api/brain/test-embed \
  -H "Content-Type: application/json" \
  -d "\"What is RAG?\""

# Ingest content
curl -X POST http://localhost:5163/api/brain/ingest \
  -H "Content-Type: application/json" \
  -d '{
    "content": "RAG combines retrieval with generation...",
    "title": "RAG Overview",
    "type": "note",
    "tags": ["rag", "ai"]
  }'

# Query the knowledge base
curl -X POST http://localhost:5163/api/brain/query \
  -H "Content-Type: application/json" \
  -d '{"question": "What is RAG?", "topK": 5}'
```

---

## 🚧 Known Limitations

- File upload supports text extraction only (PDF binary parsing not implemented)
- Streaming responses not yet implemented on frontend
- No authentication/authorization
- Single user system

---

## 🗺️ Roadmap

- [ ] Streaming query responses (SSE)
- [ ] PDF binary parsing
- [ ] URL scraping and auto-ingestion
- [ ] YouTube transcript ingestion
- [ ] Authentication and multi-user support
- [ ] Export knowledge base
- [ ] Mobile responsive design
- [ ] Dark/light theme toggle

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---

## 👨‍💻 Author

Built by **Ameya** — Software Developer at GMCS

- GitHub: [@Ameya19](https://github.com/Ameya19)
- LinkedIn: [ameya19](https://www.linkedin.com/in/ameya19/)

---

## 🙏 Acknowledgements

- [pgvector](https://github.com/pgvector/pgvector) — Vector similarity search for PostgreSQL
- [Ollama](https://ollama.ai/) — Local LLM inference
- [nomic-embed-text](https://huggingface.co/nomic-ai/nomic-embed-text-v1) — Open-source embeddings model
- [Tailwind CSS](https://tailwindcss.com/) — Utility-first CSS framework
