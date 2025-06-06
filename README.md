# FrameworkX

FrameworkX adalah kerangka kerja mikro yang sederhana dan cepat untuk membangun aplikasi web reaktif yang dapat berjalan di mana saja.

## Fitur

- 🚀 Ringan dan cepat
- 🔄 Reaktif
- 📦 Portabel dan mudah dikembangkan
- 🧩 Struktur monorepo: frontend, backend (.NET), automasi (n8n)
- 🛡️ Dukungan JWT Auth, Swagger, dan modular microservices

## Instalasi

Pastikan Anda telah menginstal:

- [Git](https://git-scm.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/)
- [Docker](https://www.docker.com/)

Clone repositori ini:

```bash
git clone https://github.com/agushendraroni/frameworkX.git
cd frameworkX
```

Jalankan layanan dengan Docker Compose:

```bash
docker-compose up -d
```

## Struktur Proyek

```
frameworkX/
├── apps/
│   ├── frontend/              # Frontend React (Shards Dashboard)
│   ├── backend/
│   │   ├── auth-service/      # Auth service dengan JWT & unit test
│   │   ├── user-service/      # Manajemen user, Swagger, request model
│   │   └── ...                # Microservice lainnya (document, file, dll)
│   └── automation/            # Workflow n8n
├── .github/                   # CI/CD atau workflow GitHub Actions
├── docker-compose.yml         # Orkestrasi container
└── README.md
```

## Kontribusi

1. Fork repositori ini
2. Buat branch fitur Anda (`git checkout -b fitur-anda`)
3. Commit perubahan Anda (`git commit -m 'Tambah fitur A'`)
4. Push ke branch Anda (`git push origin fitur-anda`)
5. Ajukan Pull Request

## Lisensi

MIT License © [agushendraroni](https://github.com/agushendraroni)
