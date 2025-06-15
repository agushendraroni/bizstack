# BizStack

**BizStack** is a reusable monorepo architecture for rapidly building MVPs for small to medium-sized businesses. It's designed to be modular, extensible, and production-ready from day one.

## Fitur

- 🚀 Ringan dan cepat
- 🔄 Reaktif
- 📦 Portabel dan mudah dikembangkan
- 🧩 Struktur monorepo: frontend, backend (.NET), automasi (n8n)
- 🛡️ Dukungan JWT Auth, Swagger, dan modular microservices

Clone repositori ini:

```bash
git clone https://github.com/agushendraroni/Bizstack.git
cd Bizstack
```

Jalankan layanan dengan Docker Compose:

```bash
docker-compose up -d
```

## 🚀 Services Overview

| Service              | Status | Description                                           |
|----------------------|--------|-------------------------------------------------------|
| `auth-service`       | ✅      | Login, JWT, Refresh Token                            |
| `user-service`       | ✅      | Admin, Kasir, Karyawan, Guru                         |
| `product-service`    | ✅      | Barang, Modul Kursus, Layanan                        |
| `settings-service`   | ✅      | Nama toko, organisasi, tenant                        |
| `customer-service`   | ✅      | Pelanggan toko, siswa, client                        |
| `notification-service` | ✅    | Email, WhatsApp, Push Notification                   |
| `report-service`     | ✅      | Semua sistem perlu laporan                           |
| `file-storage-service` | ✅    | Upload struk, logo, foto produk                      |

## 📦 Technologies Used

- .NET 8 (C#)
- PostgreSQL
- AutoMapper, FluentValidation
- JWT Authentication
- Minimal API / RESTful Controllers
- n8n (for automation workflows)
- React (Shards Dashboard for frontends)

---

## ✅ Goals

- MVP-ready for UMKM (retail, edukasi, bengkel, reseller, kursus)
- Dapat di-reuse untuk bisnis lainnya
- Developer-friendly structure
- Scalable & maintainable codebase

---

## 🛠 Setup Instructions

Coming soon...

---

## Kontribusi

1. Fork repositori ini
2. Buat branch fitur Anda (`git checkout -b fitur-anda`)
3. Commit perubahan Anda (`git commit -m 'Tambah fitur A'`)
4. Push ke branch Anda (`git push origin fitur-anda`)
5. Ajukan Pull Request

## Lisensi

MIT License © [agushendraroni](https://github.com/agushendraroni)
