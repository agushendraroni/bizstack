# Tahap Build
FROM node:18-alpine AS builder
WORKDIR /app

# Salin file dependency terlebih dahulu agar cache build bisa dimanfaatkan
COPY package*.json ./

# Install build tools & dependencies
RUN apk add --no-cache python3 make g++ \
    && npm install \
    && apk del make g++

# Salin semua source code ke image
COPY . .

# Set Python untuk environment jika dibutuhkan oleh dependensi native
ENV PYTHON=python3

# Atasi error openssl pada Node 17+
ENV NODE_OPTIONS=--openssl-legacy-provider

# Build aplikasi React
RUN npm run build

# Tahap Production
FROM nginx:alpine

# Hapus konfigurasi default Nginx
RUN rm -rf /etc/nginx/conf.d/default.conf

# (Opsional) Salin custom konfigurasi nginx jika diperlukan (SPA, React Router, dll)
# COPY nginx.conf /etc/nginx/conf.d/default.conf

# Salin hasil build dari tahap builder
COPY --from=builder /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Jalankan nginx
CMD ["nginx", "-g", "daemon off;"]
