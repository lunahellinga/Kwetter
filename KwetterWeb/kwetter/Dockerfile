# Stage 1: Build an Angular Docker Image
FROM node:20-bullseye-slim as build
WORKDIR /app
COPY kwetter/package*.json /app/
RUN npm install
COPY /kwetter /app
RUN npm run build:production -- --output-path=./dist/out
# Stage 2, use the compiled app, ready for production with Nginx
FROM nginx:1.25-alpine
COPY --from=build /app/dist/out/ /usr/share/nginx/html
COPY /nginx-custom.conf /etc/nginx/conf.d/default.conf
