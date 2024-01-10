# Run container
FROM nginx:1.17.1-alpine

RUN apk add bash

# Nginx config
COPY default.conf /etc/nginx/conf.d/default.conf

# Static build
WORKDIR /usr/share/nginx/html
COPY dist/* ./
COPY *.sh ./
COPY .env .

# Add bash and make shell script executable
RUN chmod +x *.sh

# Default port exposure
EXPOSE 8080


# # Start Nginx server
# ENTRYPOINT [ "/opt/scripts/start.sh" ]
CMD ["/usr/share/nginx/html/nginx_start.sh"]
