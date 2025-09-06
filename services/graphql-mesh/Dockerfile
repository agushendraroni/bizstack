FROM node:20-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./
COPY .meshrc.yaml ./

# Install dependencies
RUN npm install

# Copy source code
COPY . .

# Expose port
EXPOSE 4000

# Start in dev mode (runtime schema loading)
CMD ["npm", "run", "dev"]
