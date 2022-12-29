FROM node:12.13.0-alpine as angular-build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY library-frontend/package.json /app/package.json
RUN npm install --no-optional
COPY library-frontend /app
RUN npm run build

FROM node:12.13.0-alpine as react-build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY chat-frontend/package.json /app/package.json
RUN npm install --no-optional
COPY chat-frontend /app
RUN npm run build

FROM node:12.13.0-alpine as vue-build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY userprofile-frontend/package.json /app/package.json
RUN npm install --no-optional
COPY userprofile-frontend /app
RUN npm run build

FROM nginx:1.16.0-alpine
COPY --from=angular-build /app/dist/library-frontend /var/www/localhost/openlibrary/angular
COPY --from=react-build /app/build /var/www/localhost/openlibrary/react
COPY --from=vue-build /app/dist /var/www/localhost/openlibrary/vue
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]