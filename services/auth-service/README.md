# Auth‑Service

REST API untuk otentikasi & otorisasi dalam arsitektur microservices `frameworkX`.

## 📌 Tujuan

Auth-Service menyediakan:

- Pendaftaran pengguna (signup)
- Login dan logout
- Manajemen token JWT (akses + refresh token)
- Otentikasi berbasis peran (RBAC)
- Endpoint untuk profil yang dilindungi

## 🚀 Fitur

- ASP.NET Core modern (minimal API / WebApplicationBuilder)
- JWT untuk otentikasi
- Refresh token otomatis
- Integrasi Entity Framework Core + SQL Server/PostgreSQL
- Clean Architecture: Controller → Service → Repository → Model/DTO
- Middleware autentikasi/otorisasi
- Swagger & API Explorer

## 🔧 Instalasi

```bash
git clone https://github.com/agushendraroni/frameworkX.git
cd frameworkX/services/auth-service
dotnet restore
```

## ⚙️ Konfigurasi

Tambahkan di `appsettings.Development.json` atau variabel environment:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=AuthDb;User Id=...;Password=...;"
  },
  "JwtSettings": {
    "Issuer": "frameworkX",
    "Audience": "frameworkX",
    "SecretKey": "super–secret–key–here",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## 🎬 Menjalankan

```bash
cd services/auth-service
dotnet run
```

Swagger UI tersedia di `http://localhost:5000/swagger`.

## 🧩 Endpoints Umum

| Method | URL                   | Deskripsi                         | Otentikasi |
|-------:|-----------------------|----------------------------------|:----------:|
| POST   | `/api/auth/signup`    | Daftar pengguna baru             | ❌         |
| POST   | `/api/auth/login`     | Login & terima JWT               | ❌         |
| POST   | `/api/auth/refresh`   | Refresh access token             | ❌         |
| POST   | `/api/auth/logout`    | Logout & invalide refresh token  | ✅         |
| GET    | `/api/auth/me`        | Profil pengguna saat ini         | ✅         |

> ✅ = memerlukan header `Authorization: Bearer <jwt>`

## 🏗️ Arsitektur & Struktur

```
src/
├── Controllers/       ← endpoint HTTP
│   └── AuthController.cs
├── Services/          ← logika bisnis
│   └── AuthService.cs
├── Repositories/      ← akses DB
│   └── UserRepository.cs
├── Models/            ← entity EF Core
│   └── User.cs
├── DTOs/              ← objek data transfer
│   └── LoginRequest.cs
├── Middleware/        ← autentikasi & otorisasi
│   └── JwtMiddleware.cs
├── Extensions/        ← ekstensi service dan konfigurasi
│   └── ServiceCollectionExtensions.cs
├── Data/
│   └── AuthDbContext.cs
├── Program.cs         ← bootstrap .NET 6+
└── appsettings*.json  ← konfigurasi service
```

## ✅ Validasi & Testing

- Unit test (xUnit/Moq) ada di `/tests/AuthService.Tests`
- Pastikan:

  - Signup menghasilkan user baru
  - Login menerima JWT & refresh token
  - Refresh endpoint menghasilkan token baru
  - Logout membatalkan refresh token
  - Endpoint `/me` hanya bisa diakses dengan JWT sah

## 📘 Dokumentasi

Swagger UI: `http://localhost:5000/swagger`.  
Contoh `curl` tersedia di deskripsi endpoint Swagger.

## 🔐 Security & Best Practices

- Simpan secret JWT di *Azure Key Vault* atau *AWS Secrets Manager*
- Pastikan HTTPS selalu aktif
- Gunakan revocation list untuk refresh token
- Implementasi rate-limiting dan IP throttling (opsional)

## 🛠️ Kustomisasi

- Ganti SQL Server dengan PostgreSQL di `AuthDbContext` & `Program.cs`
- Tambah OAuth (Google/Facebook) di `AuthService` & konfigurasi
- Tambah role-based policy di `Program.cs` (`builder.Services.AddAuthorization(...)`)

---

## 📄 Lisensi

MIT — silakan lihat file `LICENSE` untuk detail.

---

### 📝 Contributing

1. Fork repo.
2. Buat branch fitur: `git checkout -b fitur-deskripsi`.
3. Commit perubahan: `git commit -m "Deskripsi perubahan"`.
4. Push ke branch Anda: `git push origin fitur-deskripsi`.
5. Buka pull request.
