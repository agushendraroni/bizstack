# 🧭 FrameworkX Microservices Roadmap

Proyek ini menggunakan arsitektur **microservices** berbasis `.NET`, untuk membagi tanggung jawab per domain agar mudah di-scale, dimaintain, dan dikembangkan secara independen.

---

## ✅ Selesai

### 🔐 Auth Service
- **Fungsi**: Autentikasi dan otorisasi pengguna.
- **Status**: ✅ Completed
- **Fitur**:
  - Login, Refresh Token (JWT)
  - Role, Permission, dan User Management
  - Menu dan Akses Kontrol
  - Password History
  - Audit Trail, Soft Delete, Filtering, Pagination
- 📂 Folder: `services/auth-service`

---

## 🏗️ Dalam Proses / Belum Dibuat

### 👤 User Service
- **Fungsi**: Menyimpan data pribadi pengguna (non-auth).
- **Status**: ⏳ Planned
- **Model yang akan dikelola**:
  - `UserProfile` (foto, kontak, alamat)
  - `UserSetting`, `UserPreference` (opsional)
  - Relasi ke `Employee`, `Company`

---

### 🏢 Organization Service
- **Fungsi**: Menyimpan data organisasi/perusahaan.
- **Status**: ⏳ Planned
- **Model**:
  - `Company`, `OrganizationUnit`, `CompanyDocument`

---

### 👥 HR Service
- **Fungsi**: Manajemen data SDM dan struktur kerja.
- **Status**: ⏳ Planned
- **Model**:
  - `Employee`, `JobTitle`, `Department`, `Position`
  - Relasi ke `OrganizationService` (Company/Unit)

---

### ⚙️ Preference Service
- **Fungsi**: Menyimpan pengaturan dan preferensi pengguna.
- **Status**: 🔲 Not Started
- **Model**:
  - `UserPreference` (theme, language, layout)
  - `SystemPreference`, `DefaultSettings`

---

### 📄 Document Service
- **Fungsi**: Menyimpan metadata dokumen.
- **Status**: 🔲 Not Started
- **Model**:
  - `Document`, `DocumentType`, `DocumentVersion`, `Tag`, `ReferenceId`, `OwnerType`

---

### 📦 File Storage Service
- **Fungsi**: Menyimpan file binary (PDF, gambar, dsb).
- **Status**: 🔲 Not Started
- **Model**:
  - `StoredFile`, `FileGroup`, `StorageProvider`
- **Catatan**: Hanya menyimpan path & metadata, file disimpan di S3 / MinIO / local.

---

## 📚 Shared Library

### 🧩 `shared-library`
- **Fungsi**: Menyediakan reusable komponen:
  - DTO, middleware, JWT, password hashing
  - Audit attributes, base entities
- 📂 Folder: `shared-library`
- **Status**: 🛠 Digunakan aktif oleh AuthService

---

## 🧠 Roadmap Pengembangan

| Service               | Status       | Catatan                            |
|------------------------|--------------|-------------------------------------|
| Auth Service           | ✅ Completed | Sudah lengkap dengan JWT, audit     |
| User Service           | 🔲 Planned   | Akan handle data pribadi user       |
| Organization Service   | 🔲 Planned   | Struktur perusahaan & unit kerja    |
| HR Service             | 🔲 Planned   | Employee & posisi kerja             |
| Preference Service     | 🔲 Planned   | Preferensi UI/UX pengguna           |
| Document Service       | 🔲 Planned   | Metadata dokumen legal/internal     |
| File Storage Service   | 🔲 Planned   | Penyimpanan file fisik (S3/MinIO)   |

---

## 📌 Catatan Penggunaan

- Semua service disimpan di dalam folder `services/`
- Namespace disesuaikan dengan nama PascalCase: `AuthService`, `UserService`, dst.
- Untuk kloning service dari template eksisting, gunakan script: `duplicate-clean-service.sh`
- Semua service akan menggunakan JWT Auth, Swagger, dan Middleware Standar

---

## 🔧 Stack Teknologi

- Backend: `.NET`, `EF Core`, `PostgreSQL`
- Frontend: `React` + `Shards Dashboard`
- Automasi: `n8n`
- Container: `Docker`, `Docker Compose`
- Dev Tools: `shell script`, `GitHub`, `monorepo`

---

