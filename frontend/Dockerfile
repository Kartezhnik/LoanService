FROM node:20-alpine AS build-stage
WORKDIR /app

COPY package*.json ./

RUN npm install 

COPY . .

RUN npm run build

FROM nginx:stable-alpine AS production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html

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
        proxy_pass $backend_url; \
        proxy_set_header Host $host; \
        proxy_set_header X-Real-IP $remote_addr; \
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]