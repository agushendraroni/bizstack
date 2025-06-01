# Tahap Build
FROM node:18-alpine AS builder
WORKDIR /app

# Copy hanya file yang dibutuhkan untuk menginstal dependensi terlebih dahulu
COPY package*.json ./

# Install dependensi lebih dulu untuk memanfaatkan layer caching
RUN npm install

# Copy semua source code
COPY . .

# Build aplikasi React
RUN npm run build

# Tahap Production
FROM nginx:alpine

# Hapus default konfigurasi nginx jika ada
RUN rm -rf /etc/nginx/conf.d/default.conf

# (Opsional) Copy custom nginx config jika kamu menggunakan React Router (SPA)
# COPY nginx.conf /etc/nginx/conf.d/default.conf

# Salin hasil build React ke direktori html nginx
COPY --from=builder /app/build /usr/share/nginx/html

# Expose port
EXPOSE 80

# Jalankan nginx dalam mode foreground
CMD ["nginx", "-g", "daemon off;"]
