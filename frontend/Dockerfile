# Stage 2: Serve with Nginx
FROM nginx:alpine AS production
# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy built files
COPY ./dist /usr/share/nginx/html

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
