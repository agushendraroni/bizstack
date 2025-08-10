
---

## 🛠 Teknologi

- .NET 8
- C#
- PostgreSQL
- React.js (Shards Dashboard)
- n8n Automation
- Docker & Docker Compose

---

## 📌 Catatan

- Semua service bersifat loosely coupled.
- Gunakan `shared-library` untuk kode yang dapat digunakan lintas service.
- Setiap service memiliki namespace dan database sendiri.

---

## 📅 Roadmap Pengembangan

| Service              | Status       | Catatan                      |
|----------------------|--------------|-------------------------------|
| Auth Service         | ✅ Selesai    | Sudah ada unit test & JWT     |
| User Service         | ⏳ Proses     | Profil user & preference      |
| Organization Service | ⏳ Proses     | Struktur perusahaan            |
| HR Service           | 🆕 Baru       | Data pegawai                  |
| File Storage         | ❌ Belum      | S3 / local storage            |
| Document Service     | ❌ Belum      | Dokumen & approval            |
| Preference Service   | ❌ Belum      | Preferensi user/sistem        |
| Notification Service | ❌ Belum      | Email / in-app notification   |
| Audit Service        | ❌ Belum      | Entity change log             |
| Search Service       | ❌ Belum      | Full-text search              |

---

> Made with 💡 by Agus & Team - `frameworkX`
