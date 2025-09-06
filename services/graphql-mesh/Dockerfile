FROM node:18-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./
COPY .meshrc.yaml ./

# Install dependencies
RUN npm ci --only=production

# Copy source code
COPY . .

# Build GraphQL Mesh
RUN npm run build

# Expose port
EXPOSE 4000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:4000/healthz || exit 1

# Start the application
CMD ["npm", "start"]
