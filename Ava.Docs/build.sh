#!/usr/bin/env bash

docker build -t hugo-doks-dev .
docker builder prune -f
docker image prune -f
