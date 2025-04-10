FROM node:20-alpine

ENV HUGO_VERSION=0.145.0 \
    CI=true \
    PROJECT_NAME=hugo-doks

WORKDIR /tmp

RUN apk add --no-cache curl libc6-compat

RUN npm install -g npm@11.2.0

RUN curl -L -o hugo.tar.gz "https://github.com/gohugoio/hugo/releases/download/v${HUGO_VERSION}/hugo_extended_${HUGO_VERSION}_Linux-ARM64.tar.gz" && \
    tar -xzf hugo.tar.gz hugo && \
    mv hugo /usr/local/bin/ && \
    rm hugo.tar.gz && \
    hugo version && \
    mkdir -p /usr/src/app

RUN npm install -g thulite@latest

RUN npx create-thulite --template doks "$PROJECT_NAME"

RUN cp -r "/tmp/${PROJECT_NAME}/." /usr/src/app/

WORKDIR /usr/src/app

RUN npm install && \
    npm audit fix --force

EXPOSE 1313

CMD ["hugo", "server", "--bind", "0.0.0.0", "--baseURL", "http://localhost", "--disableFastRender"]
