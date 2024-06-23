#!/bin/bash
set -e

docker compose -f docker-compose-unit-tests.yml build
docker compose -f docker-compose-integration-tests.yml build
docker compose -f docker-compose-acceptance-tests.yml build
