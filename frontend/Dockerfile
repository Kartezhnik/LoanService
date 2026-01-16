FROM node:20-alpine AS build-stage
WORKDIR /app

# 1. Copy package files
COPY package*.json ./

# 2. Install everything (including devDeps and element-plus if added to package.json)
# If you don't want to run npm install locally, you can change this to:
RUN npm install 

# 3. Copy source
COPY . .

# 4. Build
RUN npm run build

# --- Stage 2: Nginx ---
FROM nginx:stable-alpine AS production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Конфиг Nginx без "стирания" пути /api/
RUN echo 'server { \
    listen 80; \
    server_name localhost; \
    resolver 127.0.0.11 valid=30s; \
    location / { \
        root /usr/share/nginx/html; \
        index index.html index.htm; \
        try_files $uri $uri/ /index.html; \
    } \
    location /api/ { \
        set $backend_url http://loanserviceapi:8080; \
        # УБРАЛИ слеш после переменной! \
        proxy_pass $backend_url; \
        proxy_set_header Host $host; \
        proxy_set_header X-Real-IP $remote_addr; \
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]