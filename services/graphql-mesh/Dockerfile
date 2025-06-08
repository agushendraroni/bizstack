# Gunakan node image yang ringan
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Salin file konfigurasi dan dependency
COPY package*.json ./
COPY .meshrc.yaml ./

# Install dependencies
RUN npm install

# Salin seluruh project (termasuk node_modules dan mesh.config.yaml)
COPY . .

# Build jika diperlukan (opsional, tergantung setup Mesh kamu)
# RUN npm run build

# Jalankan GraphQL Mesh
CMD ["npm", "run", "mesh:dev"]
