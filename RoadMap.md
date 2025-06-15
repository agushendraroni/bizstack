# 🧭 FrameworkX Microservices Roadmap

Arsitektur proyek ini mengadopsi pendekatan **microservices modular** untuk memastikan skalabilitas, maintainability, dan pemisahan tanggung jawab yang jelas antar domain.

---

## 📌 Core Services

### 🔐 Auth Service
- **Fungsi**: Autentikasi dan otorisasi berbasis JWT.
- **Fitur**:
  - Login & Refresh Token
  - Role & Permission Management
  - Menu & Access Control
  - Password history & policy
- 📂 Folder: `services/auth-service`

---

### 👤 User Service
- **Fungsi**: Manajemen data pengguna non-authentikasi.
- **Fitur**:
  - Biodata pengguna (profile, kontak)
  - Relasi ke Company / Employee / Preference
- 📂 Folder: `services/user-service`

---

### 🏢 Organization Service
- **Fungsi**: Menyimpan informasi struktur organisasi/perusahaan.
- **Fitur**:
  - Data perusahaan (nama, NPWP, alamat)
  - Struktur hierarki (grup, cabang, unit)
  - Legal documents (SIUP, NIB, dsb)
- 📂 Folder: `services/organization-service`

---

### 👥 HR Service
- **Fungsi**: Manajemen karyawan dan struktur SDM.
- **Fitur**:
  - Employee, Job Title, Department, Position
  - Assignment ke unit organisasi
- 📂 Folder: `services/hr-service`

---

## 📂 Document & Storage Services

### 📄 Document Service *(Planned)*
- **Fungsi**: Abstraksi dokumen bisnis.
- **Fitur**:
  - Metadata dokumen: jenis, status, pemilik
  - Versi dokumen
  - Relasi ke entity (company, user, dsb)
- 📂 Folder: `services/document-service`

---

### 📦 File Storage Service *(Planned)*
- **Fungsi**: Menyimpan file mentah (raw file storage).
- **Fitur**:
  - Upload/download/hapus file
  - Penyimpanan di filesystem / S3 / MinIO
  - Metadata file (size, mime-type, owner)
- 📂 Folder: `services/file-storage-service`

---

## ⚙️ Support & Utility Services

### ⚙️ Preference Service *(Planned)*
- **Fungsi**: Menyimpan preferensi atau pengaturan user/system.
- **Fitur**:
  - Theme, Language, Notification setting
  - Default page, custom field, dsb
- 📂 Folder: `services/preference-service`

---

## 🔧 Shared Library

### 📚 `shared-library`
- Reusable class: DTOs, middleware, JWT utils, hashing, attributes, etc.
- Digunakan lintas service untuk standarisasi.
- 📂 Folder: `shared-library`

---

## 🔁 Automation

### ⚙️ n8n Service
- Orkestrasi automation & data sync.
- 📂 Folder: `automation/n8n`

---

## 📈 Roadmap Next Steps

| Task                                     | Status     |
|------------------------------------------|------------|
| ✅ Setup Auth Service                    | Completed  |
| ✅ Setup User Service                    | Completed  |
| ✅ Setup Organization Service           | Completed  |
| ✅ Setup HR Service                      | Completed  |
| 🟡 Document Service (metadata)          | In Progress|
| 🟡 File Storage Service (low-level)     | In Progress|
| 🔲 Preference Service                   | Planned    |
| 🔲 Audit Logging Service                | Planned    |
| 🔲 Notification Service (email/sms)     | Planned    |
| 🔲 Search/Indexing Service              | Planned    |

---

## 🧠 Notes

- Semua service diatur dengan struktur `-service` dan berada dalam folder `services/`.
- Setiap service memiliki namespace yang konsisten dengan format PascalCase (`AuthService`, `UserService`, dsb).
- Untuk membuat service baru dari template eksisting, gunakan `duplicate-clean-service.sh`.

---

## 📌 Referensi

- Tech Stack: `.NET`, `EF Core`, `PostgreSQL`, `React`, `n8n`, `Docker`
- Deployment: Monorepo, GitHub Actions (planned), Containerized

---

