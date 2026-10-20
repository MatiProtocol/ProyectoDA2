FROM node:22-alpine AS builder

WORKDIR /app

COPY Frontend/package*.json ./Frontend/
COPY Frontend/angular.json Frontend/tsconfig*.json ./Frontend/

WORKDIR /app/Frontend
RUN npm install

COPY Frontend/. .

RUN npm run build

FROM nginx:alpine

COPY Deploy/DeployFrontend/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=builder /app/Frontend/dist/Frontend/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]