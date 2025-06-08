# Auth Service - FrameworkX

**Auth Service** merupakan bagian dari monorepo `frameworkX` yang menangani otentikasi dan otorisasi pengguna. Layanan ini dibangun menggunakan .NET dan menyediakan fitur seperti login, refresh token, manajemen role, permission, dan integrasi JWT untuk pengamanan endpoint.

## 🚀 Fitur Utama

- 🔐 JWT Authentication (Login, Refresh Token)
- 🧑‍🤝‍🧑 Manajemen User, Role, dan Permission
- 🧾 Audit Trail (`CreatedAt`, `ChangedAt`, `CreatedBy`, `ChangedBy`)
- 🗑️ Soft Delete
- 📄 Pagination, Sorting, dan Filtering Lanjutan
- 📚 Dokumentasi Swagger
- ✅ Unit Test Terstruktur

## 📁 Struktur Direktori

auth-service/
├── Controllers/         # Endpoint HTTP untuk model-model (User, Role, dsb)
├── Dtos/                # DTO: CreateRequest, UpdateRequest, FilterRequest, Response
├── Interfaces/          # Interface untuk service-service
├── Models/              # Entity dan model database
├── Services/            # Implementasi bisnis logic
├── Middleware/          # Middleware untuk JWT
├── Helpers/             # Helper (e.g. hashing, token generation)
├── Extensions/          # Service injection & konfigurasi
├── Program.cs           # Entry point aplikasi
├── appsettings.json     # Konfigurasi aplikasi
└── ...                  # File dan folder lainnya

## ⚙️ Konfigurasi & Menjalankan

### 1. Persiapan

- Pastikan sudah menginstal:
  - [.NET SDK 8.0+](https://dotnet.microsoft.com/)
  - PostgreSQL (jika digunakan untuk penyimpanan)
  - [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### 2. Konfigurasi `appsettings.json`

{
  "JwtSettings": {
    "Secret": "your_super_secret_key",
    "Issuer": "frameworkX-auth",
    "Audience": "frameworkX-clients",
    "AccessTokenExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 7
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=authdb;Username=youruser;Password=yourpassword"
  }
}

### 3. Migrasi Database

dotnet ef database update

### 4. Menjalankan Aplikasi

dotnet run

Swagger UI tersedia di:  
https://localhost:<port>/swagger

## 🛠️ Endpoint Penting

| Endpoint               | Method | Deskripsi                  | Autentikasi |
|------------------------|--------|----------------------------|-------------|
| `/api/auth/login`      | POST   | Login dan dapatkan token   | ❌          |
| `/api/auth/refresh`    | POST   | Refresh JWT token          | ❌          |
| `/api/users`           | CRUD   | Manajemen user             | ✅          |
| `/api/roles`           | CRUD   | Manajemen role             | ✅          |
| `/api/permissions`     | CRUD   | Manajemen permission       | ✅          |
| `/api/menus`           | CRUD   | Manajemen menu navigasi    | ✅          |

## 🧪 Testing

Unit test tersedia di folder `Tests/` (jika sudah dibuat). Jalankan dengan:

dotnet test

## 🧰 Teknologi yang Digunakan

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (opsional)
- JWT Bearer Authentication
- Swagger / Swashbuckle
- xUnit (untuk unit test)

## 🤝 Kontribusi

Pull request dipersilakan. Untuk perubahan besar, silakan buka *issue* terlebih dahulu untuk mendiskusikan apa yang ingin Anda ubah.

## 📄 Lisensi

Proyek ini dilisensikan di bawah [MIT License](LICENSE).
