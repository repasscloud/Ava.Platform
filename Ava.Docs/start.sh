#!/usr/bin/env bash

EXPORT PROJECT_NAME=AvaPlatform.docs

docker run --rm -it \
  -v "$PWD:/usr/src/app/${PROJECT_NAME}" \
  -w /app \
  -p 1313:1313 \
  hugo-doks-dev
