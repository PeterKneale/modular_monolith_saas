#!/bin/bash
set -e

echo "##########################################"
echo "Running unit tests"
echo "##########################################"
docker compose -f docker-compose-unit.yml build
docker compose -f docker-compose-unit.yml up \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "Running integration tests"
echo "##########################################"
docker compose -f docker-compose-integration.yml build
docker compose -f docker-compose-integration.yml up \
  --abort-on-container-exit \
  --exit-code-from integration-tests
  
echo "##########################################"
echo "Running acceptance tests"
echo "##########################################"
docker compose -f docker-compose-acceptance.yml build
docker compose -f docker-compose-acceptance.yml up \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests
